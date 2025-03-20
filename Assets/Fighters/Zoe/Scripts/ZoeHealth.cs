using System;
using System.Collections;
using UnityEngine;

public class ZoeHealth : MonoBehaviour, Damageable
{
    public Animator animator; // Tham chiếu đến Animator để phát hoạt ảnh tấn công
    public float currentHealth; // Máu hiện tại
    public float maxHealth; // Máu tối đa
    public int livesRemaining; // Số mạng còn lại

    private Vector2 startPosition; // Vị trí bắt đầu
    private Rigidbody2D startRigidbody2D;
    private Vector3 originalLocalScale; // Tỷ lệ gốc của nhân vật
    private ZoeMovement movement;
    private ZoeAttack attack;
    private ZoeShield shield;
    private ZoeSpecialAttack specialAttack;
    private Rigidbody2D rigidBody2D;
    private RigidbodyConstraints2D originalConstraints;
    private UserConfiguration userConfiguration;
    private UIController UIController;

    [SerializeField] private AudioClip soundHurt; // Âm thanh khi bị tấn công

    private void Start()
    {
        animator = GetComponent<Animator>();
        UIController = GetComponent<UIController>();
        livesRemaining = UIController.getNumberOfLives(); // Lấy số mạng từ UI

        currentHealth = maxHealth; // Bắt đầu với lượng máu tối đa

        startPosition = transform.position; // Lưu vị trí ban đầu của nhân vật
        startRigidbody2D = GetComponent<Rigidbody2D>();
        originalLocalScale = transform.localScale; // Lưu tỷ lệ gốc

        movement = GetComponent<ZoeMovement>();
        attack = GetComponent<ZoeAttack>();
        shield = GetComponent<ZoeShield>();
        specialAttack = GetComponent<ZoeSpecialAttack>();
        userConfiguration = GetComponent<UserConfiguration>();

        rigidBody2D = GetComponent<Rigidbody2D>();

        // Lưu trạng thái ràng buộc ban đầu của Rigidbody
        originalConstraints = rigidBody2D.constraints;
    }

    void updateUI()
    {
        UIController.updateHealthBar(currentHealth, maxHealth); // Cập nhật thanh máu
        UIController.updateLives(livesRemaining); // Cập nhật số mạng còn lại
    }

    public void decreaseLife(float damage)
    {
        currentHealth -= damage;
        SoundsController.Instance.RunSound(soundHurt);
        animator.SetTrigger("hurt"); // Kích hoạt hoạt ảnh bị tấn công

        if (currentHealth <= 0)
        {
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

            specialAttack.enabled = false;
            attack.enabled = false;
            movement.enabled = false;

            livesRemaining--; // Giảm số mạng

            if (livesRemaining > 0)
            {
                currentHealth = maxHealth; // Nếu còn mạng, hồi đầy máu
            }
            animator.SetTrigger("die"); // Kích hoạt hoạt ảnh chết

            StartCoroutine(WaitForDeathAnimation());

        } 
        updateUI(); // Cập nhật UI
    }

    // Hàm này sẽ được gọi khi hoạt ảnh chết hoàn tất
    public void OnDeathAnimationComplete()
    {
        if (livesRemaining <= 0)
        {
            die(); // Hết mạng => chết hoàn toàn
        }
        else
        {
            currentHealth = maxHealth;
            respawn(); 
            //currentHealth = maxHealth;
            specialAttack.enabled = true;
            attack.enabled = true;
            movement.enabled = true;
            // Khôi phục ràng buộc ban đầu
            rigidBody2D.constraints = originalConstraints;

            // Điều chỉnh nhẹ vị trí để buộc hệ thống va chạm cập nhật
            rigidBody2D.position = new Vector2(rigidBody2D.position.x, rigidBody2D.position.y + 0.01f);
            animator.SetBool("isDead", false);
        }
    }


    private void respawn()
    {

        startRigidbody2D.simulated = false; // Tắt vật lý của nhân vật

        // Làm nhân vật vô hình tạm thời (bằng scale)
        transform.localScale = Vector3.zero;

        // Đặt lại vị trí ban đầu
        transform.position = startPosition;

        // Khôi phục hướng di chuyển dựa vào `facingRight`
        if (userConfiguration != null)
        {
            userConfiguration.setFacingRight(userConfiguration.getFacingRight());
        }
        transform.localScale = originalLocalScale;  // Khôi phục kích thước gốc
        startRigidbody2D.simulated = true;  // Bật lại vật lý của nhân vật
    }

    private void die()
    {
        Debug.Log("Player " + gameObject.layer.ToString());
        GameManager.gameManagerInstance.enableGameOverPanel(gameObject.tag); // Hiển thị bảng Game Over
        Destroy(gameObject); // Xóa nhân vật
    }

    private IEnumerator WaitForDeathAnimation()
    {
        // Chờ cho đến khi hoạt ảnh chết được kích hoạt
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            yield return null; // Chờ một frame
        }

        // Lấy thời gian của hoạt ảnh hiện tại
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length - 0.3f;

        // Chờ thời gian bằng thời lượng của hoạt ảnh
        yield return new WaitForSeconds(animationDuration);

        // Gọi hàm hoàn tất sau khi hoạt ảnh kết thúc
        OnDeathAnimationComplete();
    }
}