using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    //AudioManager audioManager;

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.backgroundMusic);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
