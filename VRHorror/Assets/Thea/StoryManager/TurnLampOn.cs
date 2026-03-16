using UnityEngine;

public class TurnLampOn : MonoBehaviour
{
    public bool interactable;

    [SerializeField] GameObject lamp;

    void Awake()
    {
        interactable = false;

        StoryActionManager.Register(StoryAction.EnableLampButton, EnableButton);
        StoryActionManager.Register(StoryAction.DisableLampButton, DisableButton);
    }

    void EnableButton()
    {
        Debug.Log("Lamp button enabled");
        interactable = true;
    }

    void DisableButton()
    {
        interactable = false;
    }

    public void Press()
    {
        if (!interactable) return;

        lamp.SetActive(true);

        StoryManager.Instance.TriggerEvent(StoryEvent.LightTurnedOn);
    }
}
