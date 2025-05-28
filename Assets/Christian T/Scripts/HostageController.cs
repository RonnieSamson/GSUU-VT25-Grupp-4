using UnityEngine;
using UnityEngine.AI;

public class HostageController : MonoBehaviour
{
    private Animator animator;       // Privat, hämtas automatiskt
    public Transform player;
    public float interactDistance = 3f;

    public Transform destinationPoint;
    private NavMeshAgent agent;

    private bool isEscorting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();  // Hämta Animator-komponenten automatiskt
    
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent saknas på hostagen");
        }
        else if (!agent.isOnNavMesh)
        {
            Debug.LogError("Hostagen är inte placerad på en NavMesh");
        }

        if (animator == null)
        {
            Debug.LogError("Animator-komponent saknas på hostagen!");
        }
    }

    void Update()
    {
        if (!isEscorting && Input.GetKeyDown(KeyCode.E))
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance <= interactDistance && agent.isOnNavMesh)
            {
                Debug.Log("Start escorting");
                isEscorting = true;
                agent.SetDestination(destinationPoint.position);
            }
        }

        if (isEscorting && agent.isOnNavMesh)
        {
            // Uppdatera animation beroende på rörelse
            if (agent.velocity.magnitude > 0.1f)
            {
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }

            // Stoppa när hostagen når destinationen
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                animator.SetBool("isRunning", false);
                isEscorting = false;
                Debug.Log("Hostage reached destination");
            }
        }
    }
}
