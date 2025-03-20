using System;
using System.Collections;
using UnityEngine;

public class KitsuneMovement : MonoBehaviour
{
    // Thuộc tính có thể thay đổi cho từng đấu sĩ
    public float speed; // Khác nhau cho từng đấu sĩ
    public float jumpForce; // Khác nhau cho từng đấu sĩ

    // Atributos modificables en base al player 1 o player 2
    //public KeyCode jumpKey; // Asignar teclas a cada jugador
    //public KeyCode downKey; // Tecla para pasar plataformas
    //public bool facingRight; // Orientaci�n inicial basada en el jugador
    //public string axis; // Eje horizontal del jugador

    // Thuộc tính chung cho tất cả các đấu sĩ
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius;
    private bool isGrounded;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public Transform weaponHitBox;

    // Thuộc tính cho âm thanh
    [SerializeField] private AudioClip soundJump;

    // Thuộc tính cho nền tảng
    private GameObject currentOneWayPlatform;
    private int fighterLayer;
    private CapsuleCollider2D playerCollider;

    private UserConfiguration userConfiguration;
    private Animator animator;
    private KitsuneSpecialAttack KitsuneSpecialAttack;

    void Start()
    {
        groundLayer = LayerMask.GetMask("Ground");
        fighterLayer = LayerMask.NameToLayer("BaseFighter");

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<CapsuleCollider2D>();

        userConfiguration = GetComponent<UserConfiguration>();
        animator = GetComponent<Animator>();

        KitsuneSpecialAttack = GetComponent<KitsuneSpecialAttack>();

        InitializeFacingDirection();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandlePlatformDrop();
        animator.SetBool("isJumping", !isGrounded);

        // Kiểm tra nếu nhấn phím tấn công đặc biệt
        if (Input.GetKeyDown(userConfiguration.getSpecialPowerKey()))
        {
            // Kích hoạt tấn công đặc biệt
            KitsuneSpecialAttack.useSpecialAttack(); // Tham chiếu đến tấn công đặc biệt
        }
    }

    private void HandleMovement()
    {
        // Di chuyển theo trục ngang
        float moveX = Input.GetAxis(userConfiguration.getAxis());
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);
        animator.SetFloat("speed", Mathf.Abs(moveX * speed));

        // Thay đổi hướng sprite dựa vào `facingRight`
        if (moveX > 0 && !userConfiguration.getFacingRight())
        {
            Flip();
        }
        else if (moveX < 0 && userConfiguration.getFacingRight())
        {
            Flip();
        }
    }


    private void HandleJump()
    {
        // Kiểm tra nếu đang đứng trên mặt đất
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);


        if (isGrounded && Input.GetKeyDown(userConfiguration.getJumpKey()))
        {

            animator.SetBool("isJumping", true);
            Debug.Log("Jump");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            SoundsController.Instance.RunSound(soundJump);

        }
    }

    private void HandlePlatformDrop()
    {
        if (Input.GetKeyDown(userConfiguration.getDownKey()) && gameObject.layer == fighterLayer)
        {
            if (currentOneWayPlatform != null)
            {
                StartCoroutine(DisablePlatformCollision());
            }
        }
    }

    private void Flip()
    {
        userConfiguration.setFacingRight(!userConfiguration.getFacingRight());

        // Đảo hướng sprite
        spriteRenderer.flipX = !spriteRenderer.flipX;

        // Đảo vị trí X của weaponHitBox
        if (weaponHitBox != null)
        {
            Vector3 localPosition = weaponHitBox.localPosition;
            localPosition.x *= -1;
            weaponHitBox.localPosition = localPosition;

            Vector3 localPositionGroundChekc = groundCheck.localPosition;
            localPositionGroundChekc.x *= -1;
            groundCheck.localPosition = localPositionGroundChekc;
        }

        // Đảo CapsuleCollider2D
        if (playerCollider != null)
        {
            Vector2 offset = playerCollider.offset;
            offset.x *= -1;
            playerCollider.offset = offset;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisablePlatformCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }

    private void OnValidate()
    {
        groundCheck = transform.Find("GroundCheck");
        weaponHitBox = transform.Find("WeaponHitBox");
        playerCollider = GetComponent<CapsuleCollider2D>();
        fighterLayer = LayerMask.NameToLayer("BaseFighter");
    }

    private void OnDrawGizmosSelected()
    {
        // Hiển thị khu vực kiểm tra mặt đất trong trình chỉnh sửa
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public void InitializeFacingDirection()
    {
        // Điều chỉnh sprite và các thành phần dựa vào hướng ban đầu
        if (!userConfiguration.getFacingRight())
        {
            spriteRenderer.flipX = true;

            if (weaponHitBox != null)
            {
                Vector3 localPosition = weaponHitBox.localPosition;
                localPosition.x *= -1;
                weaponHitBox.localPosition = localPosition;
            }

            if (playerCollider != null)
            {
                Vector2 offset = playerCollider.offset;
                offset.x *= -1;
                playerCollider.offset = offset;
            }
        }
    }

    public UserConfiguration getUserConfiguration()
    {
        return userConfiguration;
    }

    public Animator getAnimator()
    {
        return animator;
    }
}
