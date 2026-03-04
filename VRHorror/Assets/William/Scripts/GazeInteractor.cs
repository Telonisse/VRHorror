using UnityEngine;

public class GazeColorCube : MonoBehaviour
{
    public Transform headTransform;
    public Transform gazeIndicator;
    public float maxRayDistance = 100f;

    public float requiredDwellTime = 3f;

    private float dwellTimer = 0f;
    private bool hasTriggered = false;

    void Update()
    {
        if (hasTriggered || headTransform == null || gazeIndicator == null) return;

        Vector3 rayDirection = (gazeIndicator.position - headTransform.position).normalized;

        if (Physics.Raycast(headTransform.position, rayDirection, out RaycastHit hit, maxRayDistance))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                dwellTimer += Time.deltaTime;

                if (dwellTimer >= requiredDwellTime)
                {
                    ChangeColor();
                    hasTriggered = true;
                }
                return;
            }
        }

        dwellTimer = 0f;
    }

    void ChangeColor()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.green;
        }
    }
}