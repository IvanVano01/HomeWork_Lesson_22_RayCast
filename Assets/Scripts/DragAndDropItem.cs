using UnityEngine;

public class DragAndDropItem : IService
{
    private InputHandler _inputHandler;
    private LayerMask _grabbleLayerMask;
    private LayerMask _groundlayerMask;

    private float _rayDistance = 300f;
    private float _groundLevelAxisY = 0.0f;
    private Vector3 _offsetTakingItem;

    private IGrabble _grabble;

    private Plane _dragPlane;
    private Camera _camera;
    private GameObject _viewPlaceMovingOnGround;

    public DragAndDropItem(LayerMask grabbleLayerMask, LayerMask groundlayerMask, InputHandler inputHandler, GameObject viewPlaceMovingOnGroundPrefab)
    {
        _grabbleLayerMask = grabbleLayerMask;
        _groundlayerMask = groundlayerMask;
        _inputHandler = inputHandler;

        _camera = Camera.main;

        _viewPlaceMovingOnGround = viewPlaceMovingOnGroundPrefab;
        _viewPlaceMovingOnGround = GameObject.Instantiate(_viewPlaceMovingOnGround);
        _viewPlaceMovingOnGround.SetActive(false);
    }

    public void Update()
    {
        if (_inputHandler.IsPressLeftButtonMouse)
            SelectByRay();
    }

    public void FixUpdate()
    {
        if (_inputHandler.IsPressLeftButtonMouse == false)        
            Drop();        
    }

    private void SelectByRay()
    {
        Ray cameraRay = _camera.ScreenPointToRay(_inputHandler.MousePosition);
        float planeDist;
        _dragPlane.Raycast(cameraRay, out planeDist);
        Vector3 rayPointCollision = cameraRay.GetPoint(planeDist);

        if (Physics.Raycast(cameraRay, out RaycastHit hitInfo, _rayDistance, _grabbleLayerMask))
        {
            IGrabble grabble = hitInfo.collider.GetComponentInParent<IGrabble>();

            if (grabble != null && _grabble == null)
            {
                _grabble = grabble;
            }

            if (_grabble != null)
            {
                _dragPlane = new Plane(_camera.transform.forward, _grabble.Transform.position);
                _offsetTakingItem = _grabble.Transform.position - rayPointCollision;

                _groundLevelAxisY = GetGroundLevelAxisY(_grabble.Transform.position, Vector3.down, _rayDistance, _groundlayerMask);

                _grabble.OnGrab();
            }
        }

        Drag(rayPointCollision);
    }

    private void Drag(Vector3 rayPointCollision)
    {
        if (_grabble == null)
            return;

        if (_grabble.IsDraggable)
        {
            Vector3 forMovePosition = rayPointCollision + _offsetTakingItem;
            _grabble.OnDrag(forMovePosition, _groundLevelAxisY);

            _viewPlaceMovingOnGround.SetActive(true);
            _viewPlaceMovingOnGround.transform.position = new Vector3(forMovePosition.x, _groundLevelAxisY + 0.01f, forMovePosition.z);
        }
    }

    private void Drop()
    {
        if (_grabble == null)
            return;

        _grabble.OnRelease();
        _viewPlaceMovingOnGround.SetActive(false);
        _grabble = null;
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
