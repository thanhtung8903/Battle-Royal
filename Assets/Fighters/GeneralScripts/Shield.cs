using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour, Shieldable
{
    [Header("Shield Components")]
    public BoxCollider2D boxCollider2D;
    public SpriteRenderer spriteRenderer;

    [Header("Shield Settings")]
    public float shieldDuration; // Thời gian hồi chiêu nếu khiên bị vô hiệu hóa.
    public float shieldCapacity = 0; // Dung lượng hiện tại của khiên.
    public float maxShieldCapacity; // Dung lượng tối đa của khiên.
    public float rechargeRate; // Tốc độ hồi khiên mỗi giây.
    private bool isShieldActive = false; // Trạng thái khiên (đang bật hay không).
    private bool isRechargingFromZero = false; // Kiểm soát quá trình hồi khiên từ 0.

    //public KeyCode shieldKey = KeyCode.V;

    private Rigidbody2D rb;
    private RigidbodyConstraints2D originalConstraints;

    private UserConfiguration userConfiguration;

    [Header("Scripts to Disable")]
    public MonoBehaviour specialAttack;
    public MonoBehaviour fighterAttack;
    public MonoBehaviour fighterHealth;
    public MonoBehaviour fighterMovement;

    void Start()
    {
        shieldCapacity = maxShieldCapacity;

        Transform shield = transform.Find("Shield");
        boxCollider2D = shield.GetComponent<BoxCollider2D>();

        spriteRenderer = shield.GetComponent<SpriteRenderer>();

        specialAttack = GetComponent<SpecialAttack>();
        fighterAttack = GetComponent<Attack>();
        fighterHealth = GetComponent<Health>();
        fighterMovement = GetComponent<Movement>();

        rb = GetComponent<Rigidbody2D>();

        // Lưu lại trạng thái gốc của Rigidbody
        originalConstraints = rb.constraints;

        userConfiguration = GetComponent<UserConfiguration>();
    }

    void Update()
    {
        // Bật hoặc tắt khiên khi nhấn phím "V", chỉ khi không đang hồi lại từ 0.
        if (Input.GetKeyDown(userConfiguration.getShieldKey()) && !isRechargingFromZero)
        {
            ToggleShield();
        }

        // Hồi phục khiên nếu không hoạt động và không đang hồi từ 0.
        if (!isShieldActive && !isRechargingFromZero && shieldCapacity < maxShieldCapacity)
        {
            RechargeShield();
        }
    }

    private void ToggleShield()
    {
        // Nếu khiên đang hồi từ 0, không thể kích hoạt.
        if (isRechargingFromZero)
        {
            Debug.Log("Shield is recharging from zero and cannot be activated.");
            return;
        }

        isShieldActive = !isShieldActive;
        UpdateShieldComponents();
        UpdateScriptStates();
    }

    private void UpdateShieldComponents()
    {
        // Cập nhật trạng thái hiển thị và va chạm của khiên.
        boxCollider2D.enabled = isShieldActive;
        spriteRenderer.enabled = isShieldActive;
        // Giới hạn chuyển động theo trục X và khóa xoay
        if (isShieldActive)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            Debug.Log("Shield Activated");
        }
        else
        {
            // Khôi phục lại trạng thái gốc
            rb.constraints = originalConstraints;

            // Điều chỉnh nhẹ vị trí để cập nhật va chạm
            rb.position = new Vector2(rb.position.x, rb.position.y + 0.01f);

            Debug.Log("Shield Deactivated");
        }
    }

    private void UpdateScriptStates()
    {
        // Bật hoặc tắt các script dựa trên trạng thái của khiên.
        bool isActive = !isShieldActive; // Các script chỉ hoạt động khi khiên không được kích hoạt.
        specialAttack.enabled = isActive;
        fighterAttack.enabled = isActive;
        fighterHealth.enabled = isActive;
        fighterMovement.enabled = isActive;
    }

    public void TakeDamage(float damage)
    {
        if (!isShieldActive || isRechargingFromZero)
            return;

        shieldCapacity -= damage;
        Debug.Log($"Shield capacity: {shieldCapacity}");

        if (shieldCapacity <= 0)
        {
            shieldCapacity = 0;
            StartCoroutine(DeactivateAndRechargeShieldFromZero());
        }
    }

    private IEnumerator DeactivateAndRechargeShieldFromZero()
    {
        isRechargingFromZero = true;
        isShieldActive = false;
        UpdateShieldComponents();
        UpdateScriptStates();

        Debug.Log("Shield is fully depleted and recharging...");
        yield return new WaitForSeconds(shieldDuration);

        isRechargingFromZero = false;
        Debug.Log("Shield recharge timer complete. Passive recharge started.");
    }

    private void RechargeShield()
    {
        shieldCapacity += rechargeRate * Time.deltaTime;
        if (shieldCapacity > maxShieldCapacity)
        {
            shieldCapacity = maxShieldCapacity;
        }

        Debug.Log($"Passive Shield Recharge: {shieldCapacity}");
    }

    /// <summary>
    /// Devuelve si el escudo está activo.
    /// </summary>
    public bool IsShieldActive()
    {
        return isShieldActive && !isRechargingFromZero;
    }

    public void decreaseShieldCapacity(float amount)
    {
        TakeDamage(amount);
    }

    //public void setShieldKey(KeyCode shieldKey)
    //{
    //    this.shieldKey = shieldKey;
    //}

    //public void setShieldDuration(float shieldDuration)
    //{
    //    this.shieldDuration = shieldDuration;
    //}

    //public void setMaxShieldCapacity(float maxShieldCapacityFromPersonaje)
    //{
    //    maxShieldCapacity = maxShieldCapacityFromPersonaje;
    //}

    //public void setRechargeRate(float rechargeRate)
    //{
    //    this.rechargeRate = rechargeRate;
    //}
}
