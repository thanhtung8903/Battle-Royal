using Unity.VisualScripting;
using UnityEngine;

public class KitsuneSpecialAttack : MonoBehaviour
{
    public float specialCharge = 0f; // Năng lượng tấn công đặc biệt hiện tại
    public float maxCharge; // Năng lượng tấn công đặc biệt tối đa

    private bool isReady = false; // Trạng thái sẵn sàng của tấn công đặc biệt
    private UIController UIController; // Điều khiển giao diện người dùng

    // Thêm tham chiếu đến Animator
    public Animator animator; // Thành phần Animator

    // Prefab cho hiệu ứng hình ảnh của tấn công đặc biệt
    public GameObject specialAttackEffect; // Hiệu ứng tấn công đặc biệt

    [SerializeField] private GameObject FireBall; // Đối tượng cầu lửa

    private void Start()
    {
        UIController = GetComponent<UIController>();
        updateUI();

        // Đảm bảo gán Animator
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    // Tăng năng lượng tấn công đặc biệt
    public void increaseCharge(float amount)
    {
        if (!isReady)
        {
            specialCharge += amount;
            specialCharge = Mathf.Clamp(specialCharge, 0, maxCharge);

            if (specialCharge >= maxCharge)
            {
                isReady = true;
                Debug.Log("Tấn công đặc biệt đã sẵn sàng!");
            }

            updateUI();
        }
    }

    // Sử dụng tấn công đặc biệt
    public void useSpecialAttack()
    {
        if (isReady)
        {
            Debug.Log("Tấn công đặc biệt đã kích hoạt!");
            performSpecialAttack();
            specialCharge = 0f;
            isReady = false;
            updateUI();
        }
    }

    // Thực hiện tấn công đặc biệt
    private void performSpecialAttack()
    {
        // Kích hoạt hoạt ảnh tấn công đặc biệt
        if (animator != null)
        {
            animator.SetTrigger("specialAttack");
        }

        //Vector3 startPosition = new Vector3(transform.position.x + 4, );

        FireBall.GetComponent<FireBallAttack>().setUserTag(gameObject.tag);

        FireBall.GetComponent<FireBallMovement>().setUserConfiguration(gameObject.GetComponent<UserConfiguration>());

        Instantiate(FireBall, transform.position, Quaternion.identity);
        /*
        // Tạo hiệu ứng đặc biệt nếu tồn tại prefab
        if (specialAttackEffect != null)
        {
            Instantiate(specialAttackEffect, transform.position, Quaternion.identity);
        }
        */

        Debug.Log("Đang thực hiện tấn công đặc biệt!");
    }

    // Cập nhật giao diện người dùng
    private void updateUI()
    {
        UIController.updateSpecialBar(specialCharge, maxCharge);
    }

    // Thiết lập năng lượng tấn công đặc biệt tối đa
    public void setMaxCharge(float maxChargeFromPersonaje)
    {
        this.maxCharge = maxChargeFromPersonaje;
    }
}
