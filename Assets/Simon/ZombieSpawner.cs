using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [Tooltip("Prefab of the zombie to spawn. Must be tagged \"Zombie\".")]
    public GameObject zombiePrefab;
    [Tooltip("Seconds between each spawn attempt.")]
    public float spawnInterval = 1f;
    [Tooltip("Maximum number of zombies to have on the map.")]
    public int maxZombies = 40;

    [Header("Optional Spawn Points")]
    [Tooltip("If you assign one or more Transforms here, zombies will spawn at a random point from this array. If left empty, spawns at this GameObject's position.")]
    public Transform[] spawnPoints;

    private Coroutine _spawnRoutine;

    private void Start()
    {
        // Begin the spawning coroutine when the scene starts.
        _spawnRoutine = StartCoroutine(SpawnZombiesUntilMax());
    }

    private IEnumerator SpawnZombiesUntilMax()
    {
        // Continuously attempt to spawn as long as we have fewer than maxZombies active.
        while (true)
        {
            // Count how many active GameObjects are tagged "Zombie"
            int currentCount = GameObject.FindGameObjectsWithTag("Zombie").Length;

            if (currentCount < maxZombies)
            {
                SpawnOneZombie();
            }
            else
            {
                // We have reached (or exceeded) the limit; stop spawning altogether.
                yield break;
            }

            // Wait one second before attempting the next spawn.
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
            // No spawn points set → use this GameObject's position.
            spawnPosition = transform.position;
        }

        // Instantiate the zombie prefab at the chosen position, with no rotation.
        GameObject newZombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

        // (Optional) Ensure the spawned object is tagged correctly.
        if (!newZombie.CompareTag("Zombie"))
        {
            newZombie.tag = "Zombie";
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Sanity‐check so that maxZombies is always at least 1, and spawnInterval is positive.
        if (maxZombies < 1) maxZombies = 1;
        if (spawnInterval < 0.1f) spawnInterval = 0.1f;
    }
#endif
}