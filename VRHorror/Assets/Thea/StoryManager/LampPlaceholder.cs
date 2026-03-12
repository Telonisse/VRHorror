using UnityEngine;

public class LampPlaceholder : MonoBehaviour
{
    public void PressButton()
    {
        StoryManager.instance.TriggerEvent(StoryEvent.LampButtonPressed);
    }
}
