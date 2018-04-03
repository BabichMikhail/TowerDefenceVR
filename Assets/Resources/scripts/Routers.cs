using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

abstract public class BaseRouter {
    public List<Vector3> points;
    public Collider targetCollider;
    public int targetPointIndex = 1;
    public bool inPlace = false;
    protected float MIN_DISTANCE = 10f;
    abstract public void ApplyMovement(Transform transform, float deltaTime, float speed);
    abstract public void SetPosition(Transform transform, Vector3 position);

    public Vector3 GetInitialPoint()
    {
        return points[0];
    }

    protected Vector3 NormalizeState(Vector3 position, Vector3 vector)
    {
        if (targetPointIndex < points.Count && (points[targetPointIndex] - position).sqrMagnitude < MIN_DISTANCE * MIN_DISTANCE) {
            ++targetPointIndex;
            inPlace = targetPointIndex == points.Count;
        }
        vector.Normalize();
        return vector;
    }

    public bool InPlace(Vector3 position, float minDistance)
    {
        inPlace = inPlace || (targetCollider.ClosestPoint(position) - position).magnitude < Mathf.Min(minDistance, MIN_DISTANCE);
        return inPlace;
    }

    abstract protected BaseRouter CreateInstance();
    public BaseRouter CopyInstance()
    {
        var newRouter = CreateInstance();
        newRouter.points = points;
        newRouter.targetPointIndex = targetPointIndex;
        newRouter.targetCollider = targetCollider;
        return newRouter;
    }

    protected Vector3 GetLastTargetPoint()
    {
        var point = points[points.Count - 1];
        if (targetPointIndex < points.Count)
            point = points[targetPointIndex];
        return point;
    }
}

class SampleRouter : BaseRouter
{
    public override void SetPosition(Transform transform, Vector3 position)
    {
        transform.position = position;
    }

    private Vector3 GetMovement(Transform transform)
    {
        if (targetPointIndex >= points.Count)
            return new Vector3(0, 0, 0);
        var position = transform.position;
        if (targetPointIndex == points.Count - 1)
            return NormalizeState(position, GetLastTargetPoint() - position);
        float distance1 = 100 * (points[targetPointIndex] - position).sqrMagnitude;
        float distance2 = (points[targetPointIndex + 1] - position).sqrMagnitude;
        float weight1 = distance1 / (distance1 + distance2);
        float weight2 = 1 - weight1;
        return NormalizeState(position, (points[targetPointIndex] - position) * weight1 + (points[targetPointIndex + 1] - position) * weight2);
    }

    public override void ApplyMovement(Transform transform, float deltaTime, float speed)
    {
        transform.Translate(Time.deltaTime * GetMovement(transform) * speed);
    }

    protected override BaseRouter CreateInstance()
    {
        return new SampleRouter();
    }
}

// TODO copypaste
class MixedRouter : BaseRouter
{
    bool agentEnabled = false;
    bool initialized = false;

    public override void SetPosition(Transform transform, Vector3 position)
    {
        transform.position = position;
    }

    private Vector3 GetMovement(Transform transform)
    {
        Debug.Log(targetPointIndex + " " + points.Count);
        if (targetPointIndex >= points.Count)
            return new Vector3(0, 0, 0);
        var position = transform.position;

        if (targetPointIndex >= points.Count - 2 && !agentEnabled) {
            if (targetPointIndex == points.Count - 1) {
                Debug.Log("Enable agent");
                transform.gameObject.GetComponent<NavMeshAgent>().enabled = true;
                agentEnabled = true;
            }
            return NormalizeState(position, points[targetPointIndex] - position);
        }

        float distance1 = 100 * (points[targetPointIndex] - position).sqrMagnitude;
        float distance2 = (points[targetPointIndex + 1] - position).sqrMagnitude;
        float weight1 = distance1 / (distance1 + distance2);
        float weight2 = 1 - weight1;
        return NormalizeState(position, (points[targetPointIndex] - position) * weight1 + (points[targetPointIndex + 1] - position) * weight2);
    }

    public override void ApplyMovement(Transform transform, float deltaTime, float speed)
    {
        if (agentEnabled) {
            var currentTargetPointIndex = targetPointIndex;
            NormalizeState(transform.position, (transform.position - GetLastTargetPoint()));
            transform.gameObject.GetComponent<NavMeshAgent>().enabled = true;
            if (!initialized || targetPointIndex < points.Count && targetPointIndex != currentTargetPointIndex) {
                var agent = transform.gameObject.GetComponent<NavMeshAgent>();
                agent.speed = speed;
                agent.SetDestination(points[targetPointIndex]);
                initialized = true;
            }
        } else
            transform.Translate(Time.deltaTime * GetMovement(transform) * speed);
    }

    protected override BaseRouter CreateInstance()
    {
        return new MixedRouter();
    }
}

class NavMeshAgentRouter : BaseRouter
{
    private bool initialized = false;

    public override void SetPosition(Transform transform, Vector3 position)
    {
        transform.gameObject.GetComponent<NavMeshAgent>().Warp(position);
    }

    public override void ApplyMovement(Transform transform, float deltaTime, float speed)
    {
        var currentTargetPointIndex = targetPointIndex;
        NormalizeState(transform.position, (transform.position - GetLastTargetPoint()));
        transform.gameObject.GetComponent<NavMeshAgent>().enabled = true;
        if (!initialized || targetPointIndex < points.Count && targetPointIndex != currentTargetPointIndex) {
            var agent = transform.gameObject.GetComponent<NavMeshAgent>();
            agent.speed = speed;
            agent.SetDestination(points[targetPointIndex]);
            initialized = true;
        }
    }

    protected override BaseRouter CreateInstance()
    {
        return new NavMeshAgentRouter();
    }
}