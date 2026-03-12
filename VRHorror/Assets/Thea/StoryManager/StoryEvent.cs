using UnityEngine;

public enum StoryEvent 
{
   // Defines all possible events the story system can react too
   // How to trigger event: TriggerEvent(StoryEvent.LampButtonPressed)
   GameStart,
   LampButtonPressed,
   PlayerStartedMoving
}
