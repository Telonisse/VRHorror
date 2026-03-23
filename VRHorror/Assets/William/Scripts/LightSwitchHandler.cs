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
    }

    void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(ToggleSwitch);
        }
    }
}