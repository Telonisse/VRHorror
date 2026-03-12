using UnityEngine;

[CreateAssetMenu(menuName = "Story/Conditions/LampIsOn")]
public class LampIsOnCondition : StoryCondition
{
    public LampPlaceholder lamp;

    public override bool IsMet()
    {
        return true;
    }
}
