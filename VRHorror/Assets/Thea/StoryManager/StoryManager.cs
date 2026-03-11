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

    public void TriggerEvent(StoryEvent storyEvent)
    {
        List<StoryNode> newNodes = new List<StoryNode>();

        foreach (StoryNode node in activeNodes)
        {
            if (node.triggerEvent == storyEvent)
            {
                node.actions.Invoke();

                if (node.NextNodes != null)
                {
                    newNodes.AddRange(node.NextNodes);
                }
            }
        }

        activeNodes.AddRange(newNodes);
    }
}
