using UnityEngine;

public class ServiceSwitcher : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private Player _player;
    [SerializeField] private ExplosionViewVfx _explosionViewVfx;
    [SerializeField] private GameObject _viewPlaceMovingOnGroundPrefab;

    [Header("Configs")]
    [SerializeField] private LayerMask _grabbleLayerMask;
    [SerializeField] private LayerMask _groundlayerMask;
    [SerializeField] private float _radiusExplosionCast;

    private InputHandler _inputHandler;
    private DragAndDropItem _dragAndDropItem;
    private ExplosionShooter _explosionShooter;

    private Vector3 _drawZoneExplosion = Vector3.zero;

    private void Awake()
    {
        _inputHandler = new InputHandler();

        _dragAndDropItem = new DragAndDropItem(_grabbleLayerMask, _groundlayerMask, _inputHandler, _viewPlaceMovingOnGroundPrefab);
        _explosionShooter = new ExplosionShooter(_inputHandler, _groundlayerMask, _radiusExplosionCast, _explosionViewVfx);

        _player.SetService(_dragAndDropItem);
    }

    private void Update()
    {
        _inputHandler.Update();

        if (_inputHandler.IsPressLeftButtonMouse)
        {
            _player.SetService(_dragAndDropItem);
        }
        else if (_inputHandler.IsClickRightButtonMouse)
        {
            _player.SetService(_explosionShooter);
        }

        _drawZoneExplosion = _explosionShooter.ExplosionPosition;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_drawZoneExplosion, _radiusExplosionCast);
    }
}
