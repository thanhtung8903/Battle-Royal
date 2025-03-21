using System;
using UnityEngine;

public class KitsuneAttack : MonoBehaviour
{

    public Animator animator; // Tham chiếu đến Animator để phát các hoạt ảnh tấn công
    public Transform weaponHitBox; // Vị trí kiểm tra va chạm của vũ khí
    public float attackRange; // Phạm vi có thể phát hiện người chơi đối phương

    // Giá trị sát thương cho các đòn tấn công khác nhau
    public float hitDamage;
    public float kickDamage;
    public float specialPowerDamage;

    public float hitDamageToShield;
    public float kickDamageToShield;

    public float attackRate = 1f; // Tốc độ tấn công: số lần tấn công cho phép mỗi giây
    public float waitingTimeHit; // Thời gian chờ giữa các đòn đánh
    public float waitingTimeKick; // Thời gian chờ giữa các đòn đá
    private float nexAttackTime = 0f; // Bộ tích lũy thời gian chờ cho đòn tấn công tiếp theo

    //public KeyCode hitKey;
    //public KeyCode kickKey;
    //public KeyCode specialPowerKey;

    private KitsuneSpecialAttack specialAttack;
    private UserConfiguration userConfiguration;

    // Thuộc tính cho âm thanh
    [SerializeField] private AudioClip soundAttack1;

    string ownTag;

    void Start()
    {
        specialAttack = GetComponent<KitsuneSpecialAttack>(); //THAY ĐỔI thành Kitsune
        animator = GetComponent<Animator>();
        userConfiguration = GetComponent<UserConfiguration>();
        ownTag = gameObject.tag;
        //otherPlayer = LayerMask.GetMask("BaseFighter");
    }

    // Update được gọi mỗi khung hình
    void Update()
    {
        // Chỉ cho phép tấn công nếu đã đủ thời gian từ lần tấn công cuối
        if (Time.time >= nexAttackTime)
        {
            // Nếu phím tương ứng được nhấn, thực hiện đòn đánh
            if (Input.GetKeyDown(userConfiguration.getHitKey()))
            {
                hit();
                SoundsController.Instance.RunSound(soundAttack1);
                nexAttackTime = Time.time + waitingTimeHit / attackRate;
            }
            // Nếu phím tương ứng được nhấn, thực hiện đòn đá
            else if (Input.GetKeyDown(userConfiguration.getKickKey()))
            {
                kick();
                nexAttackTime = Time.time + waitingTimeKick / attackRate;
            }
            // Nếu phím tương ứng được nhấn, kích hoạt kỹ năng đặc biệt
            else if (Input.GetKeyDown(userConfiguration.getSpecialPowerKey()))
            {
                specialAttack.useSpecialAttack();
            }
        }
    }

    // Phương thức thực hiện đòn đánh
    void hit()
    {
        animator.SetTrigger("attack1"); // Kích hoạt hoạt ảnh tấn công
        applyDamageToEnemies(hitDamage, hitDamageToShield); // Gây sát thương cho kẻ địch được phát hiện
    }

    // Phương thức thực hiện đòn đá
    private void kick()
    {
        // Kích hoạt hoạt ảnh tấn công
        animator.SetTrigger("attack2"); // NÊN KHÁC NHAU CHO HOẠT ẢNH ĐÁ
        applyDamageToEnemies(kickDamage, kickDamageToShield);
    }



    // Phương thức gây sát thương cho kẻ địch được phát hiện
    private void applyDamageToEnemies(float damage, float damageToShield)
    {
        // Phát hiện người chơi đối phương trong khu vực "weaponHitBox"
        //Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(weaponHitBox.position, attackRange, otherPlayer);
        //Collider2D[] hitOtherPlayers = Physics2D.OverlapCapsuleAll(weaponHitBox.position, attackRange, )
        Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(weaponHitBox.position, attackRange);


        // Gây sát thương cho mỗi kẻ địch được phát hiện
        foreach (Collider2D playerEnemy in hitOtherPlayers)
        {

            //var health = playerEnemy.GetComponent<Health>();
            //var shield = playerEnemy.GetComponent<Shield>();
            Damageable damageable = playerEnemy.GetComponent<Damageable>();
            Shieldable shieldable = playerEnemy.GetComponent<Shieldable>();

            if (damageable != null && gameObject.tag != playerEnemy.tag)
            {
                if (shieldable == null || !shieldable.IsShieldActive())
                {
                    damageable.decreaseLife(damage);
                    Debug.Log("Chúng ta đã đánh trúng " + playerEnemy.name);
                    // Nạp thanh tấn công đặc biệt với mỗi đòn đánh trúng
                    specialAttack.increaseCharge(damage);
                }
                else
                {
                    shieldable.decreaseShieldCapacity(damageToShield);
                }
            }

        }
    }


    // Phương thức cần thiết để sử dụng các đối tượng con của GameObject trong trình chỉnh sửa
    private void OnValidate()
    {
        if (weaponHitBox == null)
        {
            weaponHitBox = transform.Find("WeaponHitBox");
            if (weaponHitBox == null)
            {
                Debug.LogWarning("Không tìm thấy WeaponHitBox. Đảm bảo có một GameObject con có tên 'WeaponHitBox'.");
            }
        }
    }

    // Vẽ Gizmo để hiển thị khu vực tấn công trong scene
    private void OnDrawGizmosSelected()
    {
        if (weaponHitBox == null)
        {
            return;
        }
        Gizmos.color = Color.red; // Màu của Gizmo
        Gizmos.DrawWireSphere(weaponHitBox.position, attackRange); // Khu vực hình tròn của phạm vi tấn công
    }

}
