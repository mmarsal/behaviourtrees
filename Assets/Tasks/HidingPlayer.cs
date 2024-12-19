using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Custom")]
public class HidingPlayer : Action
{

    private GameObject player;
    private PlayerMovement playerScript;

    public override void OnStart()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerMovement>();
    }

    public override TaskStatus OnUpdate()
    {
        if (playerScript.hiding)
        {
            Debug.Log("CAN'T SEE PLAYER");
            return TaskStatus.Failure;
        }

        return TaskStatus.Success;
    }
}
