using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class LookAround : Action
{
    public float rotationSpeed = 30f; // Drehgeschwindigkeit
    private float rotationGoal;

    public override void OnStart()
    {
        rotationGoal = Random.Range(-45f, 45f); // Zufällige Rotation
    }

    public override TaskStatus OnUpdate()
    {
        float step = rotationSpeed * Time.deltaTime;
        transform.Rotate(0, step, 0);

        if (Mathf.Abs(rotationGoal) <= step)
            return TaskStatus.Success;

        rotationGoal -= step;
        return TaskStatus.Running;
    }
}