using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuMusicManager : MonoBehaviour
{
    public static MenuMusicManager Instance { get; private set; }

    [Header("Clips de Música")]
    [SerializeField] private AudioClip menuMusic; // Nhạc dùng chung giữa ba cảnh
    [SerializeField] private AudioClip knockOutAudio; // Âm thanh khi bị hạ gục

    [Header("Componentes")]
    [SerializeField] private AudioSource audioSource; // AudioSource dùng để phát nhạc


    private void Awake()
    {
        // Kiểm tra xem đã có một phiên bản tồn tại chưa
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Xóa các bản sao thừa
            return;
        }

        // Giữ đối tượng này không bị hủy khi chuyển cảnh
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Cấu hình AudioSource nếu chưa được gán
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true; // Đặt nhạc chạy lặp lại
        }
        // Nếu đang ở màn hình chính, phát âm thanh knock-out trước rồi phát nhạc menu
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            StartCoroutine(PlayKnockoutThenMenuMusic());
        }
        else
        {
            PlayMenuMusic();
        }
    }

    private IEnumerator PlayKnockoutThenMenuMusic()
    {
        audioSource.clip = knockOutAudio;
        audioSource.loop = false; // Đảm bảo chỉ phát một lần
        audioSource.Play();

        yield return new WaitForSeconds(knockOutAudio.length); // Chờ cho âm thanh phát hết

        PlayMenuMusic();
    }

    private void PlayMenuMusic()
    {
        audioSource.clip = menuMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void ResumeMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        // Dừng nhạc khi một cảnh chiến đấu được tải.
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
