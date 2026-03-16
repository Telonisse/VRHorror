using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoryNode", menuName = "Story/Node")]
public class StoryNode: ScriptableObject
{
    public string nodeName;

    public List<StoryAction> enterActions = new();
    public List<StoryAction> exitActions = new();

    public List<StoryTransition> transitions = new();
}
