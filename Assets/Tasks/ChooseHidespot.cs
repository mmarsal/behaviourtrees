using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using System.Collections.Generic;

[TaskCategory("Custom")]
public class ChooseHidespot : Action
{
    public float range = 5f;
    private LayerMask layerMask;
    private BehaviorTree behaviorTree;

    public override void OnStart()
    {
        layerMask = LayerMask.GetMask("Hidespot");
        behaviorTree = GetComponent<BehaviorTree>();
    }

    public override TaskStatus OnUpdate()
    {
        // Find all hidespots within the range
        Collider[] hidespots = Physics.OverlapSphere(transform.position, range, layerMask);

        if (hidespots.Length == 0)
        {
            return TaskStatus.Failure;
        }

        // Create a list to store the hidespots' plane objects
        List<GameObject> planes = new List<GameObject>();

        // Loop through all the hidespots
        foreach (Collider hideSpot in hidespots)
        {
            // Check the vertical difference (y position)
            float heightDifference = Mathf.Abs(hideSpot.transform.position.y - transform.position.y);
            
            // Check if the object is tagged as "Plane"
            if (hideSpot.gameObject.CompareTag("Plane") && heightDifference <= 1.0f)
            {
                planes.Add(hideSpot.gameObject);
            }
        }

        if (planes.Count == 0)
        {
            return TaskStatus.Failure; // No plane objects found
        }

        // Pick a random hidespotPlane from the list
        int randomIndex = Random.Range(0, planes.Count);

        // Get the random plane
        GameObject randomPlane = planes[randomIndex];

        Debug.Log("Randomly selected plane: " + randomPlane.name);

        // Store the selected plane object in the behavior tree variable
        behaviorTree.SetVariableValue("hidespotToCheck", randomPlane);

        return TaskStatus.Success;
    }
}
