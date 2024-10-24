using UnityEngine;

public class InputHandler 
{
    public Vector3 MousePosition { get; private set; }

    public bool IsPressLeftButtonMouse { get; private set; }
    public bool IsClickRightButtonMouse { get; private set; }

    public void Update()
    {
        IsPressLeftButtonMouse = Input.GetMouseButton(0);
        IsClickRightButtonMouse = Input.GetMouseButtonDown (1);

        MousePosition = Input.mousePosition; 
    }
}
