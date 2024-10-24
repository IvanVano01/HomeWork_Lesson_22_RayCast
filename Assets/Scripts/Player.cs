using UnityEngine;

public class Player : MonoBehaviour
{ 
    private IService _service;   

    private void Update()
    {        
        if (_service == null)
            return;

        _service.Update();
    }

    private void FixedUpdate()
    { 
        if (_service == null)
            return;

        _service.FixUpdate();
    }   

    public void SetService(IService service) => _service = service;    
}
