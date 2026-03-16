using UnityEngine;
using System.Collections;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;

    public StoryGraph graph;

    int currentNode;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        EnterNode(graph.startNode);
    }

    void EnterNode(int index)
    {
        currentNode = index;

        StoryNode node = graph.nodes[index];

        Debug.Log("Entering Node: " + node.nodeName);

        foreach (var action in node.enterActions)
            StoryActionManager.Trigger(action);
    }

    void ExitNode()
    {
        StoryNode node = graph.nodes[currentNode];

        foreach (var action in node.exitActions)
            StoryActionManager.Trigger(action);
    }

    public void TriggerEvent(StoryEvent evt)
    {
        StoryNode node = graph.nodes[currentNode];

        foreach (var transition in node.transitions)
        {
            if (transition.triggerEvent == evt)
            {
                ExitNode();
                EnterNode(transition.nextNode);
                return;
            }
        }
    }
}
