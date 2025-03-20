using System;
using UnityEngine;

public class ZoeAttack : MonoBehaviour
{

    public Animator animator; // Tham chiếu đến Animator để phát hoạt ảnh tấn công
    public Transform weaponHitBox; // Vị trí nơi sẽ kiểm tra va chạm của vũ khí
    public float attackRange; // Phạm vi có thể phát hiện kẻ địch

    // Giá trị sát thương cho các loại tấn công khác nhau
    public float hitDamage; // Sát thương đòn đánh thường
    public float kickDamage; // Sát thương đòn đá


    public float hitDamageToShield; // Sát thương đòn đánh thường lên khiên
    public float kickDamageToShield; // Sát thương đòn đá lên khiên


    public float attackRate = 1f; // Tốc độ tấn công: số lần tấn công cho phép mỗi giây
    public float waitingTimeHit; // Thời gian chờ giữa các đòn đánh thường
    public float waitingTimeKick; // Thời gian chờ giữa các đòn đá
    private float nexAttackTime = 0f; // Bộ đếm thời gian chờ cho lần tấn công tiếp theo

    private ZoeSpecialAttack specialAttack;
    private UserConfiguration userConfiguration;

    // Âm thanh khi tấn công
    [SerializeField] private AudioClip soundAttack1;
    [SerializeField] private AudioClip soundAttack2;

    //Tag của nhân vật
    string ownTag;

    void Start()
    {
        specialAttack = GetComponent<ZoeSpecialAttack>();
        animator = GetComponent<Animator>();
        userConfiguration = GetComponent<UserConfiguration>();
        ownTag = gameObject.tag;
    }

    // Update được gọi mỗi khung hình
    void Update()
    {
        // Chỉ cho phép tấn công nếu đã đủ thời gian chờ từ đòn tấn công trước đó
        if (Time.time >= nexAttackTime)
        {
            // Nếu nhấn phím tấn công thường, thực hiện đòn đánh
            if (Input.GetKeyDown(userConfiguration.getHitKey()))
            {
                hit();
                SoundsController.Instance.RunSound(soundAttack1);
                nexAttackTime = Time.time + waitingTimeHit / attackRate;
            }
            // Nếu nhấn phím đá, thực hiện đòn đá
            else if (Input.GetKeyDown(userConfiguration.getKickKey()))
            {
                kick();
                SoundsController.Instance.RunSound(soundAttack2);
                nexAttackTime = Time.time + waitingTimeKick / attackRate;
            }
            // Nếu nhấn phím kích hoạt kỹ năng đặc biệt, sử dụng kỹ năng đặc biệt
            else if (Input.GetKeyDown(userConfiguration.getSpecialPowerKey()))
            {
                specialAttack.useSpecialAttack();
            }
        }
    }

    // Hàm thực hiện đòn đánh thường
    void hit()
    {
        animator.SetTrigger("attack1"); // Kích hoạt hoạt ảnh tấn công
        applyDamageToEnemies(hitDamage, hitDamageToShield); // Gây sát thương lên kẻ địch
    }

    // Hàm thực hiện đòn đá
    private void kick()
    {
        // Kích hoạt hoạt ảnh đòn đá
        animator.SetTrigger("attack2");
        applyDamageToEnemies(kickDamage, kickDamageToShield);
    }

    // Hàm áp dụng sát thương lên kẻ địch bị trúng đòn
    public void applyDamageToEnemies(float damage, float damageToShield)
    {
        // Phát hiện kẻ địch trong phạm vi vũ khí
        Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(weaponHitBox.position, attackRange);


        // Áp dụng sát thương lên từng kẻ địch bị trúng đòn
        foreach (Collider2D playerEnemy in hitOtherPlayers)
        {

            Damageable damageable = playerEnemy.GetComponent<Damageable>();
            Shieldable shield = playerEnemy.GetComponent<Shieldable>();

            if (damageable != null && gameObject.tag != playerEnemy.tag)
            {
                if (shield == null || !shield.IsShieldActive()) // Nếu không có khiên hoặc khiên không hoạt động
                {
                    damageable.decreaseLife(damage);
                    Debug.Log("We hit " + playerEnemy.name);
                    // Nạp năng lượng cho kỹ năng đặc biệt khi đánh trúng kẻ địch
                    specialAttack.increaseCharge(damage);
                }
                else
                {
                    shield.decreaseShieldCapacity(damageToShield); // Gây sát thương lên khiên
                }
            }

        }
    }


    // Hàm cần thiết để sử dụng các đối tượng con trong GameObject trong trình chỉnh sửa
    private void OnValidate()
    {
        if (weaponHitBox == null)
        {
            weaponHitBox = transform.Find("WeaponHitBox");
            if (weaponHitBox == null)
            {
                Debug.LogWarning("WeaponHitBox not found. Ensure there is a child GameObject named 'WeaponHitBox'.");
            }
        }
    }

    // Hiển thị Gizmo để trực quan hóa vùng tấn công trong Scene
    private void OnDrawGizmosSelected()
    {
        if (weaponHitBox == null)
        {
            return;
        }
        Gizmos.color = Color.red; // Màu của Gizmo
        Gizmos.DrawWireSphere(weaponHitBox.position, attackRange); // Vẽ vòng tròn hiển thị phạm vi tấn công
    }
}