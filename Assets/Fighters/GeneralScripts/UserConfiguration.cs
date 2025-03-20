using UnityEngine;

public class UserConfiguration : MonoBehaviour
{
    //[SerializeField] private int userIndex; // Chỉ số của người chơi (1 hoặc 2)
    //[SerializeField] private Transform spawnPosition; // Vị trí xuất hiện của đấu sĩ
    //[SerializeField] private string uiPrefix; // Tiền tố UI ("Fighter1UI" hoặc "Fighter2UI")
    [SerializeField] private KeyCode[] movementKeys; // Các phím điều khiển di chuyển
    [SerializeField] private KeyCode[] attackKeys; // Các phím tấn công
    [SerializeField] private KeyCode shieldKey; // Phím kích hoạt khiên
    [SerializeField] private bool facingRight; // Xác định hướng mặc định của nhân vật
    [SerializeField] private string axis; // Trục điều khiển (dùng cho joystick hoặc phím di chuyển)

    //[SerializeField] private UIInitializer UIInitializer;

    public void setMovementKeys(KeyCode[] movementKeys)
    {
        this.movementKeys = movementKeys;
    }

    public void setAttackKeys(KeyCode[] attackKeys) { this.attackKeys = attackKeys; }

    public void setShieldKey(KeyCode shieldKey)
    {
        this.shieldKey = shieldKey;
    }

    public void setFacingRight(bool facingRight)
    {
        this.facingRight = facingRight;
    }

    public void setAxis(string axis)
    {
        this.axis = axis;
    }

    public KeyCode getJumpKey()
    {
        return movementKeys[0];  // Trả về phím dùng để nhảy
    }

    public KeyCode getDownKey()
    {
        return movementKeys[1]; // Trả về phím dùng để cúi xuống
    }

    public KeyCode getHitKey()
    {
        return attackKeys[0];  // Trả về phím thực hiện đòn đánh thường
    }

    public KeyCode getKickKey()
    {
        return attackKeys[1]; // Trả về phím thực hiện đòn đá
    }

    public KeyCode getSpecialPowerKey()
    {
        return attackKeys[2]; // Trả về phím thực hiện kỹ năng đặc biệt
    }

    public KeyCode getShieldKey()
    {
        return shieldKey; // Trả về phím kích hoạt khiên
    }

    public bool getFacingRight()
    {
        return facingRight;  // Trả về hướng mặc định của nhân vật
    }

    public string getAxis()
    {
        return axis; // Trả về trục điều khiển
    }
}
