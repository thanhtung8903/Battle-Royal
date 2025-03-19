using UnityEngine;

public class OVNIMovement : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition; // Vị trí bắt đầu
    [SerializeField] private Vector3 endPosition; // Vị trí kết thúc
    [SerializeField] private float speed; // Tốc độ di chuyển
    [SerializeField] private float rotationSpeed; // Tốc độ xoay
    [SerializeField] private float progress = 0f;  // Tiến trình di chuyển (từ 0 đến 1)
    [SerializeField] private AudioClip movementSound; // Âm thanh di chuyển

    private void Start()
    {
        SoundsController.Instance.RunSound(movementSound); // Phát âm thanh di chuyển
    }

    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime); // Xoay đối tượng
        progress += Time.deltaTime * speed; // Tăng tiến trình di chuyển dựa trên tốc độ và thời gian
        transform.position = Vector3.Lerp(startPosition, endPosition, progress);  // Di chuyển đối tượng từ vị trí bắt đầu đến vị trí kết thúc

        if (progress < 1f)
        {
            return;
        }
        // Dừng di chuyển khi đến vị trí cuối
        progress = 1f; // Đảm bảo giá trị không vượt quá phạm vi
        Destroy(gameObject);// Hủy đối tượng khi đến vị trí kết thúc
    }

    public void setStartPosition(Vector3 startPosition)
    {
        this.startPosition = startPosition;
    }

    public void setEndPosition(Vector3 endPosition)
    {
        this.endPosition = endPosition;
    }
}
