using UnityEngine;

public interface IDetonateble
{
    Transform Transform { get; }

    void OnDetonate(Vector3 detonateDirection, float strengthExplosion);
}
