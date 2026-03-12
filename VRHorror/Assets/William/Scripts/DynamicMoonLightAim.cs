using UnityEngine;

[ExecuteAlways]
public class DynamicMoonlightAim : MonoBehaviour
{
    public Transform targetSwitch;
    public Light moonSpotlight;
    public Transform volumetricShaft;

    [Range(0.1f, 5f)]
    public float beamThickness = 1f;

    void Update()
    {
        if (targetSwitch == null) return;

        transform.LookAt(targetSwitch);
        float distance = Vector3.Distance(transform.position, targetSwitch.position);

        if (moonSpotlight != null)
        {
            moonSpotlight.range = distance + 1f;
        }

        if (volumetricShaft != null)
        {
            volumetricShaft.localScale = new Vector3(beamThickness, beamThickness, distance);
        }
    }
}