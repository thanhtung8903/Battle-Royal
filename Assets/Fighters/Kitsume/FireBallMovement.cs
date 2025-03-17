using System;
using UnityEngine;

public class FireBallMovement : MonoBehaviour
{
    public Vector3 startPosition;  // Vị trí ban đầu
    public Vector3 endPosition;    // Vị trí kết thúc
    public float speed = 1.0f;     // Tốc độ di chuyển
    [SerializeField] private UserConfiguration userConfiguration; // Cấu hình người dùng

    private float progress = 0.0f; // Tiến trình di chuyển

    internal void setUserConfiguration(UserConfiguration userConfiguration)
    {
        this.userConfiguration = userConfiguration; // Gán cấu hình người dùng
    }

    void Start()
    {
        startPosition = transform.position; // Lấy vị trí ban đầu của đối tượng

        float xEndValue = userConfiguration.getFacingRight() ? 10 : -10; // Xác định vị trí kết thúc theo hướng của người chơi

        endPosition = new Vector3(xEndValue, transform.position.y, transform.position.z); // Thiết lập vị trí kết thúc
    }

    void Update()
    {
        // Tăng tiến trình di chuyển dựa trên thời gian và tốc độ
        progress += Time.deltaTime * speed;

        // Nội suy vị trí của đối tượng từ startPosition đến endPosition
        transform.position = Vector3.Lerp(startPosition, endPosition, progress);

        // Dừng di chuyển khi đối tượng đến vị trí kết thúc
        if (progress >= 1.0f)
        {
            progress = 1.0f; // Đặt tiến trình về 1

            Destroy(gameObject); // Hủy đối tượng
        }
    }
}