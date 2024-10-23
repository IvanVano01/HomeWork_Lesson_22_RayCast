using UnityEngine;

public class ExplosionShooter
{
    private ExplosionViewVfx _explosionViewVfx;
    private InputHandler _inputHandler;
    private LayerMask _groundlayerMask;

    private float _rayDistance = 400f;
    private float _radiusExplosionCast;
    private float _strengthExplosion = 14f;

    private Vector3 _explosionPosition = Vector3.zero;
    private Camera _camera;

    public ExplosionShooter(InputHandler inputHandler, LayerMask groundlayerMask, float radiusExplosionCast, ExplosionViewVfx explosionViewVfx)
    {
        _inputHandler = inputHandler;
        _groundlayerMask = groundlayerMask;
        _radiusExplosionCast = radiusExplosionCast; 
        _explosionViewVfx = explosionViewVfx;

        _camera = Camera.main;
    }

    public void Update()
    {
        if (_inputHandler.IsClickRightButtonMouse)
            CastExplosion();
    }

    public Vector3 ExplosionPosition => _explosionPosition;

    public void CastExplosion()
    {
        Ray ray = _camera.ScreenPointToRay(_inputHandler.MousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, _rayDistance, _groundlayerMask))
        {
            _explosionPosition = hitInfo.point;

            Collider[] targets = Physics.OverlapSphere(_explosionPosition, _radiusExplosionCast);

            foreach (Collider target in targets)
            {
                if (target.GetComponentInParent<Item>() != null)
                {
                    Item item = target.GetComponentInParent<Item>();

                    Vector3 forceDirection = (item.transform.position - _explosionPosition) + Vector3.up;

                    item.GetComponent<Rigidbody>().AddForce(forceDirection * _strengthExplosion, ForceMode.Impulse);
                    _explosionViewVfx.PlayVfx(_explosionPosition);
                }
            }
        }
    }
}
