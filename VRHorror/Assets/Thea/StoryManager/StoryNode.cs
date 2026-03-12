using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Story/Story Node")]
public class StoryNode : ScriptableObject
{
    // A data container for story logic
    public string nodeName; // name for node
    public StoryEvent triggerEvent; // What event activates this node
    public StoryCondition[] conditions; // Optioal condidtion that needs to be met 
    public StoryAction[] actions;
    public StoryNode[] NextNodes; // What nodes get activated after this one has been activated 
}
