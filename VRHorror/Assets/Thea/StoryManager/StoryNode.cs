using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Story/Story Node")]
public class StoryNode : ScriptableObject
{
    public string nodeName;
    public StoryEvent triggerEvent;
    public UnityEvent actions;
    public StoryNode[] NextNodes;
}
