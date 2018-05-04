using UnityEngine;
using UnityEngine.AI;

public class RouterController : MonoBehaviour {
    public GameObject[] units;
    public int type;

    private BaseRouter router;

    public void SetRouter(BaseRouter router) { this.router = router; }
    public BaseRouter GetRouter() { return router; }

    public BaseRouter GetEmptyRouter()
    {
        return new NavMeshAgentRouter();
    }

    public void EnableUnit(GameObject unit)
    {
        unit.GetComponent<NavMeshAgent>().enabled = true;
    }
}
