using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("Custom")]
public class CheckHidespot : Action
{
    private BehaviorTree behaviorTree;
    private Hidespot hidespotScript;

    private GameObject player;
    private PlayerMovement playerScript;

    public override void OnStart()
    {
        behaviorTree = GetComponent<BehaviorTree>();
        SharedGameObject hidespot = behaviorTree.GetVariable("hidespotToCheck") as SharedGameObject;
        GameObject hidespotGameObject = hidespot.Value;

        hidespotScript = hidespotGameObject.GetComponent<Hidespot>();
        Debug.Log(hidespotScript);

        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerMovement>();
    }

    public override TaskStatus OnUpdate()
    {
        Debug.Log("IS HIDING:" + hidespotScript.playerIsHiding);
        if (hidespotScript.playerIsHiding) {
            playerScript.exposed = true;
            return TaskStatus.Success;
        } else {
            return TaskStatus.Failure;
        }
    }
}
