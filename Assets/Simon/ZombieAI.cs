using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public enum AIState
    {
        Idle,
        Moving,
        Running,
        Attacking
    }

    [Header("Zombie Settings")]
    public float zombieHealth = 100f;
    public float zombieDamage = 10f;
    public float attackCooldown = 1.5f;

    [Header("AI State")]
    public AIState aistate = AIState.Idle;

    private float lastAttackTime = -1f;
    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;

    private const float attackRange = 0.8f;
    private const float disengageRange = 0.9f;
    private const float chaseDistance = 8f;
    private const float stopChasingDistance = 12f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (aistate)
        {
            case AIState.Idle:
                HandleIdleState(distanceToPlayer);
                break;

            case AIState.Moving:
                HandleMovingState(distanceToPlayer);
                break;

            case AIState.Running:
                HandleRunningState(distanceToPlayer);
                break;

            case AIState.Attacking:
                HandleAttackingState(distanceToPlayer);
                break;
        }
    }

    private void HandleIdleState(float distanceToPlayer)
    {
        if (distanceToPlayer < chaseDistance)
        {
            Debug.Log("Idle > Moving");
            aistate = AIState.Moving;
        }
    }

    private void HandleMovingState(float distanceToPlayer)
    {
        if (distanceToPlayer < attackRange)
        {
            Debug.Log("Moving > Attacking");
            aistate = AIState.Attacking;
            return;
        }

        if (distanceToPlayer > stopChasingDistance)
        {
            Debug.Log("Moving > Running");
            aistate = AIState.Running;
            return;
        }

        animator.SetFloat("Speed", 1f, 0.3f, Time.deltaTime);
        agent.speed = 2f;
        agent.acceleration = 1.5f;
        agent.stoppingDistance = 0.1f;
        agent.SetDestination(player.position);
    }

    private void HandleRunningState(float distanceToPlayer)
    {
        if (distanceToPlayer < attackRange)
        {
            Debug.Log("Running > Attacking");
            aistate = AIState.Attacking;
            return;
        }

        if (distanceToPlayer < stopChasingDistance)
        {
            Debug.Log("Running > Moving");
            aistate = AIState.Moving;
            return;
        }

        animator.SetFloat("Speed", 1f, 0.3f, Time.deltaTime);
        agent.speed = 4f;
        agent.acceleration = 2f;
        agent.stoppingDistance = 0.3f;
        agent.SetDestination(player.position);
    }

    private void HandleAttackingState(float distanceToPlayer)
    {
        if (distanceToPlayer > disengageRange)
        {
            Debug.Log("Attacking > Moving");
            aistate = AIState.Moving;
            return;
        }

        // Stop movement and play idle animation
        agent.SetDestination(transform.position);
        animator.SetFloat("Speed", 0f, 0.3f, Time.deltaTime);

        // Cooldown check
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log("Zombie Attacks!");
            animator.SetTrigger("IsAttacking");
            lastAttackTime = Time.time;

            //Play animation
            //HealthManager.Health - zombieDamage;
            //Maybe call on a different manager that takes cares of everything?
            //For example here I call EventManager.PlayerDamage(zombieDamage)
            //And inside of the EventManager it takes care of;  1. changing the health value 2.play audio 3. play visual effects.
        }
    }
}
