using UnityEngine;

public class TurnLampOn : MonoBehaviour
{
    [SerializeField] GameObject lamp;
    public bool interactable;

    void Awake()
    {
        interactable = false;
        StoryActionManager.Register(StoryAction.EnableLampButton, EnableButton);
        StoryActionManager.Register(StoryAction.DisableLampButton, DisableButton);
        StoryActionManager.Register(StoryAction.TurnLampOff, TurnLampOff);
        StoryActionManager.Register(StoryAction.TurnLampOn, TurnLampOnInGame);
    }

    void OnDestroy()
    {
        StoryActionManager.Unregister(StoryAction.EnableLampButton, EnableButton);
        StoryActionManager.Unregister(StoryAction.DisableLampButton, DisableButton);
        StoryActionManager.Unregister(StoryAction.TurnLampOff, TurnLampOff);
        StoryActionManager.Unregister(StoryAction.TurnLampOn, TurnLampOnInGame);
    }

    void EnableButton()
    {
        interactable = true;
        Debug.Log("Lamp button enabled");
    }

    void DisableButton()
    {
        interactable = false;
    }

    void TurnLampOff()
    {
        lamp.SetActive(false);
    }

    void TurnLampOnInGame()
    {
        lamp.SetActive(true);
    }

    public void Press()
    {
        if (!interactable) return;

        if (lamp.activeSelf)
        {
            TurnLampOff();
            StoryManager.Instance.TriggerEvent(StoryEvent.LightTurnedOff); // or maybe create a separate event "LightTurnedOff"
            Debug.Log("Lamp turned OFF");
        }
        else
        {
            TurnLampOnInGame();
            StoryManager.Instance.TriggerEvent(StoryEvent.LightTurnedOn);
            Debug.Log("Lamp turned ON");
        }
    }
}
