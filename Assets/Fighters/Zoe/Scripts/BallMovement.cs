using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Vector2 direction; // Hướng mà quả bóng sẽ mở rộng
    private float expansionSpeed; // Tốc độ mở rộng
    private string userTag; // Tag của người chơi đã tạo ra quả bóng

    public float specialPowerDamage; // Sát thương đặc biệt khi trúng kẻ địch
    public float specialPowerDamageToShield; // Sát thương đặc biệt lên lá chắn

    public void Initialize(Vector2 dir, float speed)
    {
        direction = dir.normalized; // Đảm bảo hướng luôn được chuẩn hóa (độ dài = 1)
        expansionSpeed = speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu quả bóng chạm vào nhân vật cùng phe, bỏ qua va chạm
        if (other.gameObject.CompareTag(userTag))
        {
            Debug.Log(userTag);
            Debug.Log(other.gameObject.tag);
            return;
        }

        Damageable damageable = other.gameObject.GetComponent<Damageable>(); // Kiểm tra xem đối tượng có thể nhận sát thương không
        Shieldable shield = other.gameObject.GetComponent<Shieldable>(); // Kiểm tra xem đối tượng có lá chắn không

        if (damageable != null)
        {
            // Nếu không có lá chắn hoặc lá chắn không hoạt động, gây sát thương trực tiếp
            if (shield == null || !shield.IsShieldActive())
            {
                damageable.decreaseLife(specialPowerDamage);
            }
            else
            {
                // Nếu có lá chắn, giảm sức bền của lá chắn
                shield.decreaseShieldCapacity(specialPowerDamageToShield);
            }
        }
    }

    public void setUserTag(string userTag)
    {
        this.userTag = userTag;
    }

    void Update()
    {
        // Di chuyển quả bóng theo hướng đã tính toán
        transform.position += (Vector3)direction * expansionSpeed * Time.deltaTime;

        // Hủy đối tượng nếu nó ra khỏi màn hình
        if (IsOutOfScreen())
        {
            Destroy(gameObject);
        }
    }

    private bool IsOutOfScreen()
    {
        Vector3 screenPosition = Camera.main.WorldToViewportPoint(transform.position);

        // Hủy đối tượng nếu nó ra khỏi màn hình
        return screenPosition.x < 0 || screenPosition.x > 1 ||
               screenPosition.y < 0 || screenPosition.y > 1;
    }
}