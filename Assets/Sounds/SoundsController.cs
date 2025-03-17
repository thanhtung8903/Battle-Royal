using UnityEngine;

public class SoundsController : MonoBehaviour
{
    public static SoundsController Instance; // Instance tĩnh để truy cập từ mọi nơi
    private AudioSource audioSource; // Thành phần AudioSource để phát âm thanh

    [Header("Cài đặt âm thanh")]
    [SerializeField] private AudioClip backgroundSound; // Dùng để gán âm thanh nền
    [SerializeField] private AudioClip fightSound; // Âm thanh khi chiến đấu
    private bool isPaused = false; // Biến để quản lý trạng thái tạm dừng

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Gán instance của SoundsController
            DontDestroyOnLoad(gameObject); // Giữ đối tượng không bị hủy khi chuyển scene
        }
        else
        {
            Destroy(gameObject); // Hủy đối tượng nếu đã có instance khác
        }

        // Lấy thành phần AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("Không tìm thấy thành phần AudioSource trong đối tượng SoundsController.");
        }

        // Cấu hình âm thanh nền để lặp lại
        audioSource.loop = true; // Cho phép âm thanh nền lặp lại
    }

    // Phương thức để khởi động một âm thanh (như hiệu ứng)
    public void RunSound(AudioClip sonido)
    {
        if (audioSource != null && sonido != null)
        {
            audioSource.PlayOneShot(sonido); // Phát âm thanh một lần
        }
        else if (sonido == null)
        {
            Debug.LogWarning("AudioClip truyền vào RunSound là null.");
        }
    }

    // Tạm dừng tất cả âm thanh
    public void pauseSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause(); // Tạm dừng âm thanh
            isPaused = true; // Lưu trạng thái tạm dừng
        }
        else
        {
            Debug.LogWarning("Không có âm thanh nào đang phát để tạm dừng.");
        }
    }

    // Kích hoạt lại âm thanh
    public void reactiveSound()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            if (isPaused)
            {
                audioSource.UnPause(); // Tiếp tục từ điểm tạm dừng
            }
            else
            {
                audioSource.Play(); // Phát lại từ đầu nếu không ở trạng thái tạm dừng
            }
        }
        else
        {
            Debug.LogWarning("Âm thanh đã đang phát hoặc chưa được cấu hình.");
        }
    }

    // Phương thức này nên được gọi từ GameSceneManager khi khởi động scene để tránh khởi động lại âm thanh
    public void StartBackgroundMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.clip = backgroundSound; // Gán âm thanh nền
            audioSource.Play(); // Phát âm thanh
        }
    }

    private void Start()
    {
        RunSound(fightSound); // Phát âm thanh chiến đấu khi khởi động
    }
}