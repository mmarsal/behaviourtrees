using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Custom")]
public class StandStillTask : Action
{
    public float duration = 5f;
    private float elapsedTime = 0f;

    public override TaskStatus OnUpdate()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= duration)
        {
            var target = GetComponent<Target>();
            if (target != null)
            {
                target.RestoreHealth(); // Wiederherstellen der Gesundheit nach dem Stehen
            }
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}