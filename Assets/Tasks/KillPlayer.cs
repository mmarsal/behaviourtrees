using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom")]
public class KillPlayer : Action
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
        playerScript.GameOver();
        return TaskStatus.Success;
    }
}
