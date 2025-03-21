using System;
using System.Collections;
using UnityEngine;

public class KitsuneMovement : MonoBehaviour
{
    // Thuộc tính có thể điều chỉnh cho mỗi nhân vật
    public float speed; // Khác nhau cho mỗi nhân vật mới
    public float jumpForce; // Khác nhau cho mỗi nhân vật mới

    // Thuộc tính có thể điều chỉnh dựa trên người chơi 1 hoặc người chơi 2
    //public KeyCode jumpKey; // Gán phím cho mỗi người chơi
    //public KeyCode downKey; // Phím để đi qua nền tảng
    //public bool facingRight; // Hướng ban đầu dựa trên người chơi
    //public string axis; // Trục ngang của người chơi

    // Thuộc tính chung cho tất cả các nhân vật
    public LayerMask groundLayer; // Lớp mặt đất
    public Transform groundCheck; // Điểm kiểm tra mặt đất
    public float groundCheckRadius; // Bán kính kiểm tra mặt đất
    private bool isGrounded; // Kiểm tra xem có đang đứng trên mặt đất không

    private Rigidbody2D rb; // Thành phần Rigidbody2D
    private SpriteRenderer spriteRenderer; // Thành phần SpriteRenderer
    public Transform weaponHitBox; // Vùng tấn công của vũ khí

    // Thuộc tính cho âm thanh
    [SerializeField] private AudioClip soundJump; // Âm thanh nhảy

    // Thuộc tính cho nền tảng
    private GameObject currentOneWayPlatform; // Nền tảng một chiều hiện tại
    private int fighterLayer; // Lớp của nhân vật
    private CapsuleCollider2D playerCollider; // Collider của người chơi

    private UserConfiguration userConfiguration; // Cấu hình người dùng
    private Animator animator; // Thành phần Animator
    private KitsuneSpecialAttack KitsuneSpecialAttack; // Thành phần tấn công đặc biệt

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
        HandleMovement(); // Xử lý di chuyển
        HandleJump(); // Xử lý nhảy
        HandlePlatformDrop(); // Xử lý rơi qua nền tảng
        animator.SetBool("isJumping", !isGrounded);

        // Kiểm tra nếu phím tấn công đặc biệt được nhấn
        if (Input.GetKeyDown(userConfiguration.getSpecialPowerKey()))
        {
            // Kích hoạt tấn công đặc biệt
            KitsuneSpecialAttack.useSpecialAttack(); // Tham chiếu đến tấn công đặc biệt
        }
    }

    // Xử lý di chuyển
    private void HandleMovement()
    {
        // Di chuyển ngang
        float moveX = Input.GetAxis(userConfiguration.getAxis());
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);
        animator.SetFloat("speed", Mathf.Abs(moveX * speed));

        // Thay đổi hướng của sprite dựa trên `facingRight`
        if (moveX > 0 && !userConfiguration.getFacingRight())
        {
            Flip();
        }
        else if (moveX < 0 && userConfiguration.getFacingRight())
        {
            Flip();
        }
    }

    // Xử lý nhảy
    private void HandleJump()
    {
        // Phát hiện nếu đang ở trên mặt đất
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && Input.GetKeyDown(userConfiguration.getJumpKey()))
        {
            animator.SetBool("isJumping", true);
            Debug.Log("Nhảy");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            SoundsController.Instance.RunSound(soundJump);
        }
    }

    // Xử lý rơi qua nền tảng
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

    // Lật hướng nhân vật
    private void Flip()
    {
        userConfiguration.setFacingRight(!userConfiguration.getFacingRight());

        // Thay đổi hướng của sprite
        spriteRenderer.flipX = !spriteRenderer.flipX;

        // Đảo ngược vị trí X của weaponHitBox
        if (weaponHitBox != null)
        {
            Vector3 localPosition = weaponHitBox.localPosition;
            localPosition.x *= -1;
            weaponHitBox.localPosition = localPosition;

            Vector3 localPositionGroundChekc = groundCheck.localPosition;
            localPositionGroundChekc.x *= -1;
            groundCheck.localPosition = localPositionGroundChekc;
        }

        // Đảo ngược CapsuleCollider2D
        if (playerCollider != null)
        {
            Vector2 offset = playerCollider.offset;
            offset.x *= -1;
            playerCollider.offset = offset;
        }
    }

    // Xử lý khi va chạm với đối tượng khác
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    // Xử lý khi kết thúc va chạm với đối tượng khác
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    // Vô hiệu hóa va chạm với nền tảng tạm thời
    private IEnumerator DisablePlatformCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }

    // Kiểm tra và xác thực các tham chiếu
    private void OnValidate()
    {
        groundCheck = transform.Find("GroundCheck");
        weaponHitBox = transform.Find("WeaponHitBox");
        playerCollider = GetComponent<CapsuleCollider2D>();
        fighterLayer = LayerMask.NameToLayer("BaseFighter");
    }

    // Vẽ gizmos để hiển thị trong trình chỉnh sửa
    private void OnDrawGizmosSelected()
    {
        // Hiển thị khu vực phát hiện mặt đất trong trình chỉnh sửa
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    // Khởi tạo hướng ban đầu của nhân vật
    public void InitializeFacingDirection()
    {
        // Điều chỉnh sprite và các thành phần theo giá trị ban đầu của facingRight
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

    // Lấy cấu hình người dùng
    public UserConfiguration getUserConfiguration()
    {
        return userConfiguration;
    }

    // Lấy thành phần Animator
    public Animator getAnimator()
    {
        return animator;
    }
}
