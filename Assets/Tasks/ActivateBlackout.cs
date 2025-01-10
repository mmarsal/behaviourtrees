using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ActivateBlackout : Action
{
    public SharedGameObject Player;
    public SharedFloat Duration = 3f;

    public override TaskStatus OnUpdate()
    {
        PlayerController controller = Player.Value.GetComponent<PlayerController>();
        controller.TriggerBlackout(Duration.Value);
        return TaskStatus.Success;
    }
}