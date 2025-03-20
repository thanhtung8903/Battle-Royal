using Unity.VisualScripting;
using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    public float specialCharge = 0f; // Lượng nạp hiện tại của thanh kỹ năng đặc biệt (bắt đầu từ 0).
    public float maxCharge; // Lượng nạp tối đa để kích hoạt kỹ năng đặc biệt.

    private bool isReady = false; // Biến kiểm tra xem kỹ năng đặc biệt đã sẵn sàng chưa.
    private UIController UIController; // Tham chiếu đến bộ điều khiển giao diện người dùng (UI).

    private void Start()
    {
        UIController = GetComponent<UIController>();
        updateUI();
        
    }

    // Phương thức tăng lượng nạp của thanh kỹ năng đặc biệt.
    public void increaseCharge(float amount)
    {
        if (!isReady) // Nếu kỹ năng chưa sẵn sàng, tiếp tục nạp.
        {
            specialCharge += amount;
            specialCharge = Mathf.Clamp(specialCharge, 0, maxCharge); // Đảm bảo không vượt quá giới hạn tối đa.

            if (specialCharge >= maxCharge)  // Khi thanh nạp đầy, đánh dấu là đã sẵn sàng.
            {
                isReady = true;
                Debug.Log("Special Attack Ready!");
            }

            updateUI();
        }
    }

    // Phương thức kích hoạt kỹ năng đặc biệt.
    public void useSpecialAttack()
    {
        if (isReady) // Chỉ có thể sử dụng khi đã được nạp đầy.
        {
            Debug.Log("Special Attack Activated!");
            performSpecialAttack(); // Gọi phương thức thực hiện kỹ năng đặc biệt.
            specialCharge = 0f; // Đặt lại thanh nạp về 0.
            isReady = false;
            updateUI();
        }
    }

    private void performSpecialAttack()
    {
       
        Debug.Log("Performing the special attack!");
    }

    private void updateUI()
    {
        UIController.updateSpecialBar(specialCharge, maxCharge);
    }

    public void setMaxCharge(float maxChargeFromPersonaje)
    {
        this.maxCharge = maxChargeFromPersonaje;
    }
}
