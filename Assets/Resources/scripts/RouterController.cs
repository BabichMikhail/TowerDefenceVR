using UnityEngine;
using UnityEngine.AI;

public class RouterController : MonoBehaviour {
    public GameObject[] units;
    public int type;

    public const int ROUTER_TYPE_MIXED = 0;
    public const int ROUTER_TYPE_NAVMESH = 1;

    private BaseRouter router;

    public void SetRouter(BaseRouter router) { this.router = router; }
    public BaseRouter GetRouter() { return router; }

    public BaseRouter GetEmptyRouter()
    {
        BaseRouter router = null;
        if (type == ROUTER_TYPE_MIXED)
            router = new MixedRouter();
        else if (type == ROUTER_TYPE_NAVMESH)
            router = new NavMeshAgentRouter();
        Debug.Assert(router != null);
        return router;
    }

    public void EnableUnit(GameObject unit)
    {
        if (type == ROUTER_TYPE_NAVMESH)
            unit.GetComponent<NavMeshAgent>().enabled = true;
    }
}
