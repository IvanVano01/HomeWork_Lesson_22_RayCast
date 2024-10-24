using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour, IGrabble, IDetonateble
{
    private float _centerOfItemAxisY;
    private Rigidbody _rigidbody;

    [SerializeField] private bool _isDraggable;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _centerOfItemAxisY = GetComponentInChildren<Collider>().bounds.size.y / 2;
    }

    public Transform Transform => transform;
    public bool IsDraggable => _isDraggable;

    public void OnGrab()
    {
        _rigidbody.isKinematic = true;
        _rigidbody.Sleep();
    }

    public void OnDrag(Vector3 position, float groundLevel)
    {
        transform.position = position;

        if ((transform.position.y - _centerOfItemAxisY) < groundLevel)
        {
            transform.position = new Vector3(transform.position.x, groundLevel + _centerOfItemAxisY,
                transform.position.z);
        }
    }

    public void OnRelease()
    {
        _rigidbody.WakeUp();
        _rigidbody.isKinematic = false;
    }

    public void OnDetonate(Vector3 detonateDirection, float strengthExplosion)
    {
        _rigidbody.AddForce(detonateDirection * strengthExplosion, ForceMode.Impulse);
    }
}
