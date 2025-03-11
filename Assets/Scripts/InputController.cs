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
    private float _swipeThreshold = 50f; // Swipe algýlanmasý için minimum mesafe

    void Update()
    {
        // Dokunmatik cihazlarda swipe kontrolü
        if (Input.touchCount > 0) // ekranda en az bir dokunma varsa döngüye girmesini saðlar
        {
            Touch touch = Input.GetTouch(0); // dokunmaya ait bilgi içeren touch nesnesi ve touch deðiþkeni ,Input.GetTouch dokunmanýn ilkinin indeksini sýfýr alýr

            if (touch.phase == TouchPhase.Began) // touch.phase dokunmanýn hangi aþamada old. belirtir touchphase.began parmaðýn ekrana deðdiði ilk an
            {
                _startTouchPosition = touch.position; // touch.position  dokunmanýn baþladýðý anda  parmaðýn x,y koordinatýný verir
                _swipeStarted = true; //swipe hareketi baþladý
            }
            else if (touch.phase == TouchPhase.Ended && _swipeStarted)
            {
                Vector2 swipeDelta = touch.position - _startTouchPosition;
                //parmaðýn ekrana dokunduðu andaki pozisyon ile parmaðýn kaldýrýldýðý andaki pozisyon arasýndaki fark
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y) && Mathf.Abs(swipeDelta.x) > _swipeThreshold)
                //Eðer swipeDelta'nýn x bileþeninin mutlak deðeri, y bileþeninin mutlak deðerinden büyük 
                //ve ayný zamanda swipeDelta'nýn x bileþeninin mutlak deðeri _swipeThreshold deðerinden büyükse
                {
                    if (swipeDelta.x > 0)
                        SwipeRightEvent?.Invoke();
                    else
                        SwipeLeftEvent?.Invoke();
                }
                _swipeStarted = false;
            }
        }
        // Editör için fare ile swipe kontrolü
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
