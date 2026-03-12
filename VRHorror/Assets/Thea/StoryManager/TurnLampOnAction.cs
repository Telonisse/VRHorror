using UnityEngine;

[CreateAssetMenu(menuName = "Story/Actions/Turn Lamp On")]
public class TurnLampOnAction : StoryAction
{
    public LampController Lamp;
    public override void Execute()
    {
        Debug.Log("naah");
        Lamp.TurnOff();
    }
}
