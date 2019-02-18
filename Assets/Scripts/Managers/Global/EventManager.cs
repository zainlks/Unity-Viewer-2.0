using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Event system to allow the creation and subscriptions to events
/// Taken from: https://unity3d.com/learn/tutorials/topics/scripting/events-creating-simple-messaging-system
/// Created by: Adam Buckner
/// </summary>

public class EventManager : Singleton<EventManager> {

    private Dictionary<string, UnityEvent> eventDictionary = null;
    private Dictionary<string, int> eventListenerCount = null;

    private void Awake() {

        eventDictionary = new Dictionary<string, UnityEvent>();
        eventListenerCount = new Dictionary<string, int>();
    }

    [Obsolete]
    public static void CreateEvent(string name) {

        Instance.eventDictionary.Add(name, new UnityEvent());
    }

    public static void StartListening(string name, UnityAction listener) {

        UnityEvent thisEvent = null;
        try {

            if (Instance.eventDictionary.TryGetValue(name, out thisEvent)) {
                thisEvent.AddListener(listener);
                Instance.eventListenerCount[name] += 1;
            }

            else {

                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                Instance.eventDictionary.Add(name, thisEvent);
            }
        }
        catch (NullReferenceException err) {

            Debug.LogError($"{err.Source} : {err.Message} \n {Instance.eventDictionary} {Instance.eventListenerCount[name]} Listening to {name}");
        }
    }

    public static void StopListening(string name, UnityAction listener) {

        if (Instance == null) return;
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(name, out thisEvent))
            thisEvent.RemoveListener(listener);
    }

    public static void TriggerEvent(string name) {

        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(name, out thisEvent))
            thisEvent.Invoke();
    }
}