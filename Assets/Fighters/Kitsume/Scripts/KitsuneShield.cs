using UnityEngine;
using System.Collections;

public class KitsuneShield : MonoBehaviour, Shieldable
{
    [Header("Shield Components")]
    public BoxCollider2D boxCollider2D; // Thành phần BoxCollider2D của khiên
    public SpriteRenderer spriteRenderer; // Thành phần SpriteRenderer của khiên

    [Header("Shield Settings")]
    public float shieldDuration; // Thời gian nạp lại nếu khiên bị vô hiệu hóa
    public float shieldCapacity = 0; // Khả năng chịu đựng của khiên
    public float maxShieldCapacity; // Khả năng chịu đựng tối đa của khiên
    public float rechargeRate; // Tốc độ nạp lại mỗi giây
    private bool isShieldActive = false; // Trạng thái của khiên
    private bool isRechargingFromZero = false; // Để kiểm soát việc nạp lại sau khi cạn kiệt

    //public KeyCode shieldKey = KeyCode.V;

    private Rigidbody2D rb; // Thành phần Rigidbody2D
    private RigidbodyConstraints2D originalConstraints; // Ràng buộc ban đầu của Rigidbody

    private UserConfiguration userConfiguration; // Cấu hình người dùng

    [Header("Scripts to Disable")]
    public MonoBehaviour specialAttack; // Script tấn công đặc biệt
    public MonoBehaviour fighterAttack; // Script tấn công cơ bản
    public MonoBehaviour fighterHealth; // Script quản lý máu
    public MonoBehaviour fighterMovement; // Script di chuyển

    void Start()
    {
        shieldCapacity = maxShieldCapacity;

        Transform shield = transform.Find("Shield");
        boxCollider2D = shield.GetComponent<BoxCollider2D>();

        spriteRenderer = shield.GetComponent<SpriteRenderer>();

        specialAttack = GetComponent<KitsuneSpecialAttack>();
        fighterAttack = GetComponent<KitsuneAttack>();
        fighterHealth = GetComponent<KitsuneHealth>();
        fighterMovement = GetComponent<KitsuneMovement>();

        rb = GetComponent<Rigidbody2D>();

        // Lưu các ràng buộc ban đầu của Rigidbody
        originalConstraints = rb.constraints;

        userConfiguration = GetComponent<UserConfiguration>();
    }

    void Update()
    {
        // Kích hoạt hoặc vô hiệu hóa khiên khi nhấn phím "V", chỉ khi không đang nạp lại từ 0
        if (Input.GetKeyDown(userConfiguration.getShieldKey()) && !isRechargingFromZero)
        {
            ToggleShield();
        }

        // Nạp lại khiên nếu không đang kích hoạt và không đang nạp lại từ 0
        if (!isShieldActive && !isRechargingFromZero && shieldCapacity < maxShieldCapacity)
        {
            RechargeShield();
        }
    }

    // Bật/tắt khiên
    private void ToggleShield()
    {
        // Nếu khiên đang nạp lại từ 0, không thể kích hoạt
        if (isRechargingFromZero)
        {
            Debug.Log("Khiên đang nạp lại từ 0 và không thể kích hoạt.");
            return;
        }

        isShieldActive = !isShieldActive;
        UpdateShieldComponents();
        UpdateScriptStates();
    }

    // Cập nhật các thành phần của khiên
    private void UpdateShieldComponents()
    {
        // Cập nhật khả năng hiển thị và va chạm của khiên
        boxCollider2D.enabled = isShieldActive;
        spriteRenderer.enabled = isShieldActive;
        // Hạn chế di chuyển theo trục X và đóng băng xoay
        if (isShieldActive)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            Debug.Log("Khiên đã kích hoạt");
        }
        else
        {
            // Khôi phục các ràng buộc ban đầu
            rb.constraints = originalConstraints;

            // Điều chỉnh nhẹ vị trí để buộc tính toán lại va chạm
            rb.position = new Vector2(rb.position.x, rb.position.y + 0.01f);

            Debug.Log("Khiên đã vô hiệu hóa");
        }
    }

    // Cập nhật trạng thái của các script
    private void UpdateScriptStates()
    {
        // Kích hoạt hoặc vô hiệu hóa các script dựa trên trạng thái của khiên
        bool isActive = !isShieldActive; // Các script hoạt động khi khiên không hoạt động
        specialAttack.enabled = isActive;
        fighterAttack.enabled = isActive;
        fighterHealth.enabled = isActive;
        fighterMovement.enabled = isActive;
    }

    // Nhận sát thương vào khiên
    public void TakeDamage(float damage)
    {
        if (!isShieldActive || isRechargingFromZero)
            return;

        shieldCapacity -= damage;
        Debug.Log($"Khả năng của khiên: {shieldCapacity}");

        if (shieldCapacity <= 0)
        {
            shieldCapacity = 0;
            StartCoroutine(DeactivateAndRechargeShieldFromZero());
        }
    }

    // Vô hiệu hóa và nạp lại khiên từ 0
    private IEnumerator DeactivateAndRechargeShieldFromZero()
    {
        isRechargingFromZero = true;
        isShieldActive = false;
        UpdateShieldComponents();
        UpdateScriptStates();

        Debug.Log("Khiên đã cạn kiệt hoàn toàn và đang nạp lại...");
        yield return new WaitForSeconds(shieldDuration);

        isRechargingFromZero = false;
        Debug.Log("Hẹn giờ nạp lại khiên hoàn tất. Bắt đầu nạp lại thụ động.");
    }

    // Nạp lại khiên
    private void RechargeShield()
    {
        shieldCapacity += rechargeRate * Time.deltaTime;
        if (shieldCapacity > maxShieldCapacity)
        {
            shieldCapacity = maxShieldCapacity;
        }

        Debug.Log($"Nạp lại khiên thụ động: {shieldCapacity}");
    }

    /// <summary>
    /// Trả về trạng thái hoạt động của khiên
    /// </summary>
    public bool IsShieldActive()
    {
        return isShieldActive && !isRechargingFromZero;
    }

    // Giảm khả năng của khiên
    public void decreaseShieldCapacity(float amount)
    {
        TakeDamage(amount);
    }
}
