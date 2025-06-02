using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject zombiePrefab;
    public float spawnInterval = 3f;
    public int maxZombies = 40;

    [Header("Optional Spawn Points")]
    [Tooltip("If you assign one or more Transforms here, zombies will spawn at a random point from this array. If left empty, spawns at this GameObject's position.")]
    public Transform[] spawnPoints;

    private Coroutine _spawnRoutine;

    private void Start()
    {
        _spawnRoutine = StartCoroutine(SpawnZombiesUntilMax());
    }

    private IEnumerator SpawnZombiesUntilMax()
    {
        // Continuously attempt to spawn as long as we have fewer than maxZombies active.
        while (true)
        {
            // how many active GameObjects are tagged with "Zombie"
            int currentCount = GameObject.FindGameObjectsWithTag("Zombie").Length;

            if (currentCount < maxZombies)
            {
                SpawnOneZombie();
            }
            else
            {
                // when at the limit, stop spawning
                yield break;
            }

            // Wait the interval before attempting the next spawn.
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnOneZombie()
    {
        Vector3 spawnPosition;

        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            // Choose a random spawn point from the array.
            int index = Random.Range(0, spawnPoints.Length);
            spawnPosition = spawnPoints[index].position;
        }
        else
        {
            // if no spawn set then use this position
            spawnPosition = transform.position;
        }

        // instantiate the zombie prefab at the chosen position with no rotation.
        GameObject newZombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

        // tag correctly.
        if (!newZombie.CompareTag("Zombie"))
        {
            newZombie.tag = "Zombie";
        }
    }
}