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
        
    }

    // Update is called once per frame
    void Update()
    {
        nm.SetDestination(target.position);
    }
}
