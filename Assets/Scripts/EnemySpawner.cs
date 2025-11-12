using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private float spawnRate = 1f;           // Delay between spawns
    [SerializeField] private GameObject[] enemyPrefabs;      // Enemies to spawn
    [SerializeField] private bool canSpawn = true;           // Control spawning
    [SerializeField] private Transform[] spawnPoints;        // Optional spawn points

<<<<<<< HEAD
    [SerializeField] private GameObject[] enemyPrefabs;

    [SerializeField] private bool canSpawn = true;

=======
>>>>>>> c0d6e54 (changes done)
    private void Start()
    {
        // Start spawning when game begins
        StartCoroutine(SpawnRoutine());
    }

<<<<<<< HEAD

    private IEnumerator Spawner () {
     WaitForSeconds wait = new WaitForSeconds(spawnRate);

   while (canSpawn)
    {
    yield return wait;
    int rand = Random.Range(0, enemyPrefabs.Length);
     GameObject enemyToSpawn = enemyPrefabs[rand];

     Instantiate(enemyToSpawn, transform.position, Quaternion.identity);

    }
 
 


}

}
=======
    // 🔁 Handles repeated enemy spawning
    private IEnumerator SpawnRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnRate);

        while (canSpawn)
        {
            yield return wait;
            SpawnEnemy();
        }
    }

    // 🧩 Actual spawning logic (called every cycle)
    private void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0) return;

        int randIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyToSpawn = enemyPrefabs[randIndex];

        Vector3 spawnPos = transform.position;

        // If spawn points exist, pick a random one
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            spawnPos = spawnPoints[spawnIndex].position;
        }

        Instantiate(enemyToSpawn, spawnPos, Quaternion.identity);

        // 🔊 Optional: play a small spawn sound
        if (AudioManager.instance != null && AudioManager.instance.IsSFXEnabled())
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.enemyDeathSFX);
        }
    }

    // 🟥 Stop spawner externally (e.g., when player dies)
    public void StopSpawning()
    {
        canSpawn = false;
        StopAllCoroutines();
    }
}
>>>>>>> c0d6e54 (changes done)
