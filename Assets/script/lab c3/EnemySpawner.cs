using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform targetPosition;

    [Header("Timing")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int maxEnemies = 10;

    private int currentEnemyCount = 0;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Chỉ spawn nếu chưa đạt max
            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoints.Length == 0) return;

        // Chọn random spawn point
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        // Spawn enemy
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        // Set target
        EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
        if (movement != null && targetPosition != null)
        {
            movement.SetTarget(targetPosition);
        }

        currentEnemyCount++;
        Debug.Log($"<color=cyan>Enemy spawned at {spawnPoint.name}. Total: {currentEnemyCount}</color>");

        // Subscribe để giảm count khi enemy bị destroy
        StartCoroutine(WaitForEnemyDestroy(enemy));
    }

    IEnumerator WaitForEnemyDestroy(GameObject enemy)
    {
        // Đợi enemy bị destroy
        while (enemy != null)
        {
            yield return null;
        }

        currentEnemyCount--;
        Debug.Log($"<color=yellow>Enemy destroyed. Remaining: {currentEnemyCount}</color>");
    }

    // Gizmos
    void OnDrawGizmos()
    {
        if (spawnPoints == null) return;

        Gizmos.color = Color.blue;
        foreach (Transform point in spawnPoints)
        {
            if (point != null)
            {
                Gizmos.DrawWireSphere(point.position, 0.5f);
            }
        }

        // Line từ spawn đến target
        if (targetPosition != null)
        {
            Gizmos.color = Color.red;
            foreach (Transform point in spawnPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawLine(point.position, targetPosition.position);
                }
            }
        }
    }
}