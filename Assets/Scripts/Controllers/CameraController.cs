using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CameraController : MonoBehaviour {

    public Transform target;
    public float speed = 5.0f;
    public float closetDistance = 3.0f;
    public float furthestDistance = 7.0f;

    private Dictionary<String, Action> actions;

    private void Awake() {

        // Mayve make the following code a static method of ActionEvent
        // and just pass the dictionary
        actions = new Dictionary<String, Action> {
            {"Drag", delegate { StartCoroutine(FollowDrag()); } },
            {"Zoom", delegate { StartCoroutine(Zoom()); } }
        };

        foreach (KeyValuePair<String, Action> item in actions) {

            ActionEvent actionEvent = gameObject.AddComponent<ActionEvent>() as ActionEvent;
            actionEvent.Init(item.Key, item.Value);
        }
    }

    // Actions that take place over time should use IEnumerators with a while loop
    private IEnumerator FollowDrag() {

        Vector2 touchPosition = new Vector2();

        //Moved x, y to Vecto2 implentation

        while (TouchManager.Instance.Dragging) {

            touchPosition.x = Input.GetTouch(0).deltaPosition.x * speed * Time.deltaTime;
            touchPosition.y = Input.GetTouch(0).deltaPosition.y * speed * Time.deltaTime;

            transform.RotateAround(target.position, transform.up, touchPosition.x);
            transform.RotateAround(target.position, transform.right, -touchPosition.y);

            transform.LookAt(target, Vector3.up);

            yield return null;
        }
    }

    private IEnumerator Zoom() {

        Vector3 distanceToTarget;

        // Do calculation of moveDistance before switch
        Vector3 moveDistance;

        while (TouchManager.Instance.Zooming) {

            distanceToTarget = Vector3.MoveTowards(transform.position, target.position, 1.0f);
            moveDistance = distanceToTarget.normalized / speed;

            switch (TouchManager.Instance.ZoomDirection) {

                case TouchManager.Zoom.IN:
                    if (Vector3.Distance(transform.position, target.position) > closetDistance)
                    transform.position -= moveDistance;
                    break;

                case TouchManager.Zoom.OUT:
                    if (Vector3.Distance(transform.position, target.position) < furthestDistance)
                        //transform.position += distanceToTarget.normalized / speed;
                        transform.position += moveDistance;
                    break;
            }

            yield return null;
        }
    }
}
