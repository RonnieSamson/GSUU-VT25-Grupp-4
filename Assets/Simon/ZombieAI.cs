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
    private GameObject[] hostages; // cache hostage GameObjects
    private Transform target;


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
        hostages = GameObject.FindGameObjectsWithTag("Hostage");

        DisableRagdoll();
    }

    private void Update()
    {
        if (isDead) return;

        UpdateTarget();
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        switch (aistate) //Handling AI states depending on context. Default is idle, if in range then moving/running, else if within close range then attack
        {
            case AIState.Idle:
                HandleIdleState(distanceToTarget);
                break;
            case AIState.Moving:
                HandleMovingState(distanceToTarget);
                break;
            case AIState.Running:
                HandleRunningState(distanceToTarget);
                break;
            case AIState.Attacking:
                HandleAttackingState(distanceToTarget);
                break;
        }
    }

    private void HandleIdleState(float distanceToTarget)
    {
        if (distanceToTarget < chaseDistance)
            aistate = AIState.Moving;
    }

    private void HandleMovingState(float distanceToTarget)
    {
        if (distanceToTarget < attackRange)
        {
            aistate = AIState.Attacking;
            return;
        }

        if (distanceToTarget > stopChasingDistance)
        {
            aistate = AIState.Running;
            return;
        }

        animator.SetFloat("Speed", 1f, 0.3f, Time.deltaTime);
        agent.speed = 1.5f;
        agent.acceleration = 1.5f;
        agent.stoppingDistance = 0.1f;
        agent.SetDestination(target.position);
    }

    private void HandleRunningState(float distanceToTarget)
    {
        if (distanceToTarget < attackRange)
        {
            aistate = AIState.Attacking;
            return;
        }

        if (distanceToTarget < stopChasingDistance)
        {
            aistate = AIState.Moving;
            return;
        }

        animator.SetFloat("Speed", 1f, 0.3f, Time.deltaTime); 
        agent.speed = 3f;
        agent.acceleration = 2f;
        agent.stoppingDistance = 0.3f;
        agent.SetDestination(target.position);
    }

    private void HandleAttackingState(float distanceToTarget)
    {
        if (distanceToTarget > disengageRange)
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

    private void UpdateTarget()
    {
        Transform closestHostage = null;
        float closestHostageDistance = Mathf.Infinity;

        foreach (GameObject hostage in hostages)
        {
            if (hostage == null) continue;

            float dist = Vector3.Distance(transform.position, hostage.transform.position);
            if (dist < closestHostageDistance)
            {
                closestHostageDistance = dist;
                closestHostage = hostage.transform;
            }
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (closestHostage != null && closestHostageDistance < distanceToPlayer)
        {
            target = closestHostage;
        }
        else
        {
            target = player;
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
