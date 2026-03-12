using UnityEngine;

public class LampController : MonoBehaviour
{
    public GameObject lampLight;

    public void TurnOn()
    {
        Debug.Log("noo this");
        lampLight.SetActive(true);
    }

    public void TurnOff()
    {
        Debug.Log("helu i did this");
        lampLight.SetActive(false);
    }
}
