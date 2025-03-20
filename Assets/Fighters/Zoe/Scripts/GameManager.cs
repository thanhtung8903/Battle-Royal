using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mono.Data.Sqlite;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;

    [SerializeField] public List<FightersData> fightersData; // Danh sách dữ liệu của các nhân vật
    [SerializeField] private GameObject gameOverUI; // Giao diện hiển thị khi trò chơi kết thúc
    [SerializeField] private AudioClip knockoutVoice; // Âm thanh khi có knockout
    [SerializeField] private GameObject puaseUI; // Giao diện tạm dừng
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape; // Phím tạm dừng 

    [SerializeField] private AudioClip fighterSelectionAudio; // Âm thanh trong menu chọn nhân vật

    private void Awake()
    {
        if (GameManager.gameManagerInstance == null)
        {
            GameManager.gameManagerInstance = this; // Gán instance của GameManager
            DontDestroyOnLoad(this.gameObject); // Giữ đối tượng không bị hủy khi chuyển scene
        }
        else
        {
            Destroy(gameObject); // Hủy đối tượng nếu đã có instance khác
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "FighterSelectionMenu")
        {
            PlayerPrefs.SetString("User1", ""); // Xóa dữ liệu User1 trong PlayerPrefs
            PlayerPrefs.Save(); // Lưu thay đổi
            PlayerPrefs.SetString("User2", ""); // Xóa dữ liệu User2 trong PlayerPrefs
            PlayerPrefs.Save(); // Lưu thay đổi
            SoundsController.Instance.RunSound(fighterSelectionAudio); // Phát âm thanh menu chọn nhân vật
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(pauseKey) && puaseUI != null && !gameOverUI.activeSelf)
        {
            puaseUI.SetActive(!puaseUI.activeSelf); // Chuyển trạng thái hiển thị của giao diện tạm dừng
            Time.timeScale = puaseUI.activeSelf ? 0 : 1; // Tạm dừng hoặc tiếp tục thời gian trò chơi

            if (puaseUI.activeSelf)
            {
                SoundsController.Instance.pauseSound(); // Tạm dừng tất cả âm thanh
            }
            else
            {
                SoundsController.Instance.reactiveSound(); // Kích hoạt lại tất cả âm thanh
            }
        }
    }

    public void enableGameOverPanel(string looserUserTag)
    {
        string winnerUserTag = looserUserTag == "User1" ? "User2" : "User1"; // Xác định người thắng dựa trên người thua
        GameObject winner = GameObject.FindGameObjectWithTag(winnerUserTag); // Tìm đối tượng người thắng
        //gameOverUI.setSpriteRenderer(winner.GetComponent<SpriteRenderer>());
        Image winnerImage = winner.GetComponent<Image>(); // Lấy hình ảnh của người thắng
        Transform childTransform = gameOverUI.transform.Find("WinnerImage"); // Tìm đối tượng con trong gameOverUI
        childTransform.GetComponent<Image>().sprite = winnerImage.sprite; // Gán hình ảnh người thắng

        gameOverUI.SetActive(true); // Hiển thị giao diện kết thúc
        Time.timeScale = 0; // Tạm dừng thời gian
        //SoundsController.Instance.PauseAllSounds(); // Tạm dừng tất cả âm thanh
        SoundsController.Instance.RunSound(knockoutVoice); // Phát âm thanh knockout

        TextMeshProUGUI textMeshProUGUI = gameOverUI.transform.Find("KnockoutTMP").GetComponent<TextMeshProUGUI>(); // Lấy thành phần văn bản

        if (winnerUserTag == "User1" && !string.IsNullOrEmpty(PlayerPrefs.GetString("User1")))
        {
            string nameWinner = PlayerPrefs.GetString("User1"); // Lấy tên người thắng
            textMeshProUGUI.text = nameWinner + " is the winner!"; // Cập nhật văn bản
            Debug.Log("Người thắng là: " + nameWinner); // Ghi log
            //updateWinnerScore(nameWinner); // Cập nhật điểm số
        }
        else if (winnerUserTag == "User2" && !string.IsNullOrEmpty(PlayerPrefs.GetString("User2")))
        {
            string nameWinner = PlayerPrefs.GetString("User2"); // Lấy tên người thắng
            textMeshProUGUI.text = nameWinner + " is the winner!"; // Cập nhật văn bản
            Debug.Log("Người thắng là: " + nameWinner); // Ghi log
            //updateWinnerScore(nameWinner); // Cập nhật điểm số
        }
        else if (winnerUserTag == "User1")
        {
            Debug.Log("Giá trị của User1: " + PlayerPrefs.GetString("User1")); // Ghi log giá trị
            textMeshProUGUI.text = "Player 1 is the winner!"; // Cập nhật văn bản
        }
        else
        {
            Debug.Log("Giá trị của User2: " + PlayerPrefs.GetString("User2")); // Ghi log giá trị
            textMeshProUGUI.text = "Player 2 is the winner!"; // Cập nhật văn bản
        }
    }

    public void resume()
    {
        puaseUI.SetActive(false); // Ẩn giao diện tạm dừng
        Time.timeScale = 1; // Tiếp tục thời gian
        SoundsController.Instance.reactiveSound(); // Kích hoạt lại tất cả âm thanh
        //SoundsController.Instance.ResumeAllSounds(); // Kích hoạt lại tất cả âm thanh
        //SoundsController.Instance.RunSound(pauseSound);
    }

    public void restartGame()
    {
        Debug.Log("Đang khởi động lại trò chơi..."); // Ghi log thông báo

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Tải lại scene hiện tại
        Time.timeScale = 1; // Tiếp tục thời gian
    }

    public void goToMainMenu()
    {
        // Khi quay lại menu chính, đảm bảo âm nhạc được tiếp tục
        if (MenuMusicManager.Instance != null)
        {
            MenuMusicManager.Instance.ResumeMusic(); // Tiếp tục âm nhạc
        }

        PlayerPrefs.SetString("User1", ""); // Xóa dữ liệu User1
        PlayerPrefs.Save(); // Lưu thay đổi
        PlayerPrefs.SetString("User2", ""); // Xóa dữ liệu User2
        PlayerPrefs.Save(); // Lưu thay đổi

        SceneManager.LoadScene("MainMenu"); // Chuyển đến scene MainMenu
        Time.timeScale = 1; // Tiếp tục thời gian
    }

    public void goToFighterSelectionMenu()
    {
        // Khi quay lại menu chính, đảm bảo âm nhạc được tiếp tục
        if (MenuMusicManager.Instance != null)
        {
            MenuMusicManager.Instance.ResumeMusic(); // Tiếp tục âm nhạc
        }

        PlayerPrefs.SetString("User1", ""); // Xóa dữ liệu User1
        PlayerPrefs.Save(); // Lưu thay đổi
        PlayerPrefs.SetString("User2", ""); // Xóa dữ liệu User2
        PlayerPrefs.Save(); // Lưu thay đổi

        SceneManager.LoadScene("FighterSelectionMenu"); // Chuyển đến scene FighterSelectionMenu
        Time.timeScale = 1; // Tiếp tục thời gian
    }

    //private void updateWinnerScore(string username)
    //{
    //    string dbName = "URI=file:LeaderboardDB.db"; // Đường dẫn đến cơ sở dữ liệu

    //    using (var connection = new SqliteConnection(dbName))
    //    {
    //        connection.Open(); // Mở kết nối

    //        using (var command = connection.CreateCommand())
    //        {
    //            // Cập nhật điểm số của người thắng (cộng thêm 5)
    //            command.CommandText = "UPDATE User SET score = score + 5 WHERE username = @username";
    //            command.Parameters.AddWithValue("@username", username); // Gán tham số username
    //            int rowsAffected = command.ExecuteNonQuery(); // Thực thi lệnh

    //            if (rowsAffected > 0)
    //                Debug.Log("Điểm số đã được cập nhật cho: " + username); // Ghi log thành công
    //            else
    //                Debug.LogError("Không tìm thấy người dùng trong cơ sở dữ liệu: " + username); // Ghi log lỗi
    //        }

    //        connection.Close(); // Đóng kết nối
    //    }
    //}
}