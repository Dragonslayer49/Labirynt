using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject Canvass;

    [SerializeField] Slider brightnessSlider;
    [SerializeField] Slider sensitivitySlider;
    public bool isPaused=false;

    private Color initialAmbientColor; // Store the initial ambient color
    public float mouseSensitivity = 100f; // Default sensitivity

    public TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    void Start()
    {
        Cursor.visible = false;
        Time.timeScale = 1f;
        initialAmbientColor = RenderSettings.ambientLight;
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 100f);  // Default sensitivity is 100
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 0.5f);  // Default brightness is 1

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + ", " + resolutions[i].refreshRate + "Hz";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);

        // Load saved resolution index, or set it to current
        int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Apply the saved resolution on start
        //SetResolution(savedResolutionIndex);

        // Set default slider values
        if (brightnessSlider != null)
        {
            brightnessSlider.value = RenderSettings.ambientIntensity;
            brightnessSlider.onValueChanged.AddListener(ChangeBrightness);
        }

        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = mouseSensitivity;
            sensitivitySlider.onValueChanged.AddListener(ChangeSensitivity);
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        // Save the selected resolution index in PlayerPrefs
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.Save();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void Home()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1f;
    }
    public void Resume()
    {
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        Canvass.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void ChangeBrightness(float value)
    {
        Debug.Log("Brightness changed to: " + value);  // Debugging output

        // Scale the initial ambient light color intensity based on the slider value
        RenderSettings.ambientLight = initialAmbientColor * value;
        PlayerPrefs.SetFloat("Brightness", value);
        PlayerPrefs.Save();
    }


    // Function to change mouse sensitivity
    public void ChangeSensitivity(float value)
    {
        mouseSensitivity = value;
        Debug.Log("Mouse Sensitivity changed to: " + mouseSensitivity);
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivity);
        PlayerPrefs.Save();
    }
}
