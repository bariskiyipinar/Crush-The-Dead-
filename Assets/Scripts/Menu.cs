using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject SettingsPanel;
    public void Play()
    {
        SceneManager.LoadScene("Market");
    }
    public void Settings()
    {
        SettingsPanel.SetActive(true);

    }

    public void SoundPause()
    {
        if (SoundManager.instance.Music != null)
        {
            AudioSource music = SoundManager.instance.Music;

            if (music.isPlaying)
            {
                music.Pause(); 
            }
            else
            {
                music.UnPause(); 
            }
        }
    }

    public void settingsBack()
    {
        SettingsPanel.SetActive(false);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
