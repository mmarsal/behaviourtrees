using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("Custom")]
public class HidespotsLearner : Action
{
    private GameObject player;
    private PlayerMovement playerScript;
    public float range = 7.5f;
    private BehaviorTree behaviorTree;
    private Rigidbody alienRb;

    private float lastExecutionTime = -Mathf.Infinity;
    public float cooldown = 5f;
    public bool patrolling;

    public override void OnStart()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerMovement>();
        behaviorTree = GetComponent<BehaviorTree>();
        alienRb = behaviorTree.GetComponent<Rigidbody>();
    }

    public override TaskStatus OnUpdate()
    {
        if (Time.time - lastExecutionTime < cooldown && patrolling)
        {
            return TaskStatus.Running;
        }

        if (alienRb != null)
        {
            Vector3 velocity = alienRb.velocity;

            // schauen ob x und z 0 sind
            if (Mathf.Approximately(velocity.x, 0f) && Mathf.Approximately(velocity.z, 0f))
            {
                lastExecutionTime = Time.time;
                float distance = Vector3.Distance(transform.position, player.transform.position);
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
                    return patrolling ? TaskStatus.Running : TaskStatus.Success;
                }
            }
            else
            {
                return TaskStatus.Running;
            }
        }
        return TaskStatus.Failure;
    }

}
