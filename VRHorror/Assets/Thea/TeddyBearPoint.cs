using Oculus.Interaction.HandGrab;
using UnityEngine;

public class TeddyBearPoint : MonoBehaviour
{
    public Transform armBone;
    public Transform target;
    public float rotateSpeed = 10f;
    public MonoBehaviour ezBoneScript;

    bool shouldPoint = false;

    private void Start()
    {
        PointAtTarget(target); // Needs to be triggered elsewhere, just test trigger for now, also needs to be added that is to turn of the point but thats easy :3 
    }
    public void PointAtTarget(Transform newTarget)
    {
        target = newTarget;
        shouldPoint = true;
        ezBoneScript.enabled = false;
    }

    void LateUpdate()
    {
        if (shouldPoint && target != null)
        {
            Vector3 direction = target.position - armBone.position;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion offset = Quaternion.Euler(90, 0, 0);

            Quaternion desiredRotation = targetRotation * offset;

            float maxAngle = 80f; 

            float angle = Quaternion.Angle(transform.rotation, desiredRotation);

            if (angle > maxAngle)
            {
                desiredRotation = Quaternion.RotateTowards(
                    transform.rotation,
                    desiredRotation,
                    maxAngle
                );
            }

            armBone.rotation = Quaternion.Slerp(
                armBone.rotation,
                desiredRotation,
                Time.deltaTime * rotateSpeed
            );
        }
    }
}
