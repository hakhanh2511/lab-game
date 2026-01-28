using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRotation : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    
    [Header("Rotation Settings")]
    [SerializeField] private RotationMode mode = RotationMode.Smooth;
    [SerializeField] private float rotationSpeed = 5f;
    
    [Header("Gizmos")]
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private float gizmoDistance = 3f;
    
    public enum RotationMode
    {
        Instant,    // LookAt - xoay ngay lập tức
        Smooth      // RotateTowards/Slerp - xoay mượt
    }
    
    void Update()
    {
        if (target == null) return;
        
        // Chuyển đổi chế độ bằng Space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            mode = (mode == RotationMode.Instant) ? RotationMode.Smooth : RotationMode.Instant;
            Debug.Log($"Rotation Mode: {mode}");
        }
        
        RotateTowardsTarget();
    }
    
    void RotateTowardsTarget()
    {
        // Tính toán hướng nhìn
        Vector3 direction = target.position - transform.position;
        direction.y = 0; // Giữ turret không nghiêng lên/xuống
        
        if (direction.magnitude < 0.01f) return;
        
        // Quaternion mục tiêu
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        
        switch (mode)
        {
            case RotationMode.Instant:
                // LookAt - xoay ngay lập tức
                transform.rotation = targetRotation;
                break;
                
            case RotationMode.Smooth:
                // RotateTowards - xoay mượt với tốc độ cố định
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation, 
                    targetRotation, 
                    rotationSpeed * Time.deltaTime * 50f
                );
                
                // Hoặc có thể dùng Slerp
                // transform.rotation = Quaternion.Slerp(
                //     transform.rotation, 
                //     targetRotation, 
                //     rotationSpeed * Time.deltaTime
                // );
                break;
        }
    }
    
    void OnDrawGizmos()
    {
        if (!showGizmos || target == null) return;
        
        // Vẽ đường nối đến target
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, target.position);
        
        // Vẽ hướng đang nhìn
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * gizmoDistance);
        
        // Vẽ sphere tại target
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(target.position, 0.5f);
    }
    
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 350, 120));
        GUILayout.Box("Turret Rotation Info");
        GUILayout.Label($"Mode: {mode}");
        GUILayout.Label($"Rotation Speed: {rotationSpeed}");
        
        if (target != null)
        {
            float angle = Vector3.Angle(transform.forward, target.position - transform.position);
            GUILayout.Label($"Angle to Target: {angle:F1}°");
        }
        
        GUILayout.Label("\nSPACE - Chuyển chế độ");
        GUILayout.EndArea();
    }
}