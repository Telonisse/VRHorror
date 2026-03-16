using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StoryNode
{
    public string nodeName;

    public List<StoryAction> enterActions = new();
    public List<StoryAction> exitActions = new();

    public List<StoryTransition> transitions = new();
}
