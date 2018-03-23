using System.Collections.Generic;
using UnityEngine;

abstract public class BaseRouter
{
    public List<Vector2> points;
    public int targetPointIndex = 1;
    public bool inPlace = false;
    const double MIN_DISTANCE = 0.5;
    abstract public Vector2 GetMovement(Transform transform);
    abstract public Vector2 GetInitialPoint();

    public static Vector2 GetObjectPosition(Transform transform)
    {
        return new Vector2(transform.position.x, transform.position.z);
    }

    protected Vector2 NormalizeState(Vector2 position, Vector2 vector)
    {
        if ((points[targetPointIndex] - position).sqrMagnitude < MIN_DISTANCE) {
            ++targetPointIndex;
            inPlace = targetPointIndex == points.Count;
        }
        vector.Normalize();
        return vector;
    }

    public bool InPlace()
    {
        return inPlace;
    }

    abstract protected BaseRouter CreateInstance();
    public BaseRouter CopyInstance()
    {
        var newRouter = CreateInstance();
        newRouter.points = points;
        newRouter.targetPointIndex = targetPointIndex;
        return newRouter;
    }
}

class SampleRouter : BaseRouter
{
    public override Vector2 GetMovement(Transform transform)
    {
        Debug.Assert(targetPointIndex < points.Count);
        var position = GetObjectPosition(transform);
        if (targetPointIndex == points.Count - 1)
            return NormalizeState(position, points[targetPointIndex] - position);
        float distance1 = 100 * (points[targetPointIndex] - position).sqrMagnitude;
        float distance2 = (points[targetPointIndex + 1] - position).sqrMagnitude;
        float weight1 = distance1 / (distance1 + distance2);
        float weight2 = 1 - weight1;
        return NormalizeState(position, (points[targetPointIndex] - position) * weight1 + (points[targetPointIndex + 1] - position) * weight2);
    }

    public override Vector2 GetInitialPoint()
    {
        return points[0];
    }

    protected override BaseRouter CreateInstance()
    {
        return new SampleRouter();
    }
}
