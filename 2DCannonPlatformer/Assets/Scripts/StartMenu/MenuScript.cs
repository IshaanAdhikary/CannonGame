using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuScript : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown graphicsDropdown;

    private Scene scene;
    private Resolution[] resolutionsRaw;
    private Resolution[] resolutions;
    private int currentGraphicsIndex;

    void Start()
    {
        scene = SceneManager.GetActiveScene();

        resolutions = Screen.resolutions;
        //resolutions = resolutionsRaw.Distinct().ToArray();
        currentGraphicsIndex = QualitySettings.GetQualityLevel();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @" + resolutions[i].refreshRate + " hz";
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height && resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        graphicsDropdown.value = currentGraphicsIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StartGame(int number)
    {
        SceneManager.LoadScene(number);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
