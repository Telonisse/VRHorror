using UnityEngine;
using Bhaptics.SDK2;

public class bHapticsTestScript : MonoBehaviour
{
    private void Awake()
    {
        StoryActionManager.Register(StoryAction.Shiver, StartShiver);
    }
   
    public void StartShiver()
    {
        bhaptics_library.play(BhapticsEvent.SHIVER);
    }

    private void OnDestroy()
    {
        StoryActionManager.Unregister(StoryAction.Shiver, StartShiver);
    }
}
