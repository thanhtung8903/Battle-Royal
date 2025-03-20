using System.Collections;
using UnityEngine;

public class ZoeSpecialAttack : MonoBehaviour
{
    public float specialCharge = 0f;  // Lượng nạp hiện tại của thanh đặc biệt (bắt đầu từ 0).
    public float maxCharge; // Lượng nạp tối đa để kích hoạt đòn tấn công đặc biệt.

    private bool isReady = false; // Biến kiểm tra xem đòn tấn công đặc biệt đã sẵn sàng hay chưa.
    private UIController UIController; // Tham chiếu đến bộ điều khiển giao diện người dùng (UI).

    public float specialPowerDamage; // Lượng sát thương của đòn đặc biệt.
    public float specialPowerDamageToShield; // Lượng sát thương gây lên khiên của đối thủ.
    public Animator animator; // Tham chiếu đến Animator để phát hoạt ảnh tấn công.
    private ZoeAttack attack;

    // Cấu hình cho đòn tấn công đặc biệt với các quả cầu năng lượng.
    public GameObject ballPrefab; // Prefab của quả cầu năng lượng.
    public int ballCount = 20; // Số lượng quả cầu xuất hiện theo vòng tròn.
    public float expansionSpeed = 5f; // Tốc độ mở rộng của vòng tròn quả cầu.
    public int bursts = 3; // Số lần phóng quả cầu.
    public float timeBetweenBursts = 0.5f; // Khoảng thời gian giữa các lần phóng.

    [SerializeField] private AudioClip soundSpecialAttack1; // Âm thanh của đòn tấn công đặc biệt.

    private void Start()
    {
        UIController = GetComponent<UIController>();
        attack = GetComponent<ZoeAttack>(); // Khởi tạo biến attack.
        updateUI();
    }

    // Phương thức tăng lượng nạp của thanh đặc biệt.
    public void increaseCharge(float amount)
    {
        if (!isReady) // Nếu đòn tấn công đặc biệt chưa sẵn sàng, tiếp tục nạp thanh đặc biệt.
        {
            specialCharge += amount;
            specialCharge = Mathf.Clamp(specialCharge, 0, maxCharge); // Đảm bảo giá trị không vượt quá maxCharge.

            if (specialCharge >= maxCharge) // Khi thanh nạp đầy, đánh dấu là đã sẵn sàng.
            {
                isReady = true;
                Debug.Log("Special Attack Ready!");
            }

            updateUI();
        }
    }

    // Phương thức sử dụng đòn tấn công đặc biệt.
    public void useSpecialAttack()
    {
        if (isReady)  // Chỉ có thể sử dụng nếu thanh đặc biệt đã được nạp đầy.
        {
            Debug.Log("Special Attack Activated!");
            performSpecialAttack(); // Gọi đến hàm thực hiện đòn tấn công đặc biệt.
            specialCharge = 0f; // Đặt lại thanh nạp về 0.
            isReady = false;
            updateUI();
        }
    }

    private void performSpecialAttack()
    {
        special();
        Debug.Log("Performing the special attack!");
        // Việc phóng các quả cầu sẽ được kích hoạt bằng Animation Event khi kết thúc hoạt ảnh đặc biệt.
    }

    private void special()
    {
        // Kích hoạt hoạt ảnh tấn công đặc biệt.
        SoundsController.Instance.RunSound(soundSpecialAttack1);
        animator.SetTrigger("special");
        attack.applyDamageToEnemies(specialPowerDamage, specialPowerDamageToShield);
    }

    // Phương thức này sẽ được gọi khi hoạt ảnh "special" kết thúc thông qua Animation Event.
    private void OnSpecialAnimationEnd()
    {
        StartCoroutine(GenerateBursts());
    }

    private IEnumerator GenerateBursts()
    {
        animator.SetTrigger("balls");
        for (int i = 0; i < bursts; i++)
        {
            GenerateCircle(); // Tạo vòng tròn quả cầu năng lượng.
            yield return new WaitForSeconds(timeBetweenBursts); // Chờ một khoảng thời gian giữa các lần phóng.
        }
    }

    private void GenerateCircle()
    {
        float angleIncrement = 360f / ballCount;  // Góc giữa mỗi quả cầu trong vòng tròn.

        for (int i = 0; i < ballCount; i++)
        {
            // Tính toán góc cho mỗi quả cầu.
            float angle = i * angleIncrement * Mathf.Deg2Rad;

            // Vị trí khởi đầu ở trung tâm nhân vật.
            Vector2 initialPosition = transform.position;

            // Xác định hướng tỏa ra theo góc tính toán.
            Vector2 radialDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            // Tạo một quả cầu mới ở trung tâm nhân vật.
            GameObject ball = Instantiate(ballPrefab, initialPosition, Quaternion.identity);

            // Thiết lập quả cầu để nó di chuyển tỏa ra từ trung tâm.
            ball.GetComponent<BallMovement>().setUserTag(gameObject.tag);
            ball.GetComponent<BallMovement>().Initialize(radialDirection, expansionSpeed);
        }
    }

    private void updateUI()
    {
        UIController.updateSpecialBar(specialCharge, maxCharge);
    }

    public void setMaxCharge(float maxChargeFromCharacter)
    {
        this.maxCharge = maxChargeFromCharacter;
    }
}
