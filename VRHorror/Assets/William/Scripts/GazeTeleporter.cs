using UnityEngine;

public class GazeTeleporter : MonoBehaviour
{
    public Transform headTransform;
    public Transform gazeIndicator;
    public float maxRayDistance = 100f;

    public float requiredDwellTime = 3f;
    public Transform teleportDestination;

    private float dwellTimer = 0f;
    private bool isPrimed = false;
    private bool hasTeleported = false;

    void Update()
    {
        if (hasTeleported || headTransform == null || gazeIndicator == null || teleportDestination == null) return;

        if (!isPrimed)
        {
            Vector3 rayDirection = (gazeIndicator.position - headTransform.position).normalized;

            if (Physics.Raycast(headTransform.position, rayDirection, out RaycastHit hit, maxRayDistance))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    dwellTimer += Time.deltaTime;

                    if (dwellTimer >= requiredDwellTime)
                    {
                        isPrimed = true;
                    }
                    return;
                }
            }

            dwellTimer = 0f;
        }
        else
        {
            float angleToObject = Vector3.Angle(headTransform.forward, transform.position - headTransform.position);
            float angleToDestination = Vector3.Angle(headTransform.forward, teleportDestination.position - headTransform.position);

            bool canSeeObject = angleToObject < 75f;
            bool canSeeDestination = angleToDestination < 75f;

            if (!canSeeObject && !canSeeDestination)
            {
                transform.position = teleportDestination.position;
                transform.rotation = teleportDestination.rotation;
                hasTeleported = true;
            }
        }
    }
}