using UnityEngine;

[CreateAssetMenu(menuName = "Story/Actions/Enable Interaction")]
public class EnableInteractionAction : StoryAction
{
    public LampPlaceholder button;

    public override void Execute()
    {
        button.EnableInteraction();
    }
}
