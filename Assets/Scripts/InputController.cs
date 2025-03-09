using UnityEngine;

public class InputController : MonoBehaviour
{
    public delegate void OnPlayerPressedLeftButtonDelegate();
    public static OnPlayerPressedLeftButtonDelegate OnPlayerPressedLeftButtonEvent;

    public delegate void OnPlayerPressedRightButtonDelegate();
    public static OnPlayerPressedRightButtonDelegate OnPlayerPressedRightButtonEvent;

    private Vector2 _startTouchPosition;
    private bool _swipeStarted = false;
    private float _swipeThreshold = 50f; // Swipe algýlanmasý için minimum mesafe

    void Update()
    {
        // Dokunmatik cihazlarda swipe kontrolü
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _startTouchPosition = touch.position;
                _swipeStarted = true;
            }
            else if (touch.phase == TouchPhase.Ended && _swipeStarted)
            {
                Vector2 swipeDelta = touch.position - _startTouchPosition;
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y) && Mathf.Abs(swipeDelta.x) > _swipeThreshold)
                {
                    if (swipeDelta.x > 0)
                        OnPlayerPressedRightButtonEvent?.Invoke();
                    else
                        OnPlayerPressedLeftButtonEvent?.Invoke();
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
                    OnPlayerPressedRightButtonEvent?.Invoke();
                else
                    OnPlayerPressedLeftButtonEvent?.Invoke();
            }
            _swipeStarted = false;
        }
    }
}
