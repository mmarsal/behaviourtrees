using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("Custom")]
public class HidespotsLearner : Action
{
    private GameObject player;
    private PlayerMovement playerScript;
    public float range = 5.0f;      // The range to check against
    private BehaviorTree behaviorTree;

    public override void OnStart()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerMovement>();
        behaviorTree = GetComponent<BehaviorTree>();
    }

    public override TaskStatus OnUpdate()
    {
        // Calculate the distance between the alien and the player
        float distance = Vector3.Distance(transform.position, player.transform.position);

        // Check if the distance is within the range
        if (distance <= range && playerScript.hiding)
        {
            Debug.Log("Target is hiding within range!");
            // Get the current value of the variable "hidespotslearned"
            SharedInt hidespotsLearned = behaviorTree.GetVariable("hidespotsLearned") as SharedInt;

            // Increment the value by 1
            hidespotsLearned.Value += 1;

            // Set the updated value back into the variable
            behaviorTree.SetVariableValue("hidespotsLearned", hidespotsLearned);

            return TaskStatus.Success;
        }
        else
        {
            Debug.Log("Target is out of range or not hiding.");
            return TaskStatus.Success;
        }
    }
}
