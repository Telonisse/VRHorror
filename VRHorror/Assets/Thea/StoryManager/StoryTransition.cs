using System;
using UnityEngine;

[System.Serializable]
public class StoryTransition
{
    public StoryEvent triggerEvent;
    public StoryNode nextNode; 
}
