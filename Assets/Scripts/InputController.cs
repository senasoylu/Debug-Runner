using UnityEngine;

public class InputController : MonoBehaviour
{
    public delegate void OnPlayerPressedLeftButtonDelegate();
    public static OnPlayerPressedLeftButtonDelegate OnPlayerPressedLeftButtonEvent;

    public delegate void OnPlayerPressedRightButtonDelegate();
    public static OnPlayerPressedRightButtonDelegate OnPlayerPressedRightButtonEvent;
   
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                // Dokunulan noktanýn ekranýn sol yarýsýnda olup olmadýðýna bakýyoruz
                if (touch.position.x < Screen.width / 2)
                    OnPlayerPressedLeftButtonEvent?.Invoke();
                else
                    OnPlayerPressedRightButtonEvent?.Invoke();
            }
        }
        // Editörde test için fare týklamasý kontrolü
        else if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.x < Screen.width / 2)
                OnPlayerPressedLeftButtonEvent?.Invoke();
            else
                OnPlayerPressedRightButtonEvent?.Invoke();
        }
    }
}
