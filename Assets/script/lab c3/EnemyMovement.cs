using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Transform targetPosition;

    void Update()
    {
        if (targetPosition != null)
        {
            // Di chuyển về phía target
            Vector3 direction = (targetPosition.position - transform.position).normalized;
            direction.y = 0; // Không di chuyển theo trục Y

            transform.position += direction * moveSpeed * Time.deltaTime;

            // Xoay về hướng di chuyển
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            // Destroy nếu đến target
            if (Vector3.Distance(transform.position, targetPosition.position) < 1f)
            {
                Debug.Log("<color=red>Enemy reached target!</color>");
                Destroy(gameObject);
            }
        }
    }

    public void SetTarget(Transform target)
    {
        targetPosition = target;
    }

    // Destroy khi va chạm với bullet
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("<color=green>Enemy destroyed by bullet!</color>");
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}