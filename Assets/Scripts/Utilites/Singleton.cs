using UnityEngine;

/// <summary>
/// Inherit from this base class to create a singleton.
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// Taken from: http://wiki.unity3d.com/index.php/Singleton
/// Licensed under Creative Common's Attribution-ShareAlike 3.0 Unported (CC BY-SA 3.0).
/// </summary>

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

    private static bool shuttingDown = false;
    private static object _lock = new Object();
    private static T _instance;

    public static T Instance {
        get {

            if (shuttingDown) {

                Debug.LogWarning($"Shutting down! {typeof(T)} will be destroyed, returning null");
                return null;
            }

            lock (_lock) {

                if (_instance == null) {

                    _instance = FindObjectOfType<T>();

                    if (_instance == null) {

                        GameObject singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<T>();
                        singletonObject.name = $"{typeof(T).ToString()} (Singleton)";
                    }
                    DontDestroyOnLoad(_instance);
                }
                return _instance;
            }
        }
    }

    public virtual void OnApplicationQuit() {

        shuttingDown = true;
    }

    public virtual void OnDestroy() {

        shuttingDown = true;
    }
}