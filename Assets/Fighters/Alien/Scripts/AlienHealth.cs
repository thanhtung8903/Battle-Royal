using System;
using UnityEngine;

public class AlienHealth : MonoBehaviour, Damageable
{
    [Header("Health Settings")]
    [SerializeField] private float currentHealth; // Máu hiện tại
    [SerializeField] private float maxHealth; // Máu tối đa
    [SerializeField] private int livesRemaining; // Số mạng còn lại

    [Header("Respawn Settings")]
    [SerializeField] private Vector2 startPosition; // Vị trí bắt đầu
    [SerializeField] private Rigidbody2D startRigidbody2D;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private RigidbodyConstraints2D originalConstraints; // Lưu lại các ràng buộc ban đầu của Rigidbody
    [SerializeField] private Vector3 originalLocalScale; // Tỷ lệ ban đầu của nhân vật

    [Header("Components")]
    [SerializeField] private Animator animator; // Animator để điều khiển hoạt ảnh
    [SerializeField] private AudioClip soundHurt; // Âm thanh khi bị thương
    [SerializeField] private AudioClip soundDie; // Âm thanh khi chết

    [Header("Scripts")]
    [SerializeField] private AlienMovement movement; // Script di chuyển
    [SerializeField] private AlienAttack attack; // Script tấn công
    [SerializeField] private AlienShield shield; // Script lá chắn 
    [SerializeField] private AlienSpecialAttack specialAttack; // Script tấn công đặc biệt

    [SerializeField] private UserConfiguration userConfiguration; // Cấu hình người chơi
    [SerializeField] private UIController UIController;    // Điều khiển giao diện người dùng   

    private void Start()
    {
        animator = GetComponent<Animator>();
        startRigidbody2D = GetComponent<Rigidbody2D>();

        rigidBody2D = GetComponent<Rigidbody2D>();
        originalConstraints = rigidBody2D.constraints; // Lưu lại ràng buộc gốc của Rigidbody

        startPosition = transform.position; // Lưu vị trí ban đầu
        originalLocalScale = transform.localScale; // Lưu tỷ lệ gốc

        movement = GetComponent<AlienMovement>();
        attack = GetComponent<AlienAttack>();
        shield = GetComponent<AlienShield>();
        specialAttack = GetComponent<AlienSpecialAttack>();

        userConfiguration = GetComponent<UserConfiguration>();
        UIController = GetComponent<UIController>();
        livesRemaining = UIController.getNumberOfLives(); // Lấy số mạng từ UI
        currentHealth = maxHealth; // Đặt máu ban đầu bằng máu tối đa

    }

    // Cập nhật giao diện (UI)
    void updateUI()
    {
        UIController.updateHealthBar(currentHealth, maxHealth);
        UIController.updateLives(livesRemaining);
    }


    // Gây sát thương cho nhân vật
    public void decreaseLife(float damage)
    {
        if(currentHealth < 0)
        {
            return;
        }

        currentHealth -= damage; // Trừ máu
        SoundsController.Instance.RunSound(soundHurt); // Phát âm thanh bị thương
        animator.SetTrigger("hurt"); // Gọi hoạt ảnh bị thương

        if (currentHealth > 0)
        {
            updateUI();
            return;
        }
        manageDead(); // Gọi xử lý khi chết
    }

    // Xử lý khi nhân vật chết
    public void manageDead()
    {
        rigidBody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        specialAttack.enabled = false;
        attack.enabled = false;
        movement.enabled = false;

        livesRemaining--; // Giảm số mạng còn lại

        SoundsController.Instance.RunSound(soundDie); // Phát âm thanh chết
        animator.SetTrigger("die"); // Gọi hoạt ảnh chết
    }

    // Phương thức này sẽ được gọi khi hoạt ảnh chết kết thúc
    public void onDeathAnimationComplete()
    {
        if (livesRemaining <= 0)
        {
            die(); // Nếu hết mạng thì chết hẳn
        }
        currentHealth = maxHealth; // Hồi lại máu tối đa
        respawn(); // Hồi sinh nhân vật

        specialAttack.enabled = true;
        attack.enabled = true;
        movement.enabled = true;

        // Khôi phục lại ràng buộc ban đầu của Rigidbody
        rigidBody2D.constraints = originalConstraints;

        // Điều chỉnh vị trí một chút để tránh lỗi va chạm
        rigidBody2D.position = new Vector2(rigidBody2D.position.x, rigidBody2D.position.y + 0.01f);
        updateUI(); // Cập nhật lại giao diện
    }

    // Xử lý hồi sinh nhân vật
    private void respawn()
    {
        startRigidbody2D.simulated = false; // Vô hiệu hóa Rigidbody tạm thời
        transform.localScale = Vector3.zero; // Làm nhân vật vô hình
        transform.position = startPosition; // Đưa nhân vật về vị trí ban đầu

        // Khôi phục hướng nhân vật dựa trên thông tin cũ
        if (userConfiguration == null)
        {
            return;
        }
        userConfiguration.setFacingRight(userConfiguration.getFacingRight());
        transform.localScale = originalLocalScale; // Đặt lại kích thước ban đầu
        startRigidbody2D.simulated = true; // Kích hoạt lại Rigidbody
    }

    private void die()
    {
        Debug.Log("Game Over");
        GameManager.gameManagerInstance.enableGameOverPanel(gameObject.tag); // Hiển thị màn hình Game Over
        Destroy(gameObject); // Hủy đối tượng nhân vật
    }

}
