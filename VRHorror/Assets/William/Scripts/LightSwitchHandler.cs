using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRSimpleInteractable))]
public class LightSwitchHandler : MonoBehaviour
{
    public Light targetLight;
    public Transform switchModel;

    public Vector3 onRotation;
    public Vector3 offRotation;
    public float flipSpeed = 10f;
    public bool startOn = false;

    private bool isOn;
    private Quaternion targetQuaternion;
    private XRSimpleInteractable interactable;

    // ADDDED HERE 
    private void Awake()
    {
        StoryActionManager.Register(StoryAction.TurnLampOn, TurnOn);
        StoryActionManager.Register(StoryAction.TurnLampOff, TurnOff);
        StoryActionManager.Register(StoryAction.EnableLampButton, EnableInteraction);
        StoryActionManager.Register(StoryAction.DisableLampButton, DisableInteraction);
    }
    void Start()
    {
        isOn = startOn;
        targetQuaternion = isOn ? Quaternion.Euler(onRotation) : Quaternion.Euler(offRotation);

        if (targetLight != null) targetLight.enabled = isOn;
        if (switchModel != null) switchModel.localRotation = targetQuaternion;

        interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(ToggleSwitch);
    }

    void Update()
    {
        if (switchModel != null)
        {
            switchModel.localRotation = Quaternion.Slerp(switchModel.localRotation, targetQuaternion, Time.deltaTime * flipSpeed);
        }
    }

    private void ToggleSwitch(SelectEnterEventArgs args)
    {
        isOn = !isOn;

        if (targetLight != null) targetLight.enabled = isOn;

        targetQuaternion = isOn ? Quaternion.Euler(onRotation) : Quaternion.Euler(offRotation);

        //unsure about this, might be redudant and weird
        if (isOn)
            StoryManager.Instance.TriggerEvent(StoryEvent.LightTurnedOn);
        else
            StoryManager.Instance.TriggerEvent(StoryEvent.LightTurnedOff);

    }
    // ADDED HERE
    void TurnOn()
    {
        isOn = true;
        ApplyState();
    }

    void TurnOff()
    {
        isOn = false;
        ApplyState();
    }

    void EnableInteraction()
    {
        interactable.enabled = true;
    }

    void DisableInteraction()
    {
        interactable.enabled = false;
    }

    void ApplyState()
    {
        if (targetLight != null)
            targetLight.enabled = isOn;

        targetQuaternion = isOn
            ? Quaternion.Euler(onRotation)
            : Quaternion.Euler(offRotation);
    }
    // TIL HERE

    void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(ToggleSwitch);
        }

        // ADDED HERE
        StoryActionManager.Unregister(StoryAction.TurnLampOn, TurnOn);
        StoryActionManager.Unregister(StoryAction.TurnLampOff, TurnOff);
        StoryActionManager.Unregister(StoryAction.EnableLampButton, EnableInteraction);
        StoryActionManager.Unregister(StoryAction.DisableLampButton, DisableInteraction);
    }
}