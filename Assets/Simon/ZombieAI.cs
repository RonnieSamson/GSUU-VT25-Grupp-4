using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour, IDamageable
{
    public enum AIState { Idle, Moving, Running, Attacking }

    [Header("Zombie Settings")]
    public float zombieHealth = 100f;
    public float zombieDamage = 10f;
    public float attackCooldown = 1.5f;

    [Header("Ragdoll")]
    public Rigidbody[] ragdollBodies;
    public Collider[] ragdollColliders;
    public Collider mainCollider;

    [Header("AI State")]
    public AIState aistate = AIState.Idle;

    private float lastAttackTime = -1f;
    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;

    private const float attackRange = 2f;
    private const float disengageRange = 3f;
    private const float chaseDistance = 8f;
    private const float stopChasingDistance = 12f;

    private bool isDead = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        DisableRagdoll();
    }

    private void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (aistate) //Handling AI states depending on context. Default is idle, if in range then moving/running, else if within close range then attack
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
            aistate = AIState.Moving;
    }

    private void HandleMovingState(float distanceToPlayer)
    {
        if (distanceToPlayer < attackRange)
        {
            aistate = AIState.Attacking;
            return;
        }

        if (distanceToPlayer > stopChasingDistance)
        {
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
            aistate = AIState.Attacking;
            return;
        }

        if (distanceToPlayer < stopChasingDistance)
        {
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
            aistate = AIState.Moving;
            return;
        }

        agent.SetDestination(transform.position);
        animator.SetFloat("Speed", 0f, 0.3f, Time.deltaTime);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            animator.SetTrigger("IsAttacking");
            lastAttackTime = Time.time;

            //Play animation
            //HealthManager.Health - zombieDamage;
            //Maybe call on a different manager that takes cares of everything?
            //For example here I call EventManager.PlayerDamage(zombieDamage)
            //And inside of the EventManager it takes care of;  1. changing the health value 2.play audio 3. play visual effects.
        }
    }
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        zombieHealth -= amount;
        animator.SetTrigger("IsHit");
        Debug.Log("Zombie HP: " + zombieHealth);

        if (zombieHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        agent.enabled = false;
        animator.enabled = false;
        mainCollider.enabled = false;

        EnableRagdoll();

        // Destroy zombie after 30 seconds for performance
        Destroy(gameObject, 30f);
    }

    private void EnableRagdoll()
    {
        foreach (var rb in ragdollBodies)
        {
            rb.isKinematic = false;
        }
    }

    private void DisableRagdoll()
    {
        foreach (var rb in ragdollBodies)
        {
            rb.isKinematic = true;
        }
    }
}
