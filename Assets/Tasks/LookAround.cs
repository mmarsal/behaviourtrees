using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class LookAround : Action
{
    public float rotationSpeed = 35f;
    private float rotationGoal;
    private float currentRotation;

    public override void OnStart()
    {
        rotationGoal = Random.Range(-75f, 75f);
        currentRotation = 0f;
    }

    public override TaskStatus OnUpdate()
    {
        float step = rotationSpeed * Time.deltaTime;
        float rotationStep = Mathf.Sign(rotationGoal) * step;

        transform.Rotate(0, rotationStep, 0);
        currentRotation += rotationStep;

        if (Mathf.Abs(currentRotation) >= Mathf.Abs(rotationGoal))
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
