using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;

public class ZombieAI : MonoBehaviour
{

    public enum AIState
    {
        idle,
        moving,
        running,
        attacking
    }

    public AIState aistate = AIState.idle;

    NavMeshAgent nm;
    Transform player;
    public float zombieHealth = 100;
    public float zombieDamage = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nm = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform; //Find gameobject with tag Player
    }

    // Update is called once per frame
    void Update()
    {
        switch (aistate)
        {
            case AIState.idle:
                //If player within X range OR in line of sight, start moving towards palyer.
                if (Vector2.Distance(gameObject.transform.position, player.position) < 8)
                {
                    aistate = AIState.moving;
                }
                break;

            case AIState.moving:
                if (Vector2.Distance(gameObject.transform.position, player.position) < 0.8f)
                {
                    aistate = AIState.attacking;
                    break;
                }

                if (Vector2.Distance(gameObject.transform.position, player.position) > 12)
                {
                    aistate = AIState.running;
                    break;
                }
                nm.speed = 5;
                nm.acceleration = 12;
                nm.stoppingDistance = 0.2f;

                nm.SetDestination(player.position);
                break;

            case AIState.running:
                if (Vector2.Distance(gameObject.transform.position, player.position) < 0.8f)
                {
                    aistate = AIState.attacking;
                    break;
                }

                if (Vector2.Distance(gameObject.transform.position, player.position) < 12)
                {
                    aistate = AIState.moving;
                    break;
                }
                nm.speed = 8;
                nm.acceleration = 6;
                nm.stoppingDistance = 0.2f;

                nm.SetDestination(player.position);
                break;

            case AIState.attacking:
                if (Vector2.Distance(gameObject.transform.position, player.position) > 0.8f)
                {
                    aistate = AIState.moving;
                    break;
                }

                //Play animation
                //HealthManager.Health - zombieDamage;
                //Maybe call on a different manager that takes cares of everything?
                //For example here I call EventManager.PlayerDamage(zombieDamage)
                //And inside of the EventManager it takes care of;  1. changing the health value 2.play audio 3. play visual effects.

                break;

            default:

                break;
        }
        
    }
}
