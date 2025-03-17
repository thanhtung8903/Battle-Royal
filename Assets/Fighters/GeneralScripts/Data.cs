using UnityEngine;

[CreateAssetMenu(fileName = "New Fighter", menuName = "Fighter")]

public class FightersData : ScriptableObject
{
    // 1. KHAI BÁO CÁC THUỘC TÍNH

    // Thuộc tính chung của nhân vật
    [Header("Dữ liệu chung của nhân vật")]
    [SerializeField] private Sprite fighterImage; // Hình ảnh của nhân vật
    [SerializeField] private string fighterName; // Tên của nhân vật
    [SerializeField] private string fighterDescription; // Mô tả của nhân vật
    [SerializeField] private GameObject fighterPrefab; // Prefab của nhân vật

    // Thuộc tính của Script Fighter Movement
    [Header("Script Fighter Movement")]
    // [SerializeField] private float speed; // Tốc độ di chuyển
    // [SerializeField] private float jumpForce; // Lực nhảy
    [SerializeField] private float groundCheckRadius = 0.1f; // Bán kính kiểm tra mặt đất (Có giá trị mặc định)
    // Thuộc tính phụ thuộc vào Player 1 hoặc Player 2: axis, jumpKey và downKey.

    // Thuộc tính của Script Fighter Health
    [Header("Script Fighter Health")]
    [SerializeField] private float maxHealth = 100f; // Máu tối đa
    // Thuộc tính phụ thuộc vào Player 1 hoặc Player 2: healthBarFill và lives.

    // Thuộc tính của Script Fighter Attack
    [Header("Script Fighter Attack")]
    [SerializeField] private float attackRange = 0.5f; // Phạm vi tấn công (Có giá trị mặc định)

    // [SerializeField] private float attack1Value; 
    // [SerializeField] private float attack2Value;
    // [SerializeField] private float specialPowerDamage;

    // [SerializeField] private float attack1ValueToShield; // Sát thương lên khiên khi đánh
    // [SerializeField] private float attack2ValueToShield; // Sát thương lên khiên khi đá

    [SerializeField] private float attackRate = 1f; // Tỷ lệ tấn công: số lần tấn công mỗi giây được phép (Có giá trị mặc định)
    // [SerializeField] private float waitingTimeAttack1; // Thời gian chờ giữa các lần đánh
    // [SerializeField] private float waitingTimeAttack2; // Thời gian chờ giữa các lần đá
    // Thuộc tính phụ thuộc vào Player 1 hoặc Player 2: hitKey, kickKey và specialPowerKey.

    // Thuộc tính của Script Special Attack
    [Header("Script Special Attack")]
    [SerializeField] private float maxCharge = 100f; // Tối đa năng lượng đặc biệt
    // Thuộc tính phụ thuộc vào Player 1 hoặc Player 2: specialBarFill.

    // Thuộc tính của Script Fighter Shield
    [Header("Script Fighter Shield")]
    [SerializeField] private float shieldDuration = 5f; // Thời gian hồi lại nếu khiên bị tắt
    [SerializeField] private float maxShieldCapacity = 100f; // Dung lượng tối đa của khiên
    [SerializeField] private float rechargeRate = 10f; // Lượng hồi mỗi giây
    // Thuộc tính phụ thuộc vào Player 1 hoặc Player 2: shieldKey.

    // ------------------------------------------------------------------------------------------------------------------------------------------
    // 2. CÁC PHƯƠNG THỨC

    public Sprite getFighterImage() { return fighterImage; } // Lấy hình ảnh của nhân vật
    public string getFighterName() { return fighterName; } // Lấy tên của nhân vật
    public string getFighterDescription() { return fighterDescription; } // Lấy mô tả của nhân vật
    public GameObject getFighterPrefab() { return fighterPrefab; } // Lấy prefab của nhân vật
    // public float getSpeed() { return speed; }
    // public float getJumpForce() { return jumpForce; }
    public float getGroundCheckRadius() { return groundCheckRadius; } // Lấy bán kính kiểm tra mặt đất
    public float getMaxHealth() { return maxHealth; } // Lấy máu tối đa
    public float getAttackRange() { return attackRange; } // Lấy phạm vi tấn công
    // public float getHitDamage() { return attack1Value; }
    // public float getKickDamage() { return attack2Value; }
    // public float getSpecialPowerDamage() { return specialPowerDamage; }
    // public float getHitDamageToShield() { return attack1ValueToShield; }
    // public float getKickDamageToShield() { return attack2ValueToShield; }
    public float getAttackRate() { return attackRate; } // Lấy tỷ lệ tấn công
    // public float getWaitingTimeHit() { return waitingTimeAttack1; }
    // public float getWaitingTimeKick() { return waitingTimeAttack2; }
    public float getMaxCharge() { return maxCharge; } // Lấy tối đa năng lượng đặc biệt
    public float getShieldDuration() { return shieldDuration; } // Lấy thời gian hồi khiên
    public float getMaxShieldCapacity() { return maxShieldCapacity; } // Lấy dung lượng tối đa của khiên
    public float getRechargeRate() { return rechargeRate; } // Lấy lượng hồi mỗi giây
}