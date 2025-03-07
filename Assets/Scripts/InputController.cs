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
                // Dokunulan noktan�n ekran�n sol yar�s�nda olup olmad���na bak�yoruz
                if (touch.position.x < Screen.width / 2)
                    OnPlayerPressedLeftButtonEvent?.Invoke();
                else
                    OnPlayerPressedRightButtonEvent?.Invoke();
            }
        }
        // Edit�rde test i�in fare t�klamas� kontrol�
        else if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.x < Screen.width / 2)
                OnPlayerPressedLeftButtonEvent?.Invoke();
            else
                OnPlayerPressedRightButtonEvent?.Invoke();
        }
    }
}
