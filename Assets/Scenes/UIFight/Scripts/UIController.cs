using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Tham chiếu đến các phần tử giao diện người dùng
    [Header("UI Elements")]
    [SerializeField] private Image healthBarFill;
    [SerializeField] private List<Image> lives;
    [SerializeField] private Image specialBarFill;

    // Cập nhật thanh máu
    public void updateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0, 1);
        }
    }

    // Cập nhật số mạng còn lại
    public void updateLives(int livesRemaining)
    {
        for (int i = 0; i < lives.Count; i++)
        {
            lives[i].enabled = i < livesRemaining;
        }
    }

    // Cập nhật thanh nạp cho đòn tấn công đặc biệt
    public void updateSpecialBar(float currentCharge, float maxCharge)
    {
        if (specialBarFill != null)
        {
            specialBarFill.fillAmount = Mathf.Clamp(currentCharge / maxCharge, 0, 1);
        }
    }

    // Các phương thức để gán tham chiếu từ các script khác
    public void setHealthBarFill(Image healthBar)
    {
        this.healthBarFill = healthBar;
    }

    public void setLives(List<Image> livesList)
    {
        lives = livesList;
    }

    public int getNumberOfLives()
    {
        return lives.Count;
    }

    public void setSpecialBarFill(Image specialBar)
    {
        this.specialBarFill = specialBar;
    }
}
