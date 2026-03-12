using UnityEngine;

public class LampPlaceholder : MonoBehaviour
{
    public bool interactionEnabled = false;

    public void EnableInteraction()
    {
        interactionEnabled = true;
        Debug.Log("Lamp button unlocked");
    }

    public void PressButton()
    {
        if (!interactionEnabled)
        {
            Debug.Log("Lamp button is locked");
            return;
        }

        StoryManager.instance.TriggerEvent(StoryEvent.LampButtonPressed);
    }
}
