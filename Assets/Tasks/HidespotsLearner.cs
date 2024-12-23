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
    private Rigidbody alienRb;

    private float lastExecutionTime = -Mathf.Infinity; // Tracks the last execution time
    public float cooldown = 5f; // Cooldown duration in seconds
    public bool patrolling; // If Custom Node is being used for patroling or for looking around

    public override void OnStart()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerMovement>();
        behaviorTree = GetComponent<BehaviorTree>();
        alienRb = behaviorTree.GetComponent<Rigidbody>();
    }

    public override TaskStatus OnUpdate()
    {
        // Check if cooldown period has elapsed
        if (Time.time - lastExecutionTime < cooldown && patrolling)
        {
            return TaskStatus.Running;
        }

        if (alienRb != null)
        {
            Vector3 velocity = alienRb.velocity;

            // Check if x and z components of velocity are zero
            if (Mathf.Approximately(velocity.x, 0f) && Mathf.Approximately(velocity.z, 0f))
            {
                // Update the last execution time
                lastExecutionTime = Time.time;

                // Calculate the distance between the alien and the player
                float distance = Vector3.Distance(transform.position, player.transform.position);

                // Check if the distance is within the range and player is hiding
                if (distance <= range && playerScript.hiding)
                {
                    Debug.Log("Target is hiding within range!");

                    // Increment the value of "hidespotsLearned"
                    SharedInt hidespotsLearned = behaviorTree.GetVariable("hidespotsLearned") as SharedInt;
                    hidespotsLearned.Value += 1;
                    behaviorTree.SetVariableValue("hidespotsLearned", hidespotsLearned);
                    return patrolling ? hidespotsLearned.Value >= 2 ? TaskStatus.Success : TaskStatus.Running : TaskStatus.Success;
                }
                else
                {
                    Debug.Log("Target is out of range or not hiding.");
                    return patrolling ? TaskStatus.Running : TaskStatus.Success;
                }
            }
            else
            {
                return TaskStatus.Running;
            }
        }

        Debug.LogError("Alien Rigidbody is null.");
        return TaskStatus.Failure;
    }

}
