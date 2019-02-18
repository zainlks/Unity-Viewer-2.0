using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotView : Singleton<RobotView> {

    public GameObject x;

    private InfoWindowController InfoWindow;

    private Dictionary<String, Action> actions;

    // Use this for initialization
    private void Awake() {

        actions = new Dictionary<String, Action> {
            {"Tap", delegate { Tap(); } },
        };

        foreach (KeyValuePair<String, Action> item in actions) {

            ActionEvent actionEvent = gameObject.AddComponent<ActionEvent>() as ActionEvent;
            actionEvent.Init(item.Key, item.Value);
        }       
        //InfoWindow.gameObject.SetActive(false);
    }

    // Changed tap from IEnumerator to void
    private void Tap() {

        RaycastHit hit;
        Ray rayOut = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

        Debug.DrawRay(rayOut.origin, rayOut.direction, Color.red);
        if (Physics.Raycast(rayOut, out hit)) {
            Debug.Log(hit.transform.name);
            InfoWindow.gameObject.SetActive(true);
        }
        else {
            InfoWindow.gameObject.SetActive(false);
        }
    }
}
