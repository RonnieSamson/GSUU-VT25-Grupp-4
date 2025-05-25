using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    NavMeshAgent nm;
    public Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nm = GetComponent<NavMeshAgent>();
        nm.SetDestination(target.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
