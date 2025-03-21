using System;
using UnityEngine;

public class KitsuneHealth : MonoBehaviour, Damageable
{
    public Animator animator; // Tham chiếu đến Animator để phát các hoạt ảnh tấn công
    public float currentHealth; // Lượng máu hiện tại
    public float maxHealth; // Lượng máu tối đa
    public int livesRemaining; // Số mạng còn lại

    private Vector2 startPosition; // Vị trí bắt đầu
    private Rigidbody2D startRigidbody2D; // Rigidbody2D ban đầu
    private Vector3 originalLocalScale; // Kích thước ban đầu
    private KitsuneMovement movement; // Thành phần điều khiển di chuyển
    private KitsuneAttack attack; // Thành phần điều khiển tấn công
    private KitsuneShield shield; // Thành phần điều khiển khiên
    private KitsuneSpecialAttack specialAttack; // Thành phần điều khiển tấn công đặc biệt
    private Rigidbody2D rigidBody2D; // Rigidbody2D của đối tượng
    private RigidbodyConstraints2D originalConstraints; // Các ràng buộc ban đầu của Rigidbody
    private UserConfiguration userConfiguration; // Cấu hình người dùng
    private UIController UIController; // Điều khiển giao diện người dùng



    private void Start()
    {
        animator = GetComponent<Animator>();
        UIController = GetComponent<UIController>();
        livesRemaining = UIController.getNumberOfLives();

        currentHealth = maxHealth;

        startPosition = transform.position;
        startRigidbody2D = GetComponent<Rigidbody2D>();
        originalLocalScale = transform.localScale;

        movement = GetComponent<KitsuneMovement>();
        attack = GetComponent<KitsuneAttack>();
        shield = GetComponent<KitsuneShield>();
        specialAttack = GetComponent<KitsuneSpecialAttack>();
        userConfiguration = GetComponent<UserConfiguration>();

        rigidBody2D = GetComponent<Rigidbody2D>();

        // Lưu các ràng buộc ban đầu của Rigidbody
        originalConstraints = rigidBody2D.constraints;
    }

    // Cập nhật giao diện người dùng
    void updateUI()
    {
        UIController.updateHealthBar(currentHealth, maxHealth);
        UIController.updateLives(livesRemaining);
    }

    // Giảm máu khi nhận sát thương
    public void decreaseLife(float damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("hurt");

        if (currentHealth <= 0)
        {
            // Đóng băng vị trí X và ngăn xoay
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

            // Vô hiệu hóa các thành phần điều khiển
            specialAttack.enabled = false;
            attack.enabled = false;
            movement.enabled = false;

            livesRemaining--;

            if (livesRemaining > 0)
            {
                currentHealth = maxHealth;
            }

            // Kích hoạt hoạt ảnh chết
            animator.SetTrigger("die");

        }
        updateUI();
    }

    // Phương thức này được thực thi khi hoạt ảnh chết kết thúc
    public void OnDeathAnimationComplete()
    {
        if (livesRemaining <= 0)
        {
            die();
        }
        else
        {
            currentHealth = maxHealth;
            respawn();
            //currentHealth = maxHealth;

            // Kích hoạt lại các thành phần điều khiển
            specialAttack.enabled = true;
            attack.enabled = true;
            movement.enabled = true;

            // Khôi phục các ràng buộc ban đầu
            rigidBody2D.constraints = originalConstraints;

            // Điều chỉnh nhẹ vị trí để buộc tính toán lại va chạm
            rigidBody2D.position = new Vector2(rigidBody2D.position.x, rigidBody2D.position.y + 0.01f);
            animator.SetBool("isDead", false);
        }
    }

    // Hồi sinh nhân vật
    private void respawn()
    {
        // Tạm thời vô hiệu hóa mô phỏng của Rigidbody
        startRigidbody2D.simulated = false;

        // Tạm thời làm cho người chơi trở nên vô hình
        transform.localScale = Vector3.zero;

        // Đặt lại vị trí ban đầu của người chơi
        transform.position = startPosition;

        // Khôi phục hướng dựa trên 'facingRight'
        if (userConfiguration != null)
        {
            userConfiguration.setFacingRight(userConfiguration.getFacingRight());
        }

        // Làm người chơi hiện lại và kích hoạt lại mô phỏng
        transform.localScale = originalLocalScale;
        startRigidbody2D.simulated = true;

        Debug.Log("Hồi sinh thành công.");
    }

    // Xử lý khi nhân vật chết hoàn toàn
    private void die()
    {
        Debug.Log("Người chơi " + gameObject.layer.ToString());
        GameManager.gameManagerInstance.enableGameOverPanel(gameObject.tag);
        Destroy(gameObject);
    }
}
