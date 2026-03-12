using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        StoryManager.instance.TriggerEvent(StoryEvent.GameStart);
    }
}
