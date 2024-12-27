using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class LookAround : Action
{
    public float rotationSpeed = 30f; // Turn speed
    private float rotationGoal;
    private float currentRotation;

    public override void OnStart()
    {
        rotationGoal = Random.Range(-75f, 75f); // Random rotation
        currentRotation = 0f; // Track how much we've rotated
    }

    public override TaskStatus OnUpdate()
    {
        float step = rotationSpeed * Time.deltaTime;

        // Determine rotation direction
        float rotationStep = Mathf.Sign(rotationGoal) * step;

        // Apply the rotation
        transform.Rotate(0, rotationStep, 0);
        currentRotation += rotationStep;

        // Check if we've reached or surpassed the rotation goal
        if (Mathf.Abs(currentRotation) >= Mathf.Abs(rotationGoal))
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
