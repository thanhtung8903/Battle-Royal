using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class FighterSelectionMenuLogic : MonoBehaviour
{
    // 1. KHAI BÁO CÁC THUỘC TÍNH

    // Tham chiếu đến GameManager
    [Header("GameManager")]
    private GameManager gameManager;

    [Header("Tài nguyên Menu chọn nhân vật cho người dùng")]
    private int indexUser1; // Chỉ số của nhân vật được chọn bởi người dùng 1
    [SerializeField] private Image user1Image; // Hình ảnh của nhân vật được chọn bởi người dùng 1
    [SerializeField] private TextMeshProUGUI user1Name; // Tên của nhân vật được chọn bởi người dùng 1

    [Header("Tài nguyên Menu chọn nhân vật cho người dùng")]
    private int indexUser2; // Chỉ số của nhân vật được chọn bởi người dùng 2
    [SerializeField] private Image user2Image; // Hình ảnh của nhân vật được chọn bởi người dùng 2
    [SerializeField] private TextMeshProUGUI user2Name; // Tên của nhân vật được chọn bởi người dùng 2


    // ------------------------------------------------------------------------------------------------------------------------------------------
    // 2. CÁC PHƯƠNG THỨC
    private void Start()
    {
        gameManager = GameManager.gameManagerInstance; // Lấy instance của GameManager


        indexUser1 = PlayerPrefs.GetInt("User1Index"); // Lấy chỉ số của nhân vật đã chọn bởi người dùng 1
        indexUser2 = PlayerPrefs.GetInt("User2Index"); // Lấy chỉ số của nhân vật đã chọn bởi người dùng 2

        // Kiểm tra xem các chỉ số có vượt quá số lượng fightersData có sẵn hay không
        if (indexUser1 > gameManager.fightersData.Count - 1)
        {
            indexUser1 = 0; // Gán nhân vật đầu tiên nếu chỉ số vượt quá
        }

        // Kiểm tra xem các chỉ số có vượt quá số lượng fightersData có sẵn hay không
        if (indexUser2 > gameManager.fightersData.Count - 1)
        {
            indexUser2 = 0; // Gán nhân vật đầu tiên nếu chỉ số vượt quá
        }

        // Gán dữ liệu fightersData đã chọn cho người dùng
        updateUser1SelectionScreen();
        updateUser2SelectionScreen();
    }

    // ------------------------------------------------------------------------------------------------------------------------------------------
    // Cập nhật giao diện chọn nhân vật của người chơi 1 với thông tin của nhân vật đã chọn.
    private void updateUser1SelectionScreen()
    {
        PlayerPrefs.SetInt("User1Index", indexUser1);


        user1Image.sprite = gameManager.fightersData[indexUser1].getFighterImage();
        user1Name.text = gameManager.fightersData[indexUser1].getFighterName();
    }

    // Cập nhật giao diện chọn nhân vật của người chơi 2 với thông tin của nhân vật đã chọn.
    private void updateUser2SelectionScreen()
    {
        PlayerPrefs.SetInt("User2Index", indexUser2);

        user2Image.sprite = gameManager.fightersData[indexUser2].getFighterImage();
        user2Name.text = gameManager.fightersData[indexUser2].getFighterName();
    }

    // ------------------------------------------------------------------------------------------------------------------------------------------
    private int advanceToTheNextFighterUser(int indexUser)
    {
        if (indexUser == gameManager.fightersData.Count - 1)
        {
            indexUser = 0; // Nếu đã ở nhân vật cuối cùng thì quay lại nhân vật đầu tiên
        }
        else
        {
            indexUser += 1; // Chuyển sang nhân vật tiếp theo
        }
        return indexUser;
    }

    public void advanceToTheNextFighterUser1()
    {
        indexUser1 = advanceToTheNextFighterUser(indexUser1);
        updateUser1SelectionScreen();
    }

    public void advanceToTheNextFighterUser2()
    {
        indexUser2 = advanceToTheNextFighterUser(indexUser2);
        updateUser2SelectionScreen();
    }


    // ------------------------------------------------------------------------------------------------------------------------------------------
    private int goBackToThePreviousFighterUser(int indexUser)
    {
        if (indexUser == 0)
        {
            indexUser = gameManager.fightersData.Count - 1; // Nếu đang ở nhân vật đầu tiên thì quay lại nhân vật cuối cùng
        }
        else
        {
            indexUser -= 1; // Quay lại nhân vật trước đó
        }
        return indexUser;
    }

    public void goBackToThePreviousFighterUser1()
    {
        indexUser1 = goBackToThePreviousFighterUser(indexUser1);
        updateUser1SelectionScreen();
    }

    public void goBackToThePreviousFighterUser2()
    {
        indexUser2 = goBackToThePreviousFighterUser(indexUser2);
        updateUser2SelectionScreen();
    }


    // ------------------------------------------------------------------------------------------------------------------------------------------
    public void startGame()
    {
        // Chuyển đến scene tiếp theo
        SceneManager.LoadScene("ScenarySelectionMenu");
    }

    public void BackButton(string sceneName)
    {
        // Quay lại scene được chỉ định
        SceneManager.LoadScene(sceneName);
    }
}