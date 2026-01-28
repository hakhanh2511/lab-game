using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class TurretDefenseController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretBarrel;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Detection Settings")]
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private float shootRange = 10f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float aimThreshold = 5f; // Độ chính xác ngắm

    [Header("Shooting Settings")]
    [SerializeField] private float fireRate = 1f; // Bắn mỗi 1 giây
    [SerializeField] private float bulletSpeed = 20f;

    [Header("Unity Events")]
    public UnityEvent<Transform> OnTargetAcquired;
    public UnityEvent OnTargetLost;
    public UnityEvent OnShoot;

    private Transform currentTarget;
    private float nextFireTime;
    private List<Transform> enemiesInRange = new List<Transform>();

    void Update()
    {
        DetectEnemies();
        FindClosestEnemy();
        
        if (currentTarget != null)
        {
            RotateTowardsTarget();
            
            // Chỉ bắn khi trong tầm bắn và ngắm đủ chính xác
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
            if (distanceToTarget <= shootRange && IsAimingAtTarget())
            {
                TryShoot();
            }
        }
    }

    void DetectEnemies()
    {
        enemiesInRange.Clear();
        
        // Tìm tất cả enemy trong vòng tròn detection range
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange, enemyLayer);
        
        foreach (Collider hit in hits)
        {
            enemiesInRange.Add(hit.transform);
        }
    }

    void FindClosestEnemy()
    {
        Transform previousTarget = currentTarget;
        currentTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform enemy in enemiesInRange)
        {
            if (enemy == null) continue;

            float distance = Vector3.Distance(transform.position, enemy.position);
            
            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentTarget = enemy;
            }
        }

        // Gọi event khi có target mới
        if (currentTarget != previousTarget)
        {
            if (currentTarget != null)
            {
                OnTargetAcquired?.Invoke(currentTarget);
                Debug.Log($"<color=yellow>Target Acquired: {currentTarget.name}</color>");
            }
            else if (previousTarget != null)
            {
                OnTargetLost?.Invoke();
                Debug.Log("<color=gray>Target Lost</color>");
            }
        }
    }

    void RotateTowardsTarget()
    {
        if (turretBarrel == null || currentTarget == null) return;

        // Tính hướng đến target (chỉ xoay trên mặt phẳng Y)
        Vector3 direction = currentTarget.position - turretBarrel.position;
        direction.y = 0; // Bỏ qua độ cao

        if (direction != Vector3.zero)
        {
            // Tính rotation cần xoay đến
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            // Xoay mượt
            turretBarrel.rotation = Quaternion.RotateTowards(
                turretBarrel.rotation,
                targetRotation,
                rotationSpeed * 100f * Time.deltaTime
            );
        }
    }

    bool IsAimingAtTarget()
    {
        if (turretBarrel == null || currentTarget == null) return false;

        Vector3 directionToTarget = (currentTarget.position - turretBarrel.position).normalized;
        directionToTarget.y = 0;

        float angle = Vector3.Angle(turretBarrel.forward, directionToTarget);
        return angle < aimThreshold;
    }

    void TryShoot()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // Tạo bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
        // Bắn thẳng về phía trước
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * bulletSpeed;
        }

        // Hủy bullet sau 5 giây
        Destroy(bullet, 5f);

        // Gọi event
        OnShoot?.Invoke();
        Debug.Log("<color=cyan>SHOOT!</color>");
    }

    // Vẽ Gizmos để debug
    void OnDrawGizmosSelected()
    {
        // Vòng tròn detection range (màu vàng)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Vòng tròn shoot range (màu đỏ)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootRange);

        // Line đến target hiện tại (màu xanh)
        if (currentTarget != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, currentTarget.position);
        }
    }
}