using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private ExplosionViewVfx _explosionViewVfx;
    [SerializeField] private GameObject _viewPlaceMovingOnGroundPrefab;
    [SerializeField] private InputHandler _inputHandler;
    
    [Header("Configs")]
    [SerializeField] private LayerMask _itemlayerMask;
    [SerializeField] private LayerMask _groundlayerMask;
    [SerializeField] private float _radiusExplosionCast;

    private DragAndDropItem _dragAndDropItem;
    private ExplosionShooter _explosionShooter;

    private Vector3 _drawZoneExplosion = Vector3.zero;

    private void Awake()
    {
        _dragAndDropItem = new DragAndDropItem(_itemlayerMask, _groundlayerMask, _inputHandler, _viewPlaceMovingOnGroundPrefab);
        _explosionShooter = new ExplosionShooter(_inputHandler, _groundlayerMask, _radiusExplosionCast, _explosionViewVfx);
    }

    private void Update()
    {
        _dragAndDropItem.Update();
        _explosionShooter.Update();
        _drawZoneExplosion = _explosionShooter.ExplosionPosition; 
    }

    private void FixedUpdate()
    {
        _dragAndDropItem.FixUpdate();        
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;        
        Gizmos.DrawWireSphere(_drawZoneExplosion, _radiusExplosionCast);
    }
}
