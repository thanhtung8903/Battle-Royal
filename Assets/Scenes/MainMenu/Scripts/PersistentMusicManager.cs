using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuMusicManager : MonoBehaviour
{
    public static MenuMusicManager Instance { get; private set; }

    [Header("Clips de M�sica")]
    [SerializeField] private AudioClip menuMusic; // M�sica compartida entre las tres escenas
    [SerializeField] private AudioClip knockOutAudio;

    [Header("Componentes")]
    [SerializeField] private AudioSource audioSource; // AudioSource que reproducir� la m�sica

    
    private void Awake()
    {
        // Verifica si ya existe una instancia
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destruir duplicados
            return;
        }

        // Hacer persistente entre escenas
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Configurar el AudioSource si no est� asignado
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true; // Hacer que la m�sica se repita
        }

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
        audioSource.loop = false; // Se asegura de que solo se reproduzca una vez
        audioSource.Play();

        yield return new WaitForSeconds(knockOutAudio.length); // Esperar a que termine el audio

        PlayMenuMusic();
    }

    private void PlayMenuMusic()
    {
        audioSource.clip = menuMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    /// <summary>
    /// Reanuda la m�sica si est� detenida.
    /// </summary>
    public void ResumeMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        // Detener la m�sica cuando se cargue una escena de pelea
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
