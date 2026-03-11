using UnityEngine;
using Bhaptics.SDK2;
using JetBrains.Annotations;

public class bHapticsTestScript : MonoBehaviour
{
    private void Start()
    {
        bhaptics_library.play(BhapticsEvent.DASH);

    }
}
