using UnityEngine;

public class ExplosionShooter : IService
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
                IDetonateble detonateble = target.GetComponentInParent<IDetonateble>();

                if (detonateble != null)
                {
                    Vector3 forceDirection = (detonateble.Transform.position - _explosionPosition) + Vector3.up;

                    detonateble.OnDetonate(forceDirection, _strengthExplosion);
                    _explosionViewVfx.PlayVfx(_explosionPosition);
                }
            }
        }
    }
    
}
