using System;
using System.Collections;
using UnityEngine;

public class ZoeHealth : MonoBehaviour, Damageable
{
    public Animator animator; // Tham chiếu đến Animator để phát hoạt ảnh tấn công
    public float currentHealth;
    public float maxHealth;
    public int livesRemaining;

    private Vector2 startPosition;
    private Rigidbody2D startRigidbody2D;
    private Vector3 originalLocalScale;
    private ZoeMovement movement;
    private ZoeAttack attack;
    private ZoeShield shield;
    private ZoeSpecialAttack specialAttack;
    private Rigidbody2D rigidBody2D;
    private RigidbodyConstraints2D originalConstraints;
    private UserConfiguration userConfiguration;
    private UIController UIController;

    [SerializeField] private AudioClip soundHurt;

    private void Start()
    {
        animator = GetComponent<Animator>();
        UIController = GetComponent<UIController>();
        livesRemaining = UIController.getNumberOfLives();

        currentHealth = maxHealth;

        startPosition = transform.position;
        startRigidbody2D = GetComponent<Rigidbody2D>();
        originalLocalScale = transform.localScale;

        movement = GetComponent<ZoeMovement>();
        attack = GetComponent<ZoeAttack>();
        shield = GetComponent<ZoeShield>();
        specialAttack = GetComponent<ZoeSpecialAttack>();
        userConfiguration = GetComponent<UserConfiguration>();

        rigidBody2D = GetComponent<Rigidbody2D>();

        // Lưu lại các ràng buộc ban đầu của Rigidbody
        originalConstraints = rigidBody2D.constraints;
    }

    void updateUI()
    {
        UIController.updateHealthBar(currentHealth, maxHealth);
        UIController.updateLives(livesRemaining);
    }

    public void decreaseLife(float damage)
    {
        currentHealth -= damage;
        SoundsController.Instance.RunSound(soundHurt);
        animator.SetTrigger("hurt");

        if (currentHealth <= 0)
        {
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

            specialAttack.enabled = false;
            attack.enabled = false;
            movement.enabled = false;

            livesRemaining--;

            if (livesRemaining > 0)
            {
                currentHealth = maxHealth;
            }
            //animator.SetBool("isDead", true);
            animator.SetTrigger("die");

            StartCoroutine(WaitForDeathAnimation());
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
            specialAttack.enabled = true;
            attack.enabled = true;
            movement.enabled = true;
            // Khôi phục lại các ràng buộc ban đầu
            rigidBody2D.constraints = originalConstraints;

            // Điều chỉnh vị trí để đảm bảo va chạm được cập nhật
            rigidBody2D.position = new Vector2(rigidBody2D.position.x, rigidBody2D.position.y + 0.01f);
            animator.SetBool("isDead", false);
        }
    }


    private void respawn()
    {

        startRigidbody2D.simulated = false;

        // Làm cho nhân vật tạm thời vô hình (bằng cách chỉnh scale)
        transform.localScale = Vector3.zero;

        // Đặt lại vị trí ban đầu của nhân vật
        transform.position = startPosition;

        // Khôi phục hướng di chuyển dựa trên `facingRight`
        if (userConfiguration != null)
        {
            userConfiguration.setFacingRight(userConfiguration.getFacingRight());
        }
        transform.localScale = originalLocalScale;
        startRigidbody2D.simulated = true;
    }

    private void die()
    {
        Debug.Log("Player " + gameObject.layer.ToString());
        GameManager.gameManagerInstance.enableGameOverPanel(gameObject.tag);
        Destroy(gameObject);
    }

    private IEnumerator WaitForDeathAnimation()
    {
        // Chờ đến khi hoạt ảnh chết đang chạy
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            yield return null; // Chờ một frame
        }

        // Lấy thời gian chạy của hoạt ảnh hiện tại
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length - 0.3f;

        // Chờ thời gian tương ứng với hoạt ảnh
        yield return new WaitForSeconds(animationDuration);

        // Gọi OnDeathAnimationComplete sau khi hoạt ảnh kết thúc
        OnDeathAnimationComplete();
    }
   
}