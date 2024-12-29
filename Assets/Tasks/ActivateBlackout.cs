using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ActivateBlackout : Action
{
    public SharedGameObject Player;
    public SharedFloat Duration = 3f;

    public override TaskStatus OnUpdate()
    {
        if (Player == null || Player.Value == null)
        {
            Debug.LogWarning("Player ist nicht zugewiesen.");
            return TaskStatus.Failure;
        }

        PlayerController controller = Player.Value.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.TriggerBlackout(Duration.Value);
            return TaskStatus.Success;
        }
        else
        {
            Debug.LogWarning("PlayerController ist nicht vorhanden.");
            return TaskStatus.Failure;
        }
    }
}