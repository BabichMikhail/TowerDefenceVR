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
        Debug.Log((points[targetPointIndex] - position).sqrMagnitude);
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
        float distance1 = 100*(points[targetPointIndex] - position).sqrMagnitude;
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

public class Main : MonoBehaviour {
    private List<BaseRouter> routers = new List<BaseRouter>();
    public GameObject[] units;

    private void Start()
    {
        var mainTowerObject = GameObject.FindGameObjectWithTag("MainTower");
        var finalPoint = BaseRouter.GetObjectPosition(mainTowerObject.transform);

        foreach (var routerObject in GameObject.FindGameObjectsWithTag("Router"))
        {
            var router = new SampleRouter();
            router.points = new List<Vector2>();
            for (int i = 0; i < routerObject.transform.childCount; ++i)
                router.points.Add(BaseRouter.GetObjectPosition(routerObject.transform.GetChild(i).transform));
            router.points.Add(finalPoint);
            routers.Add(router);
        }
    }

    public void SendUnit()
    {
        var unit = Instantiate(units[Random.Range(0, units.Length)], gameObject.transform);
        var router = routers[Random.Range(0, routers.Count)];
        unit.GetComponent<Move>().router = router.CopyInstance();
        var initialPoint = router.GetInitialPoint();
        unit.transform.position = new Vector3(initialPoint.x, 0, initialPoint.y);
    }

    public void OnMouseUp()
    {
        SendUnit();
    }
}
