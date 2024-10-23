using UnityEngine;

public class ExplosionViewVfx : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosionPrefabVfx;

    public void PlayVfx(Vector3 position)
    {
        Instantiate(_explosionPrefabVfx, position, Quaternion.identity, null);
    }
}
