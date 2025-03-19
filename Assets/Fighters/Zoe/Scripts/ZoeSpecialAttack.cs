using System.Collections;
using UnityEngine;

public class ZoeSpecialAttack : MonoBehaviour
{
    public float specialCharge = 0f;  // Lượng năng lượng đặc biệt hiện tại
    public float maxCharge; // Lượng năng lượng tối đa

    private bool isReady = false;  // Kiểm tra xem kỹ năng đặc biệt đã sẵn sàng chưa
    private UIController UIController;  // Điều khiển giao diện người dùng

    public float specialPowerDamage; // Sát thương của kỹ năng đặc biệt
    public float specialPowerDamageToShield; // Sát thương gây lên lá chắn
    public Animator animator; // Animator để điều khiển hoạt ảnh
    private ZoeAttack attack; 

    
    public GameObject ballPrefab; 
    public int ballCount = 20; 
    public float expansionSpeed = 5f; 
    public int bursts = 3; 
    public float timeBetweenBursts = 0.5f; 

    [SerializeField] private AudioClip soundSpecialAttack1; // Âm thanh khi sử dụng kỹ năng đặc biệt

    private void Start()
    {
        UIController = GetComponent<UIController>();
        attack = GetComponent<ZoeAttack>(); 
        updateUI(); // Cập nhật giao diện ban đầu
    }

    // Tăng mức năng lượng đặc biệt
    public void increaseCharge(float amount)
    {
        if (!isReady) 
        {
            specialCharge += amount;
            specialCharge = Mathf.Clamp(specialCharge, 0, maxCharge);  // Đảm bảo giá trị không vượt quá giới hạn

            if (specialCharge >= maxCharge) 
            {
                isReady = true;
                Debug.Log("Special Attack Ready!");
            }

            updateUI();
        }
    }

    // Sử dụng kỹ năng đặc biệt
    public void useSpecialAttack()
    {
        if (isReady) 
        {
            Debug.Log("Special Attack Activated!");
            performSpecialAttack();
            specialCharge = 0f;
            isReady = false;
            updateUI();
        }
    }

    // Thực hiện kỹ năng đặc biệt
    private void performSpecialAttack()
    {
        special();
        Debug.Log("Performing the special attack!");
        
    }

    private void special()
    {
        // Kích hoạt hoạt ảnh tấn công
        SoundsController.Instance.RunSound(soundSpecialAttack1);
        animator.SetTrigger("special");
        attack.applyDamageToEnemies(specialPowerDamage, specialPowerDamageToShield);
    }

    // Phương thức này được gọi khi hoạt ảnh kỹ năng đặc biệt kết thúc
    private void OnSpecialAnimationEnd()
    {
        StartCoroutine(GenerateBursts());
    }

    // Tạo các đợt bắn liên tiếp
    private IEnumerator GenerateBursts()
    {
        animator.SetTrigger("balls");
        for (int i = 0; i < bursts; i++)
        {
            GenerateCircle();
            yield return new WaitForSeconds(timeBetweenBursts);
        }
    }

    // Sinh ra một vòng tròn quả cầu
    private void GenerateCircle()
    {
        float angleIncrement = 360f / ballCount;

        for (int i = 0; i < ballCount; i++)
        {
            
            float angle = i * angleIncrement * Mathf.Deg2Rad;

            // Vị trí ban đầu của quả cầu
            Vector2 initialPosition = transform.position;

            // Hướng bay của quả cầu theo hướng tròn
            Vector2 radialDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            // Tạo quả cầu mới
            GameObject ball = Instantiate(ballPrefab, initialPosition, Quaternion.identity);

            // Cấu hình thuộc tính cho quả cầu
            ball.GetComponent<BallMovement>().setUserTag(gameObject.tag);
            ball.GetComponent<BallMovement>().Initialize(radialDirection, expansionSpeed);
        }
    }

    // Cập nhật thanh năng lượng đặc biệt trên giao diện
    private void updateUI()
    {
        UIController.updateSpecialBar(specialCharge, maxCharge);
    }

    // Thiết lập giá trị năng lượng tối đa từ nhân vật
    public void setMaxCharge(float maxChargeFromCharacter)
    {
        this.maxCharge = maxChargeFromCharacter;
    }
}
