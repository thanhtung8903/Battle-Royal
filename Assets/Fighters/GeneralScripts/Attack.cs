using System;
using UnityEngine;

public class Attack : MonoBehaviour
{
    
    public Animator animator; // Tham chiếu đến Animator để phát các animation tấn công
    public Transform weaponHitBox; // Vị trí kiểm tra va chạm của vũ khí
    public float attackRange; // Phạm vi có thể phát hiện kẻ địch

    // Giá trị sát thương cho các kiểu tấn công khác nhau
    public float hitDamage;
    public float kickDamage;
    public float specialPowerDamage;
    
    public float hitDamageToShield; 
    public float kickDamageToShield;
    
    public float attackRate = 1f; // Tốc độ tấn công: số lần tấn công được phép mỗi giây
    public float waitingTimeHit; // Thời gian chờ giữa các đòn đánh
    public float waitingTimeKick; // Thời gian chờ giữa các cú đá
    private float nexAttackTime = 0f; // Bộ đếm thời gian chờ cho lần tấn công tiếp theo

    //public KeyCode hitKey;
    //public KeyCode kickKey;
    //public KeyCode specialPowerKey;

    private SpecialAttack specialAttack;
    private UserConfiguration userConfiguration;

    // Âm thanh khi tấn công
    [SerializeField] private AudioClip soundAttack1;

    string ownTag;

    void Start()
    {
        specialAttack = GetComponent<SpecialAttack>();
        animator = GetComponent<Animator>();
        userConfiguration = GetComponent<UserConfiguration>();
        ownTag = gameObject.tag;
        //otherPlayer = LayerMask.GetMask("BaseFighter");
    }

    // Update được gọi mỗi frame
    void Update()
    {
        // Chỉ cho phép tấn công nếu đủ thời gian chờ kể từ lần tấn công trước
        if (Time.time >= nexAttackTime)
        {
            // Nếu nhấn phím tấn công, thực hiện đòn đánh
            if (Input.GetKeyDown(userConfiguration.getHitKey()))
            {
                hit();
                SoundsController.Instance.RunSound(soundAttack1);
                nexAttackTime = Time.time + waitingTimeHit / attackRate;
            }
            // Nếu nhấn phím đá, thực hiện cú đá
            else if (Input.GetKeyDown(userConfiguration.getKickKey()))
            {
                kick();
                nexAttackTime = Time.time + waitingTimeKick / attackRate;
            }
            // Nếu nhấn phím chiêu đặc biệt, kích hoạt chiêu đặc biệt
            else if (Input.GetKeyDown(userConfiguration.getSpecialPowerKey()))
            {
                specialAttack.useSpecialAttack();
            }
        }
    }

    // Phương thức thực hiện đòn đánh
    void hit()
    {
        animator.SetTrigger("attack1"); // Kích hoạt animation tấn công
        applyDamageToEnemies(hitDamage, hitDamageToShield); // Gây sát thương lên kẻ địch trong phạm vi
    }

    // Phương thức thực hiện cú đá
    private void kick()
    {
        // Kích hoạt animation đá
        animator.SetTrigger("attack2"); // Đòn đá nên có animation khác với đòn đánh
        applyDamageToEnemies(kickDamage, kickDamageToShield);
    }

    // Phương thức áp dụng sát thương lên kẻ địch bị phát hiện
    private void applyDamageToEnemies(float damage, float damageToShield)
    {
        // Xác định các đối thủ trong phạm vi weaponHitBox
        Collider2D[] hitOtherPlayers = Physics2D.OverlapCircleAll(weaponHitBox.position, attackRange);


        // Áp dụng sát thương cho từng đối thủ bị phát hiện
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
                    Debug.Log("We performAttack1 " + playerEnemy.name);
                    // Tăng thanh nạp đòn đánh đặc biệt sau mỗi đòn trúng
                    specialAttack.increaseCharge(damage);
                }
                else
                {
                    shieldable.decreaseShieldCapacity(damageToShield);
                }
            }
            
        }
    }


    // Phương thức kiểm tra và đặt vị trí weaponHitBox nếu bị thiếu
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

    // Vẽ Gizmo để hiển thị phạm vi tấn công trong cửa sổ Scene
    private void OnDrawGizmosSelected()
    {
        if (weaponHitBox == null)
        {
            return;
        }
        Gizmos.color = Color.red; // Màu của Gizmo
        Gizmos.DrawWireSphere(weaponHitBox.position, attackRange); // Vẽ hình tròn thể hiện phạm vi tấn công
    }

}
