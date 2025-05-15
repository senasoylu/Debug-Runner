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
    private float _swipeThreshold = 50f; 

    void Update()
    {
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
                        SwipeRightEvent?.Invoke();
                    else
                        SwipeLeftEvent?.Invoke();
                }
                _swipeStarted = false;
            }
        }

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
                {
                    SwipeRightEvent?.Invoke();
                }

                else
                {
                    SwipeLeftEvent?.Invoke(); 
                }
            }
            _swipeStarted = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
            JumpEvent?.Invoke();
    }
}
