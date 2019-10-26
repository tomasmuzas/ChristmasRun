using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    public bool Tap, SwipeLeft, SwipeRight, SwipeUp, SwipeDown;
    public Vector2 StartTouch, SwipeDelta;
    private bool _isDraging;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Tap = SwipeLeft = SwipeRight = SwipeUp = SwipeDown = false;

        if (Input.GetMouseButtonDown(0))
        {
            Tap = true;
            _isDraging = true;
            StartTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Reset();
        }

        if (Input.touches.Length > 0)
        {
            var touchPhase = Input.touches[0].phase;
            if (touchPhase == TouchPhase.Began)
            {
                _isDraging = true;
                Tap = true;
                StartTouch = Input.touches[0].position;
            }
            else if (touchPhase == TouchPhase.Ended || touchPhase == TouchPhase.Canceled)
            {
                Reset();
            }
        }

        SwipeDelta = Vector2.zero;
        if (_isDraging)
        {
            if (Input.touches.Length > 0)
            {
                SwipeDelta = Input.touches[0].position - StartTouch;
            }
            else if (Input.GetMouseButton(0))
            {
                SwipeDelta = (Vector2) Input.mousePosition - StartTouch;
            }
        }

        if (SwipeDelta.magnitude > 100)
        {
            float x = SwipeDelta.x;
            float y = SwipeDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                //Left or right
                SwipeLeft = x < 0;
                SwipeRight = x > 0;
            }
            else
            {
                //Up or down
                SwipeDown = y < 0;
                SwipeUp = y > 0;
            }

            Reset();
        }
    }

    private void Reset()
    {
        StartTouch = SwipeDelta = Vector2.zero;
        _isDraging = false;
    }
}
