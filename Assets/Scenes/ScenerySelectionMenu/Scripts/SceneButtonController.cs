using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneButtonController : MonoBehaviour
{
    // Esta funci�n es llamada cuando se presiona un bot�n espec�fico
    public void LoadSceneByButton(string sceneName)
    {
        // Detener la m�sica si es una escena de pelea
        if (sceneName != "FighterSelectionMenu")
        {
            if (MenuMusicManager.Instance != null)
                MenuMusicManager.Instance.StopMusic();
        }

        SceneManager.LoadScene(sceneName);
    }
  
}
