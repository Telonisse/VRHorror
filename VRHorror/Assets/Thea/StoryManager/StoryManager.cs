using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public static StoryManager instance;

    public List<StoryNode> activeNodes = new List<StoryNode>();

    private void Awake()
    {
        instance = this;
    }

    bool ConditionsMet(StoryNode node)
    {
        if (node.conditions == null || node.conditions.Length == 0)
            return true;

        foreach (var condition in node.conditions)
        {
            if (condition == null) continue; // skip null elements safely

            if (!condition.IsMet())
                return false;
        }

        return true;
    }

    public void TriggerEvent(StoryEvent storyEvent)
    {
        Debug.Log("Story Event Triggered: " + storyEvent);

        List<StoryNode> newNodes = new List<StoryNode>();
        List<StoryNode> triggeredNodes = new List<StoryNode>();

        foreach (StoryNode node in activeNodes)
        {
            if (node.triggerEvent == storyEvent && ConditionsMet(node))
            {
                Debug.Log("Node Triggered: " + node.nodeName);

                foreach (var action in node.actions)
                {
                    action.Execute();
                }

                triggeredNodes.Add(node);

                if (node.NextNodes != null)
                {
                    foreach (StoryNode next in node.NextNodes)
                    {
                        Debug.Log("Activating Next Node: " + next.nodeName);
                    }

                    newNodes.AddRange(node.NextNodes);
                }
            }
        }

        foreach (var node in triggeredNodes)
        {
            activeNodes.Remove(node);
        }

        activeNodes.AddRange(newNodes);
    }
}
