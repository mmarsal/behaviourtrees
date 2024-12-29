using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom")]
public class KillPlayer : Action
{
    private GameObject player;
    private PlayerMovement playerScript;

    // Start is called before the first frame update
    public override void OnStart()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        playerScript.GameOver();
        return TaskStatus.Success;
    }
}
