using UnityEngine;

public class InputController : MonoBehaviour
{
    public delegate void OnPlayerPressedLeftButtonDelegate();
    public static OnPlayerPressedLeftButtonDelegate OnPlayerPressedLeftButtonEvent;

    public delegate void OnPlayerPressedRightButtonDelegate();
    public static OnPlayerPressedRightButtonDelegate OnPlayerPressedRightButtonEvent;
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {  
            OnPlayerPressedLeftButtonEvent?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            OnPlayerPressedRightButtonEvent?.Invoke();
        }
    }
}
