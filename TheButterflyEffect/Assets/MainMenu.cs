using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string startSceneName = "Terrain";
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Text qualityText;
    [SerializeField] private TMP_Text volumeText;

    private Resolution[] resolutions;

    private void Start()
    {
        resolutionDropdown.ClearOptions();
        resolutions = Screen.resolutions;
        Array.Reverse(resolutions);
        List<string> resolutionNames = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string name = resolutions[i].width.ToString() + "x" + resolutions[i].height.ToString();
            resolutionNames.Add(name);
        }
        resolutionDropdown.AddOptions(resolutionNames);
    }

    public void NewGame()
    {
        //Clear save file
        SceneLoader.LoadScene(startSceneName);
    }
    public void Continue()
    {
        //Load save file
        SceneLoader.LoadScene(startSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quitting...");
    }

    
    //Settings functions below
    public void SetQuality(float i)
    {
        QualitySettings.SetQualityLevel((int)i);
        qualityText.text = "Quality: " + QualitySettings.names[(int)i];
    }

    public void SetResolution(float i)
    {
        Screen.SetResolution(resolutions[(int)i].width, resolutions[(int)i].height, Screen.fullScreenMode);
    }

    public void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreenMode = fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeText.text = "Volume: " + (volume * 100).ToString("0") + "%";
    }
}
