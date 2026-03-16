using UnityEngine;
using System.Collections;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;

    [Header("Starting Node")]
    public StoryNode startNode;

    private StoryNode currentNode;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (startNode != null)
            EnterNode(startNode);
        else
            Debug.LogWarning("Start node not assigned!");
    }

    public void EnterNode(StoryNode node)
    {
        currentNode = node;
        Debug.Log("Entering Node: " + node.nodeName);

        foreach (var action in node.enterActions)
            StoryActionManager.Trigger(action);
    }

    public void ExitNode()
    {
        if (currentNode == null) return;

        foreach (var action in currentNode.exitActions)
            StoryActionManager.Trigger(action);
    }

    public void TriggerEvent(StoryEvent evt)
    {
        if (currentNode == null) return;

        foreach (var transition in currentNode.transitions)
        {
            if (transition.triggerEvent == evt)
            {
                ExitNode();
                if (transition.nextNode != null)
                    EnterNode(transition.nextNode);
                return;
            }
        }
    }
}
