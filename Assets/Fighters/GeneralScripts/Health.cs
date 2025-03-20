using System;
using UnityEngine;

public class Health : MonoBehaviour, Damageable
{
    public Animator animator; // Tham chiếu đến Animator để phát các animation tấn công
    public float currentHealth; // Máu hiện tại
    public float maxHealth; // Máu tối đa
    public int livesRemaining; // Số mạng còn lại

    private Vector2 startPosition; // Vị trí ban đầu của nhân vật
    private Rigidbody2D startRigidbody2D; // Lưu Rigidbody2D ban đầu
    private Vector3 originalLocalScale; // Lưu tỉ lệ gốc của nhân vật
    private Movement movement; // Điều khiển di chuyển
    private Attack attack; // Điều khiển tấn công
    private Shield shield; // Điều khiển khiên
    private SpecialAttack specialAttack; // Điều khiển đòn tấn công đặc biệt
    private Rigidbody2D rigidBody2D; // Thành phần Rigidbody2D của nhân vật
    private RigidbodyConstraints2D originalConstraints; // Giữ lại các ràng buộc gốc của Rigidbody2D
    private UserConfiguration userConfiguration; // Cấu hình của người chơi
    private UIController UIController; // Quản lý giao diện người dùng



    private void Start()
    {
        animator = GetComponent<Animator>();
        UIController = GetComponent<UIController>();
        livesRemaining = UIController.getNumberOfLives(); // Lấy số mạng từ UIController

        currentHealth = maxHealth;

        startPosition = transform.position; // Lưu vị trí ban đầu
        startRigidbody2D = GetComponent<Rigidbody2D>(); // Lưu Rigidbody2D ban đầu
        originalLocalScale = transform.localScale; // Lưu tỉ lệ gốc của nhân vật

        movement = GetComponent<Movement>();
        attack = GetComponent<Attack>();
        shield = GetComponent<Shield>();
        specialAttack = GetComponent<SpecialAttack>();
        userConfiguration = GetComponent<UserConfiguration>();

        rigidBody2D = GetComponent<Rigidbody2D>();

        // Lưu lại các ràng buộc ban đầu của Rigidbody2D
        originalConstraints = rigidBody2D.constraints;
    }

    void updateUI()
    {
        // Cập nhật thanh máu và số mạng trong giao diện
        UIController.updateHealthBar(currentHealth, maxHealth);
        UIController.updateLives(livesRemaining);
    }

    public void decreaseLife(float damage)
    {
        currentHealth -= damage;
        //animator.SetTrigger("hurt");

        if (currentHealth <= 0)
        {
            // Đóng băng vị trí X và xoay
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

            // Vô hiệu hóa các hành động khác
            specialAttack.enabled = false;
            attack.enabled = false;
            movement.enabled = false;
            shield.enabled = false;

            livesRemaining--;
            //animator.SetBool("isDead", true);
            //animator.SetTrigger("die");

            if (livesRemaining <= 0)
            {
                die(); // Nếu hết mạng, nhân vật sẽ bị hủy
            }
            else
            {
                // Hồi sinh nếu vẫn còn mạng
                specialAttack.enabled = true;
                attack.enabled = true;
                movement.enabled = true;
                shield.enabled = true;
                currentHealth = maxHealth;
                respawn();
               
            }
        }
        updateUI(); // Cập nhật UI sau khi mất máu
    }

    private void respawn()
    {
        // Tạm thời vô hiệu hóa va chạm của Rigidbody
        startRigidbody2D.simulated = false;

        // Làm cho nhân vật tạm thời vô hình
        transform.localScale = Vector3.zero;

        // Đưa nhân vật về vị trí ban đầu
        transform.position = startPosition;

        // Khôi phục hướng ban đầu của nhân vật
        if (userConfiguration != null)
        {
            userConfiguration.setFacingRight(userConfiguration.getFacingRight());
        }

        // Khôi phục lại các ràng buộc gốc của Rigidbody2D
        rigidBody2D.constraints = originalConstraints;

        // Làm nhân vật xuất hiện trở lại và kích hoạt va chạm
        transform.localScale = originalLocalScale;
        startRigidbody2D.simulated = true;

        // Bật lại tất cả các hành động của nhân vật
        specialAttack.enabled = true;
        attack.enabled = true;
        movement.enabled = true;
        shield.enabled = true;

        Debug.Log("Respawn completed successfully.");
    }



    private void die()
    {
        Debug.Log("Player " + gameObject.layer.ToString());
        Destroy(gameObject);
    }

    //public void setMaxHealth(float healthFromPersonaje)
    //{
    //    maxHealth = healthFromPersonaje;
    //}
}
