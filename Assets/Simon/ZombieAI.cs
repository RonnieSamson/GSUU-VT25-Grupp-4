using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour, IDamageable
{
    public enum AIState { Idle, Moving, Running, Attacking }

    [Header("Zombie Sounds")]
    public AudioClip[] ambientSounds; // Här kan du dra in dina 6 (eller fler) ljudklipp
    public float minAmbientSoundInterval = 5f;
    public float maxAmbientSoundInterval = 15f;

    private AudioSource audioSource;
    private float nextAmbientSoundTime;

    [Header("Zombie Settings")]
    public float zombieHealth = 100f;
    public float zombieDamage = 0.5f;
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
    private const float chaseDistance = 80f;
    private const float stopChasingDistance = 120f;

    private bool isDead = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        hostages = GameObject.FindGameObjectsWithTag("Hostage");

        // Hämta eller lägg till AudioSource-komponenten
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("ZombieAI saknar en AudioSource-komponent. Lägger till en automatiskt.", this);
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // Ställ in AudioSource-egenskaper om du vill, t.ex. 3D-ljud
        audioSource.spatialBlend = 1.0f; // Fullt 3D-ljud

        DisableRagdoll();

        // Sätt en initial tid för första ambient-ljudet (så det inte startar direkt på 0s)
        nextAmbientSoundTime = Time.time + Random.Range(minAmbientSoundInterval * 0.25f, maxAmbientSoundInterval * 0.5f);
    }

    private void Update()
    {
        if (isDead) return;

        HandleAmbientSounds(); // Hantera ambient-ljud

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
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            UpdateTarget();
            aistate = AIState.Moving;
            return;
        }

        //Slutar attackera om hostage sitter ner
        if (target.CompareTag("Hostage"))
        {
            HostageController controller = target.GetComponent<HostageController>();
            if (controller != null && controller.isSitting)
            {
                Debug.Log("Zombie avbryter attack – hostagen sitter.");
                aistate = AIState.Idle;
                return;
            }
        }

        if (distanceToTarget > disengageRange)
        {
            aistate = AIState.Moving;
            return;
        }

        agent.SetDestination(transform.position);
        animator.SetFloat("Speed", 0f, 0.3f, Time.deltaTime);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            //if (target.CompareTag("Player"))
            //{
            //    PlayerHealth health = target.GetComponent<PlayerHealth>();
            //    if (health != null)
            //    {
            //        health.DecreaseHealth(zombieDamage);
            //        Debug.Log("Player damaged!");
            //    }
            //}
            animator.SetTrigger("IsAttacking");
            lastAttackTime = Time.time;

            IDamageable damageable = target.GetComponent<IDamageable>();
            if (damageable != null && distanceToTarget <= attackRange)
            {
                damageable.TakeDamage(zombieDamage);
                Debug.Log("Zombie attackerade " + target.name + " och gjorde " + zombieDamage + " damage.");
            }
            else
            {
                Debug.Log("Target saknar IDamageable eller är utanför räckvidd.");
            }
        }
    }

    private void UpdateTarget()
    {

        Transform closestHostage = null;
        float closestHostageDistance = Mathf.Infinity;

        foreach (GameObject hostage in hostages)
        {
            if (hostage == null) continue;

            HostageController controller = hostage.GetComponent<HostageController>();
            if (controller != null && controller.isSitting) continue; // Ignorera sittande hostages

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
    
    private void HandleAmbientSounds()
    {
        // Se till att vi har allt som behövs och att inget ljud redan spelas på denna AudioSource
        if (audioSource == null || ambientSounds == null || ambientSounds.Length == 0 || audioSource.isPlaying)
        {
            return;
        }

        if (Time.time >= nextAmbientSoundTime)
        {
            // Välj ett slumpmässigt ljud från listan
            int randomIndex = Random.Range(0, ambientSounds.Length);
            AudioClip clipToPlay = ambientSounds[randomIndex];

            if (clipToPlay != null)
            {
                audioSource.PlayOneShot(clipToPlay); // PlayOneShot tillåter flera ljud att överlappa om du anropar det snabbt
                                                    // men vår 'audioSource.isPlaying' check ovan förhindrar detta för ambienta ljud.
            }
            
            // Sätt tiden för nästa ambient-ljud
            nextAmbientSoundTime = Time.time + Random.Range(minAmbientSoundInterval, maxAmbientSoundInterval);
        }
    }
}
