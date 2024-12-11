using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("Custom")]
public class MoveAwayTask : Action
{
    public SharedTransform playerTransform;
    public float moveDistance = 10f;
    public float duration = 3f;

    private Vector3 targetPosition;
    private float elapsedTime = 0f;

    public override void OnStart()
    {
        var target = GetComponent<Target>();
        if (target != null && target.health > 1f)
        {
            return;
        }

        var direction = (transform.position - playerTransform.Value.position).normalized;
        targetPosition = transform.position + direction * moveDistance;
    }

    public override TaskStatus OnUpdate()
    {
        elapsedTime += Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, targetPosition, elapsedTime / duration);

        if (elapsedTime >= duration)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}