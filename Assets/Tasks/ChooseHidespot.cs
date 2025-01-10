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
        Collider[] hidespots = Physics.OverlapSphere(transform.position, range, layerMask);

        if (hidespots.Length == 0)
        {
            return TaskStatus.Failure;
        }

        List<GameObject> planes = new List<GameObject>();

        foreach (Collider hideSpot in hidespots)
        {
            float heightDifference = Mathf.Abs(hideSpot.transform.position.y - transform.position.y);
            
            if (hideSpot.gameObject.CompareTag("Plane") && heightDifference <= 1.0f)
            {
                planes.Add(hideSpot.gameObject);
            }
        }

        if (planes.Count == 0)
        {
            return TaskStatus.Failure;
        }

        int randomIndex = Random.Range(0, planes.Count);
        GameObject randomPlane = planes[randomIndex];
        behaviorTree.SetVariableValue("hidespotToCheck", randomPlane);

        return TaskStatus.Success;
    }
}
