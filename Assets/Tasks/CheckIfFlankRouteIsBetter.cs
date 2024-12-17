using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class CheckIfFlankRouteIsBetter : Conditional
{
    public SharedGameObject player;
    public SharedGameObject alien;
    public SharedGameObjectList flankPoints;

    private GameObject GetClosestFlankPoint(Vector3 alienPosition)
    {
        GameObject closestPoint = null;
        float closestDistance = float.MaxValue;

        foreach (var point in flankPoints.Value)
        {
            float distance = Vector3.Distance(alienPosition, point.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = point;
            }
        }

        return closestPoint;
    }

    public override TaskStatus OnUpdate()
    {
        if (player.Value == null || alien.Value == null)
        {
            return TaskStatus.Failure;
        }
        
        GameObject closestPoint = GetClosestFlankPoint(alien.Value.transform.position);
        if (closestPoint != null)
        {
            alien.Value.transform.position = closestPoint.transform.position;
            return TaskStatus.Success;
        }
        
        
        return TaskStatus.Failure;
    }
}