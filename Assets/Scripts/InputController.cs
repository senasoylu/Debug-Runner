using UnityEngine;

public class InputController : MonoBehaviour
{
    public delegate void SwipeLeftDelegate();
    public static SwipeLeftDelegate SwipeLeftEvent;

    public delegate void SwipeRightDelegate();
    public static SwipeRightDelegate SwipeRightEvent;

    public delegate void JumpDelegate();
    public static JumpDelegate JumpEvent;

    private Vector2 _startTouchPosition;
    private bool _swipeStarted = false;
    private float _swipeThreshold = 50f; // Swipe alg�lanmas� i�in minimum mesafe

    void Update()
    {
        // Dokunmatik cihazlarda swipe kontrol�
        if (Input.touchCount > 0) // ekranda en az bir dokunma varsa d�ng�ye girmesini sa�lar
        {
            Touch touch = Input.GetTouch(0); // dokunmaya ait bilgi i�eren touch nesnesi ve touch de�i�keni ,Input.GetTouch dokunman�n ilkinin indeksini s�f�r al�r

            if (touch.phase == TouchPhase.Began) // touch.phase dokunman�n hangi a�amada old. belirtir touchphase.began parma��n ekrana de�di�i ilk an
            {
                _startTouchPosition = touch.position; // touch.position  dokunman�n ba�lad��� anda  parma��n x,y koordinat�n� verir
                _swipeStarted = true; //swipe hareketi ba�lad�
            }
            else if (touch.phase == TouchPhase.Ended && _swipeStarted)
            {
                Vector2 swipeDelta = touch.position - _startTouchPosition;
                //parma��n ekrana dokundu�u andaki pozisyon ile parma��n kald�r�ld��� andaki pozisyon aras�ndaki fark
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y) && Mathf.Abs(swipeDelta.x) > _swipeThreshold)
                //E�er swipeDelta'n�n x bile�eninin mutlak de�eri, y bile�eninin mutlak de�erinden b�y�k 
                //ve ayn� zamanda swipeDelta'n�n x bile�eninin mutlak de�eri _swipeThreshold de�erinden b�y�kse
                {
                    if (swipeDelta.x > 0)
                        SwipeRightEvent?.Invoke();
                    else
                        SwipeLeftEvent?.Invoke();
                }
                _swipeStarted = false;
            }
        }
        // Edit�r i�in fare ile swipe kontrol�
        else if (Input.GetMouseButtonDown(0))
        {
            _startTouchPosition = Input.mousePosition;
            _swipeStarted = true;
        }
        else if (Input.GetMouseButtonUp(0) && _swipeStarted)
        {
            Vector2 swipeDelta = (Vector2)Input.mousePosition - _startTouchPosition;
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y) && Mathf.Abs(swipeDelta.x) > _swipeThreshold)
            {
                if (swipeDelta.x > 0)
                    SwipeRightEvent?.Invoke();
                else
                    SwipeLeftEvent?.Invoke();
            }
            _swipeStarted = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
            JumpEvent?.Invoke();
    }
}
