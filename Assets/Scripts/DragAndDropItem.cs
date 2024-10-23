using UnityEngine;

public class DragAndDropItem
{  
    private InputHandler _inputHandler;
    private LayerMask _itemlayerMask;
    private LayerMask _groundlayerMask;

    private float _rayDistance = 300f;
    private float _limitAxisY = 0.0f;
    private Vector3 _offsetTakingItem;
    private float _centerOfItemAxisY;

    private GameObject _viewPlaceMovingOnGroundPrefab;
    private GameObject _viewPlaceMovingOnGround;
    private Item _item;
    private Rigidbody _rigidbodyItem;
    private Collider _collider;
    private Plane _dragPlane;
    private Camera _camera;

    public DragAndDropItem(LayerMask itemlayerMask, LayerMask groundlayerMask, InputHandler inputHandler, GameObject viewPlaceMovingOnGroundPrefab)
    {        
        _itemlayerMask = itemlayerMask;
        _groundlayerMask = groundlayerMask;
        _inputHandler = inputHandler;
        _viewPlaceMovingOnGroundPrefab = viewPlaceMovingOnGroundPrefab;

        _camera = Camera.main;
        
        _viewPlaceMovingOnGround = Object.Instantiate(_viewPlaceMovingOnGroundPrefab);
        _viewPlaceMovingOnGround.SetActive(false);
    } 
    
    public void Update()
    {
        if (_inputHandler.IsPressLeftButtonMouse)
        {
            SelectByRay();
        }
    }

    public void FixUpdate()
    {
        if (_inputHandler.IsPressLeftButtonMouse == false)
        {
            Drop();
        }       
    }

    private void SelectByRay()
    {
        Ray cameraRay = _camera.ScreenPointToRay(_inputHandler.MousePosition);
        float planeDist;
        _dragPlane.Raycast(cameraRay, out planeDist);
        Vector3 rayPointCollision = cameraRay.GetPoint(planeDist);

        if (Physics.Raycast(cameraRay, out RaycastHit hitInfo, _rayDistance, _itemlayerMask))
        {
            Collider itemCollider = hitInfo.collider;
            
            _collider = itemCollider;
            _centerOfItemAxisY = _collider.bounds.size.y / 2;
            
            if (_item == null)
            {
                if (itemCollider.GetComponentInParent<Item>())
                    _item = itemCollider.GetComponentInParent<Item>();
                _rigidbodyItem = _item.GetComponent<Rigidbody>();
            }

            if (_item != null)
            {
                _dragPlane = new Plane(_camera.transform.forward, _item.transform.position);
                _offsetTakingItem = _item.transform.position - rayPointCollision;
                _limitAxisY = GetGroundLevelAxisY(_item.transform.position, Vector3.down, _rayDistance, _groundlayerMask);
            }
        }

        Drag(rayPointCollision);
    }

    private void Drag(Vector3 rayPointCollision)
    {
        if (_item == null)
            return;

        _rigidbodyItem.isKinematic = true;
        _rigidbodyItem.Sleep();

        _item.transform.position = (rayPointCollision + _offsetTakingItem); 
        
        if ((_item.transform.position.y - _centerOfItemAxisY) < _limitAxisY)
        {
            _item.transform.position = new Vector3(_item.transform.position.x, _limitAxisY + _centerOfItemAxisY,
                _item.transform.position.z);
        }

        _viewPlaceMovingOnGround.SetActive(true);
        _viewPlaceMovingOnGround.transform.position = new Vector3(_item.transform.position.x, _limitAxisY + 0.01f, _item.transform.position.z);
    }

    private void Drop()
    {
        if (_item == null)
            return;

        _rigidbodyItem.WakeUp();
        _rigidbodyItem.isKinematic = false;

        _viewPlaceMovingOnGround.SetActive(false);
        _item = null;
    }

    private float GetGroundLevelAxisY(Vector3 startPoinRay, Vector3 directRay, float distance, LayerMask layerMaskGround)
    {
        Ray ray = new Ray(startPoinRay, directRay);
        float groundLevelAgxis = 0;

        if (Physics.Raycast(ray, out RaycastHit hitInfo, distance, layerMaskGround))
        {
            Collider collider = hitInfo.collider;

            groundLevelAgxis = collider.transform.position.y;

            Vector3 pointCollisionRay = hitInfo.point;
            Debug.DrawLine(startPoinRay, pointCollisionRay, Color.red);

            return groundLevelAgxis;
        }

        Debug.Log($" Внимание, уровень земли не определён, присвоено значение 0 ");
        return groundLevelAgxis;
    }    
}
