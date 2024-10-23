using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [field: SerializeField] public Vector3 MousePosition;    

    [field: SerializeField] public bool IsPressLeftButtonMouse { get; private set; }
    [field: SerializeField] public bool IsClickRightButtonMouse { get; private set; }

    public void Update()
    {
        IsPressLeftButtonMouse = Input.GetMouseButton(0);
        IsClickRightButtonMouse = Input.GetMouseButtonDown (1);

        MousePosition = Input.mousePosition; 
    }


}
