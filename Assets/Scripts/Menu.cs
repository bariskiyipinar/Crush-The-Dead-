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
        QualitySettings.globalTextureMipmapLimit = 2; // d���k ��z�n�rl�kte texture
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
        QualitySettings.shadows = ShadowQuality.Disable; // g�lgeler kapal�
        QualitySettings.antiAliasing = 0; // AA kapal�
    }

    public void SetMedium()
    {
        QualitySettings.globalTextureMipmapLimit = 1;
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
        QualitySettings.shadows = ShadowQuality.HardOnly; // sadece sert g�lge
        QualitySettings.antiAliasing = 2; // 2x AA
    }

    public void SetHigh()
    {
        QualitySettings.globalTextureMipmapLimit = 0; // tam ��z�n�rl�k
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
        QualitySettings.shadows = ShadowQuality.All; // t�m g�lgeler
        QualitySettings.antiAliasing = 4; // 4x AA
    }
}
