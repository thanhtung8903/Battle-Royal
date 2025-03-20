using UnityEngine;

public class UserInitializer : MonoBehaviour
{
    [SerializeField] private int userIndex; // Chỉ số của người chơi (1 hoặc 2)
    [SerializeField] private Transform spawnPosition; // Vị trí xuất hiện của đấu sĩ
    [SerializeField] private string uiPrefix; // Tiền tố UI ("Fighter1UI" hoặc "Fighter2UI")
    [SerializeField] private KeyCode[] movementKeys;  // Các phím điều khiển di chuyển
    [SerializeField] private KeyCode[] attackKeys; // Các phím tấn công
    [SerializeField] private KeyCode shieldKey; // Phím kích hoạt khiên
    [SerializeField] private UIInitializer UIInitializer;

    //[SerializeField] private UserConfiguration userConfiguration;

    private void Start()
    {
        int fighterIndex = PlayerPrefs.GetInt($"User{userIndex}Index"); // Lấy chỉ số của đấu sĩ được người chơi chọn

        FightersData fighterData = GameManager.gameManagerInstance.fightersData[fighterIndex]; // Lấy dữ liệu của đấu sĩ đã chọn
        GameObject fighter = fighterData.getFighterPrefab(); // Lấy prefab của đấu sĩ đã chọn
        fighter.tag = userIndex == 1 ? "User1" : "User2"; // Gán tag phù hợp cho đấu sĩ (User1 hoặc User2)
        UserConfiguration userConfiguration = fighter.GetComponent<UserConfiguration>();
        userConfiguration.setMovementKeys(movementKeys);
        userConfiguration.setAttackKeys(attackKeys);
        userConfiguration.setShieldKey(shieldKey);
        userConfiguration.setFacingRight(userIndex == 1);
        userConfiguration.setAxis(userIndex == 1 ? "Horizontal2" : "Horizontal");

        // Cấu hình các thành phần của đấu sĩ
        configureMovement(fighter, fighterData);
        configureAttack(fighter, fighterData);
        configureAttributes(fighter, fighterData);

        // Cấu hình UI cho đấu sĩ
        UIController UIController = fighter.GetComponent<UIController>();
        UIInitializer = GetComponent<UIInitializer>();
        UIInitializer.configureUI(UIController, userIndex, uiPrefix);

        // Khởi tạo đấu sĩ trong trò chơi
        Instantiate(fighter, spawnPosition.position, Quaternion.identity);
    }

    private void configureMovement(GameObject fighter, FightersData fighterData)
    {
        //Movement movement = fighter.GetComponent<Movement>();
        //movement.setFacingRight(userIndex == 1);
        //movement.setAxis(userIndex == 1 ? "Horizontal2" : "Horizontal"); // Gán trục di chuyển ngang
        //movement.setUpKey(movementKeys[0]);  // Gán phím nhảy
        //movement.setDownKey(movementKeys[1]); // Gán phím cúi xuống
        //movement.setSpeed(fighterData.getSpeed()); // Gán tốc độ di chuyển
        //movement.setJumpForce(fighterData.getJumpForce()); // Gán lực nhảy
        //movement.setGroundCheckRadius(fighterData.getGroundCheckRadius()); // Gán bán kính kiểm tra mặt đất
    }

    private void configureAttack(GameObject fighter, FightersData fighterData)
    {
        //Attack attack = fighter.GetComponent<Attack>();
        //attack.setHitKey(attackKeys[0]); // Gán phím đánh thường
        //attack.setKickKey(attackKeys[1]); // Gán phím đá
        //attack.setSpecialPowerKey(attackKeys[2]); // Gán phím kỹ năng đặc biệt

        //attack.setAttackRate(fighterData.getAttackRate()); // Gán tốc độ ra đòn
        //attack.setAttackRange(fighterData.getAttackRange()); // Gán phạm vi tấn công

        //attack.setHitDamage(fighterData.getHitDamage()); // Gán sát thương đòn đánh thường
        //attack.setKickDamage(fighterData.getKickDamage()); // Gán sát thương đòn đá
        //attack.setSpecialPowerDamage(fighterData.getSpecialPowerDamage()); // Gán sát thương đòn đặc biệt

        //attack.setHitDamageToShield(fighterData.getHitDamageToShield()); // Gán sát thương đòn đánh thường lên khiên
        //attack.setKickDamageToShield(fighterData.getKickDamageToShield()); // Gán sát thương đòn đá lên khiên

        //attack.setWaitingTimeHit(fighterData.getWaitingTimeHit()); // Gán thời gian chờ sau đòn đánh thường
        //attack.setWaitingTimeKick(fighterData.getWaitingTimeKick()); // Gán thời gian chờ sau đòn đá

        //fighter.GetComponent<Shield>().setShieldKey(shieldKey); // Gán phím kích hoạt khiên
        //fighter.GetComponent<Shield>().setShieldDuration(fighterData.getShieldDuration()); // Gán thời gian tồn tại của khiên
        //fighter.GetComponent<Shield>().setMaxShieldCapacity(fighterData.getMaxShieldCapacity()); // Gán dung lượng tối đa của khiên
        //fighter.GetComponent<Shield>().setRechargeRate(fighterData.getRechargeRate()); // Gán tốc độ hồi khiên
    }


    private void configureAttributes(GameObject fighter, FightersData fighterData)
    {
        //Health health = fighter.GetComponent<Health>();
        //health.setMaxHealth(fighterData.getMaxHealth()); // Gán lượng máu tối đa

        //SpecialAttack special = fighter.GetComponent<SpecialAttack>();
        //special.setMaxCharge(fighterData.getMaxCharge()); // Gán mức năng lượng tối đa cho đòn đặc biệt
    }
}
