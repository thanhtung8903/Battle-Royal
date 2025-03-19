using UnityEngine;

public class OVNIAttack : MonoBehaviour
{
    [SerializeField] private float damage = 15;
    [SerializeField] private float damageToShield = 10;

    // Xử lý sự kiện khi đối tượng va chạm với một collider khác
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Lấy thành phần có thể nhận sát thương từ đối tượng va chạm
        Damageable damageable = collider.gameObject.GetComponent<Damageable>();
        // Lấy thành phần lá chắn từ đối tượng va chạm
        Shieldable shieldable = collider.gameObject.GetComponent<Shieldable>();

        // Nếu đối tượng không thể nhận sát thương hoặc không có lá chắn, thoát khỏi phương thức
        if (damageable == null || shieldable == null)
        {
            return;
        }

        // Nếu không có lá chắn hoặc lá chắn không hoạt động, gây sát thương trực tiếp
        if (shieldable == null || !shieldable.IsShieldActive())
        {
            damageable.decreaseLife(damage);
            Debug.Log("We performAttack1 " + collider.gameObject.name);
            return;
        }

        // Nếu có lá chắn hoạt động, giảm khả năng chống chịu của lá chắn
        shieldable.decreaseShieldCapacity(damageToShield);
    }
}
