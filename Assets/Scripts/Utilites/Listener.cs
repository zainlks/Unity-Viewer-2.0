using System;
using System.Collections.Generic;
using UnityEngine;

public class Listener : Singleton<Listener> {

    private Dictionary<string, Action> actions;
}