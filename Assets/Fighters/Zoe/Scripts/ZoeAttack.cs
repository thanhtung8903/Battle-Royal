using System;
using UnityEngine;

public class ZoeAttack : MonoBehaviour
{

    public Animator animator; // Tham chiếu đến Animator để phát hoạt ảnh tấn công
    public Transform weaponHitBox; // Vị trí kiểm tra va chạm của vũ khí
    public float attackRange; // Phạm vi tấn công, nơi có thể phát hiện kẻ địch

    // Giá trị sát thương cho các loại tấn công khác nhau
    public float hitDamage;
    public float kickDamage;
    

    public float hitDamageToShield;
    public float kickDamageToShield;
    

    public float attackRate = 1f; // Tốc độ tấn công: số lần tấn công cho phép mỗi giây
    public float waitingTimeHit; // Thời gian chờ giữa các đòn đánh
    public float waitingTimeKick; // Thời gian chờ giữa các đòn đá
    private float nexAttackTime = 0f; // Thời gian phải chờ trước khi thực hiện đòn tấn công tiếp theo

    //public KeyCode hitKey;
    //public KeyCode kickKey;
    //public KeyCode specialPowerKey;

    private ZoeSpecialAttack specialAttack; // Tham chiếu đến kỹ năng đặc biệt
    private UserConfiguration userConfiguration; // Cấu hình người chơi

    // Âm thanh cho các đòn tấn công
    [SerializeField] private AudioClip soundAttack1;
    [SerializeField] private AudioClip soundAttack2;

    string ownTag; // Lưu tag của đối tượng này

    void Start()
    {
        specialAttack = GetComponent<ZoeSpecialAttack>();
        animator = GetComponent<Animator>();
        userConfiguration = GetComponent<UserConfiguration>();
        ownTag = gameObject.tag;
        //otherPlayer = LayerMask.GetMask("BaseFighter");
    }

    // Update được gọi một lần mỗi frame
    void Update()
    {
        // Chỉ cho phép tấn công nếu đã qua đủ thời gian chờ từ lần tấn công trước
        if (Time.time >= nexAttackTime)
        {
            // Nếu người chơi nhấn phím tương ứng, thực hiện đòn đánh
            if (Input.GetKeyDown(userConfiguration.getHitKey()))
            {
                hit();
                SoundsController.Instance.RunSound(soundAttack1);
                nexAttackTime = Time.time + waitingTimeHit / attackRate;
            }
            // Nếu người chơi nhấn phím tương ứng, thực hiện đòn đá
            else if (Input.GetKeyDown(userConfiguration.getKickKey()))
            {
                kick();
                SoundsController.Instance.RunSound(soundAttack2);
                nexAttackTime = Time.time + waitingTimeKick / attackRate;
            }
            // Nếu người chơi nhấn phím tương ứng, kích hoạt kỹ năng đặc biệt
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
        applyDamageToEnemies(hitDamage, hitDamageToShield); // Gây sát thương lên kẻ địch trong phạm vi
    }

    // Phương thức thực hiện đòn đá
    private void kick()
    {
        animator.SetTrigger("attack2"); // Kích hoạt hoạt ảnh tấn công đá
        applyDamageToEnemies(kickDamage, kickDamageToShield);
    }

    // Phương thức áp dụng sát thương lên kẻ địch bị đánh trúng
    public void applyDamageToEnemies(float damage, float damageToShield)
    {
        Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(weaponHitBox.position, attackRange);


        // Áp dụng sát thương lên từng kẻ địch bị đánh trúng
        foreach (Collider2D playerEnemy in hitOtherPlayers)
        {

            Damageable damageable = playerEnemy.GetComponent<Damageable>();
            Shieldable shield = playerEnemy.GetComponent<Shieldable>();

            if (damageable != null && gameObject.tag != playerEnemy.tag)
            {
                // Nếu đối thủ không có lá chắn hoặc lá chắn không hoạt động, gây sát thương trực tiếp
                if (shield == null || !shield.IsShieldActive())
                {
                    damageable.decreaseLife(damage);
                    Debug.Log("We hit " + playerEnemy.name);
                    // Tăng thanh năng lượng kỹ năng đặc biệt mỗi khi đánh trúng
                    specialAttack.increaseCharge(damage);
                }
                else
                {
                    // Nếu đối thủ có lá chắn, gây sát thương lên lá chắn
                    shield.decreaseShieldCapacity(damageToShield);
                }
            }

        }
    }


    // Phương thức giúp tìm kiếm thành phần con trong Unity Editor
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

    // Vẽ một vòng tròn Gizmo để hiển thị phạm vi tấn công trong Scene
    private void OnDrawGizmosSelected()
    {
        if (weaponHitBox == null)
        {
            return;
        }
        Gizmos.color = Color.red; // Màu sắc của Gizmo
        Gizmos.DrawWireSphere(weaponHitBox.position, attackRange); // Vẽ một vòng tròn thể hiện phạm vi tấn công
    }

  
}