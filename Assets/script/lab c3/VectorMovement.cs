using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool normalizeMovement = true;
    
    [Header("Gizmos Settings")]
    [SerializeField] private Color gizmoColor = Color.green;
    [SerializeField] private float gizmoLength = 2f;
    
    private Vector3 moveDirection;
    
    void Update()
    {
        HandleMovement();
    }
    
    void HandleMovement()
    {
        // Lấy input từ WASD
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D hoặc Left/Right
        float vertical = Input.GetAxisRaw("Vertical");     // W/S hoặc Up/Down
        
        // Tạo vector di chuyển
        moveDirection = new Vector3(horizontal, 0f, vertical);
        
        // NORMALIZE để tránh di chuyển chéo nhanh hơn
        if (normalizeMovement && moveDirection.magnitude > 0)
        {
            moveDirection.Normalize();
            /*
             * GIẢI THÍCH NORMALIZE:
             * - Khi nhấn W+D (chéo), vector = (1, 0, 1) có magnitude = √2 ≈ 1.414
             * - Điều này làm nhân vật chạy chéo nhanh hơn 41% so với chạy thẳng
             * - Normalize() giữ hướng nhưng đưa magnitude về 1
             * - Vector (1, 0, 1).normalized = (0.707, 0, 0.707) với magnitude = 1
             * - Kết quả: tốc độ đồng đều mọi hướng
             */
        }
        
        // Di chuyển nhân vật
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
    
    // Vẽ Gizmos trong Scene View
    void OnDrawGizmos()
    {
        if (moveDirection.magnitude > 0.01f)
        {
            Gizmos.color = gizmoColor;
            
            // Vẽ đường thẳng hiển thị hướng di chuyển
            Vector3 start = transform.position;
            Vector3 end = start + moveDirection * gizmoLength;
            Gizmos.DrawLine(start, end);
            
            // Vẽ mũi tên
            DrawArrowHead(start, end, 0.3f);
        }
    }
    
    void DrawArrowHead(Vector3 start, Vector3 end, float arrowSize)
    {
        Vector3 direction = (end - start).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, direction).normalized;
        Vector3 arrowPoint1 = end - direction * arrowSize + right * arrowSize * 0.5f;
        Vector3 arrowPoint2 = end - direction * arrowSize - right * arrowSize * 0.5f;
        
        Gizmos.DrawLine(end, arrowPoint1);
        Gizmos.DrawLine(end, arrowPoint2);
    }
    
    // UI hiển thị thông tin
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 400, 150));
        GUILayout.Box("Vector Movement Info");
        GUILayout.Label($"Direction: {moveDirection}");
        GUILayout.Label($"Magnitude: {moveDirection.magnitude:F3}");
        GUILayout.Label($"Normalized: {normalizeMovement}");
        GUILayout.Label($"Speed: {moveSpeed}");
        GUILayout.Label("\nWASD - Di chuyển");
        GUILayout.EndArea();
    }
}