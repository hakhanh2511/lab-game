using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignedAngleRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private RotationTarget rotationTarget = RotationTarget.Mouse;
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSpeed = 360f;
    
    [Header("Display")]
    [SerializeField] private bool showAngleUI = true;
    
    private Camera mainCamera;
    private float currentAngle = 0f;
    private Vector3 targetDirection;
    
    public enum RotationTarget
    {
        Mouse,
        Transform
    }
    
    void Start()
    {
        mainCamera = Camera.main;
    }
    
    void Update()
    {
        // Toggle rotation target với Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            rotationTarget = (rotationTarget == RotationTarget.Mouse) 
                ? RotationTarget.Transform 
                : RotationTarget.Mouse;
            Debug.Log($"Rotation Target: {rotationTarget}");
        }
        
        CalculateRotation();
    }
    
    void CalculateRotation()
    {
        Vector3 worldPosition = Vector3.zero;
        
        switch (rotationTarget)
        {
            case RotationTarget.Mouse:
                // Lấy vị trí chuột trong world
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                Plane groundPlane = new Plane(Vector3.up, transform.position);
                
                if (groundPlane.Raycast(ray, out float distance))
                {
                    worldPosition = ray.GetPoint(distance);
                }
                break;
                
            case RotationTarget.Transform:
                if (target != null)
                {
                    worldPosition = target.position;
                }
                else
                {
                    return;
                }
                break;
        }
        
        // Tính hướng từ nhân vật đến target
        targetDirection = worldPosition - transform.position;
        targetDirection.y = 0; // Giữ rotation trên mặt phẳng XZ
        
        if (targetDirection.magnitude < 0.01f) return;
        
        // Tính SIGNED ANGLE
        // SignedAngle trả về góc từ -180 đến +180
        // Dương: xoay ngược chiều kim đồng hồ
        // Âm: xoay theo chiều kim đồng hồ
        float targetAngle = Vector3.SignedAngle(
            Vector3.forward,        // Hướng tham chiếu (forward = 0°)
            targetDirection,        // Hướng mục tiêu
            Vector3.up              // Trục xoay
        );
        
        // Lưu góc hiện tại để hiển thị
        currentAngle = targetAngle;
        
        // Xoay nhân vật
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
    
    void OnDrawGizmos()
    {
        // Vẽ hướng forward
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 2f);
        
        // Vẽ hướng đến target
        if (targetDirection.magnitude > 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, targetDirection.normalized * 2f);
        }
        
        // Vẽ reference direction (forward)
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector3.forward * 1.5f);
    }
    
    void OnGUI()
    {
        if (!showAngleUI) return;
        
        // Vẽ UI góc xoay
        GUILayout.BeginArea(new Rect(10, 10, 350, 150));
        GUILayout.Box("Signed Angle Rotation");
        GUILayout.Label($"Target: {rotationTarget}");
        GUILayout.Label($"Current Angle: {currentAngle:F1}°");
        GUILayout.Label($"Current Rotation: {transform.eulerAngles.y:F1}°");
        GUILayout.Label("\nTAB - Chuyển đổi Mouse/Transform");
        
        // Vẽ thanh góc trực quan
        DrawAngleBar(currentAngle);
        
        GUILayout.EndArea();
    }
    
    void DrawAngleBar(float angle)
    {
        GUILayout.Label("\nAngle Visualization:");
        
        // Vẽ thanh từ -180 đến +180
        Rect barRect = GUILayoutUtility.GetRect(300, 20);
        GUI.Box(barRect, "");
        
        // Vẽ center line
        Rect centerLine = new Rect(barRect.x + barRect.width / 2 - 1, barRect.y, 2, barRect.height);
        GUI.Box(centerLine, "");
        
        // Vẽ vị trí góc hiện tại
        float normalizedAngle = (angle + 180f) / 360f; // Chuyển từ -180~180 sang 0~1
        float markerX = barRect.x + normalizedAngle * barRect.width - 5;
        Rect marker = new Rect(markerX, barRect.y - 2, 10, barRect.height + 4);
        
        Color originalColor = GUI.backgroundColor;
        GUI.backgroundColor = Color.green;
        GUI.Box(marker, "");
        GUI.backgroundColor = originalColor;
        
        GUILayout.Label($"-180°          0°          +180°");
    }
}
