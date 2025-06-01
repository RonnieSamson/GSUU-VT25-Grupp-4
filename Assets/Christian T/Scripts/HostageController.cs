using UnityEngine;
using UnityEngine.AI;

public class HostageController : MonoBehaviour
{
    private Animator animator;
    public Transform player;
    public float interactDistance = 3f;

    public Transform destinationArea; 
    private NavMeshAgent agent;

    private bool isEscorting = false;
    
    public RescueManager rescueManager;
    public bool isSitting => !isEscorting;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

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
                agent.SetDestination(GetRandomPointInArea());
            }
        }

        if (isEscorting && agent.isOnNavMesh)
        {
            animator.SetBool("isRunning", agent.velocity.magnitude > 0.1f);


            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                animator.SetBool("isRunning", false);
                isEscorting = false;
                rescueManager.HostageRescued();
                Debug.Log("Hostage reached destination");
            }
        }
    }

    private Vector3 GetRandomPointInArea()
    {
        Vector3 center = destinationArea.position;
        Vector3 size = destinationArea.localScale * 10f; 

        float offsetX = Random.Range(-size.x / 2f, size.x / 2f);
        float offsetZ = Random.Range(-size.z / 2f, size.z / 2f);

        return new Vector3(center.x + offsetX, center.y, center.z + offsetZ);
    }
}
