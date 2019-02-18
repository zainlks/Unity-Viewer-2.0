using System;
using UnityEngine;
using UnityEngine.Events;

class ActionEvent : MonoBehaviour {

    UnityAction unityAction;
    public string eventName;
    public Action callable;

    public void Init(string name, Action method) {

        eventName = name;
        callable = method;
        unityAction = new UnityAction(callable);

        // This must be here otherwise the event will not be fired without
        // toggling the gameobjects state on->off->on
        if (gameObject.activeSelf)
            OnEnable();
    }

    private void OnEnable() {

        if (eventName != null)
            EventManager.StartListening(eventName, unityAction);
    }

    private void OnDisable() {

        if (eventName != null)
            EventManager.StopListening(eventName, unityAction);
    }

}