using UnityEngine;

public interface IGrabble
{
    Transform Transform { get; }
    bool IsDraggable { get; }

    void OnGrab();
    void OnDrag(Vector3 position, float groundLevel);
    void OnRelease();

}
