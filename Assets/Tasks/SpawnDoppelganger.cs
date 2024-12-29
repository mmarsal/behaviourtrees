using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class SpawnDoppelganger : Action
{
    public GameObject alien2;
    public SharedTransform spawnPosition;
    
    public override TaskStatus OnUpdate()
    { 
        alien2.transform.position = spawnPosition.Value.position + Vector3.left * 2;
        alien2.SetActive(true);
        return TaskStatus.Success;
    }
}