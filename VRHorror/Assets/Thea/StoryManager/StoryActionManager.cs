using System.Collections.Generic;
using UnityEngine;
using System;

public class StoryActionManager : MonoBehaviour
{
    static Dictionary<StoryAction, Action> actions = new();

    public static void Register(StoryAction action, Action callback)
    {
        if (!actions.ContainsKey(action))
            actions[action] = callback;
        else
            actions[action] += callback;
    }

    public static void Trigger(StoryAction action)
    {
        if (actions.ContainsKey(action))
            actions[action]?.Invoke();
    }
}
