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



    public void SetLow()
    {
        QualitySettings.globalTextureMipmapLimit = 2; 
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
        QualitySettings.shadows = ShadowQuality.Disable; 
        QualitySettings.antiAliasing = 0; 
    }

    public void SetMedium()
    {
        QualitySettings.globalTextureMipmapLimit = 1;
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
        QualitySettings.shadows = ShadowQuality.HardOnly; 
        QualitySettings.antiAliasing = 2; 
    }

    public void SetHigh()
    {
        QualitySettings.globalTextureMipmapLimit = 0; 
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
        QualitySettings.shadows = ShadowQuality.All; 
        QualitySettings.antiAliasing = 4; 
    }
}
