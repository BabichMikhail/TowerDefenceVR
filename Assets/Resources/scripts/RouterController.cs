using UnityEngine;
using UnityEngine.AI;

public class RouterController : MonoBehaviour {
    public GameObject[] units;
    public int type;

    public BaseRouter Router { get; set; }

    public BaseRouter GetEmptyRouter()
    {
        return new NavMeshAgentRouter();
    }

    public void EnableUnit(GameObject unit)
    {
        unit.GetComponent<NavMeshAgent>().enabled = true;
    }
}
