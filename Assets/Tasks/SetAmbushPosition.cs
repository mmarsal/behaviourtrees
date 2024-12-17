using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class SetAmbushPosition : Action
{
    public SharedGameObjectList AmbushPoints;
    public SharedBool atAmbushPosition;

    public override TaskStatus OnUpdate()
    {
        if (AmbushPoints == null || AmbushPoints.Value.Count == 0)
        {
            return TaskStatus.Failure;
        }

        // Wähle zufällig eine Ambush-Position
        int index = Random.Range(0, AmbushPoints.Value.Count);
        GameObject selectedPoint = AmbushPoints.Value[index];
        Transform transformComponent = this.transform;

        // Bewege den Alien zur Ambush-Position
        transformComponent.position = selectedPoint.transform.position;
        transformComponent.rotation = selectedPoint.transform.rotation;

        // Setze die Variable
        atAmbushPosition.Value = true;

        return TaskStatus.Success;
    }
}