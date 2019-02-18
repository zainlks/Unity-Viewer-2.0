
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchManager : Singleton<TouchManager> {

    public enum Swipe {

        UP,
        DOWN,
        LEFT,
        RIGHT,
        NONE
    };

    public enum Zoom {
        IN,
        OUT,
        NONE
    }

    public float deadzone = 200.0f;
    public float timeout = 1.0f;
    public float swipeTolerance = 200.0f;
    public float dragActivation = 5.0f;

    public Vector2 ScreenPosition { get; private set; }
    public Swipe SwipeDirection { get; private set; }
    public Zoom ZoomDirection { get; private set; }
    public int Touches { get; private set; }
    public bool Dragging { get; private set; }
    public bool Zooming { get; private set; }

    private float timeDown;
    private Vector2 startPos;
    private float lastZoomDelta;
    private float thisZoomDelta;

    private void Awake() {

        Reset();
    }

    private void Update() {

        if (Input.touchCount > 0) {

            if (Input.touches[0].phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject()) {

                startPos = Input.touches[0].position;
                Touches = Input.touchCount;
            }

            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled) {

                if (timeDown < timeout && !Dragging && !Zooming) {

                    Vector2 endPos = Input.touches[0].position;

                    if (CheckDistance(startPos, endPos) < 15.0f) {

                        // Tap

                        if (Input.touchCount == 1) {

                            EventManager.TriggerEvent("Tap");
                            ScreenPosition = startPos;
                        }
                    }

                    else if (timeDown > 0.25f) {

                        // Swipe only on two more touches
                        if (CheckDistance(startPos, endPos) > deadzone && Input.touchCount > 1) {

                            float distanceBetweenTouches = Vector3.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);

                            if (distanceBetweenTouches < swipeTolerance) {

                                Vector2 delta = endPos - startPos;
                                float x = delta.x;
                                float y = delta.y;

                                if (Mathf.Abs(x) > Mathf.Abs(y)) {

                                    if (x < 0) SwipeDirection = Swipe.LEFT;
                                    else SwipeDirection = Swipe.RIGHT;
                                }

                                else {

                                    if (y < 0) SwipeDirection = Swipe.DOWN;
                                    else SwipeDirection = Swipe.UP;
                                }

                                EventManager.TriggerEvent("Swipe");
                            }
                        }
                    }
                }
                Reset();
            }

            else {

                timeDown += Time.deltaTime;
                // Drag only on 1 touch
                if (Input.touchCount == 1) {

                    switch (Input.GetTouch(0).phase) {

                        case TouchPhase.Stationary:

                            Dragging = false;

                            break;

                        case TouchPhase.Moved:

                            if (!Dragging && Input.GetTouch(0).deltaPosition.magnitude > dragActivation) {

                                Dragging = true;
                                EventManager.TriggerEvent("Drag");
                            }
                            ScreenPosition = Input.touches[0].position;
                            break;
                    }
                }

                if (Input.touchCount == 2) {

                    switch (Input.GetTouch(0).phase) {

                        case TouchPhase.Stationary:

                            if (Input.GetTouch(1).phase == TouchPhase.Moved)
                                goto case TouchPhase.Moved;

                            else
                                Zooming = false;

                            break;

                        case TouchPhase.Moved:

                            thisZoomDelta = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);

                            if (thisZoomDelta != lastZoomDelta && Mathf.Abs(thisZoomDelta - lastZoomDelta) > 25.0f) {

                                if (thisZoomDelta > lastZoomDelta)
                                    ZoomDirection = Zoom.IN;

                                else {

                                    ZoomDirection = Zoom.OUT;
                                }

                                if (!Zooming) {
                                    Zooming = true;
                                    EventManager.TriggerEvent("Zoom");
                                }
                            }
                            lastZoomDelta = thisZoomDelta;
                            break;
                    }
                }
            }
        }
    }

    private float CheckDistance(Vector2 touch1, Vector2 touch2) {

        Vector2 delta = touch2 - touch1;
        return delta.magnitude;
    }

    private void Reset() {

        timeDown = 0.0f;
        startPos = Vector2.zero;
        //Debug.Log("Reset");

        ScreenPosition = Vector2.zero;
        SwipeDirection = Swipe.NONE;
        ZoomDirection = Zoom.NONE;
        Touches = 0;
        Dragging = false;
        Zooming = false;
        lastZoomDelta = 0;
    }
}