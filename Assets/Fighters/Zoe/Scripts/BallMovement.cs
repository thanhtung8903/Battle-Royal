using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Vector2 direction; // Hướng di chuyển của quả cầu năng lượng
    private float expansionSpeed; // Tốc độ di chuyển của quả cầu năng lượng
    private string userTag; // Nhãn của người chơi đã bắn ra quả cầu năng lượng

    public float specialPowerDamage; // Sát thương của đòn tấn công đặc biệt
    public float specialPowerDamageToShield; // Sát thương lên lá chắn

    // Khởi tạo hướng di chuyển và tốc độ của quả cầu
    public void Initialize(Vector2 dir, float speed)
    {
        direction = dir.normalized;  //Hướng di chuyển
        expansionSpeed = speed; // Gán tốc độ
    }

    // Xử lý va chạm với các đối tượng khác
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu đối tượng va chạm có cùng nhãn với người bắn, bỏ qua
        if (other.gameObject.CompareTag(userTag))
        {
            Debug.Log(userTag);
            Debug.Log(other.gameObject.tag);
            return;
        }

        // Kiểm tra xem đối tượng có thể nhận sát thương hay không
        Damageable damageable = other.gameObject.GetComponent<Damageable>();
        Shieldable shield = other.gameObject.GetComponent<Shieldable>();

        if (damageable != null)
        {
            // Nếu đối tượng không có lá chắn hoặc lá chắn không hoạt động, gây sát thương trực tiếp
            if (shield == null || !shield.IsShieldActive())
            {
                damageable.decreaseLife(specialPowerDamage);
            }
            // Nếu có lá chắn, giảm sức chịu đựng của lá chắn
            else
            {
                shield.decreaseShieldCapacity(specialPowerDamageToShield);
            }
        }
    }

    // Gán nhãn của người bắn quả cầu năng lượng
    public void setUserTag(string userTag)
    {
        this.userTag = userTag;
    }

    void Update()
    {
        // Cập nhật vị trí của quả cầu theo hướng di chuyển với tốc độ đã đặt
        transform.position += (Vector3)direction * expansionSpeed * Time.deltaTime;

        // Nếu quả cầu ra khỏi màn hình, hủy đối tượng
        if (IsOutOfScreen())
        {
            Destroy(gameObject);
        }
    }

    // Kiểm tra xem quả cầu có nằm ngoài màn hình hay không
    private bool IsOutOfScreen()
    {
        Vector3 screenPosition = Camera.main.WorldToViewportPoint(transform.position);

        // Kiểm tra nếu vị trí của quả cầu nằm ngoài giới hạn màn hình
        return screenPosition.x < 0 || screenPosition.x > 1 ||
               screenPosition.y < 0 || screenPosition.y > 1;
    }
}