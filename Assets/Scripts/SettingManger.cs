using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject settingsPanel;
    public Toggle bgmToggle;
    public Toggle sfxToggle;

    private void Start()
    {
        // Load saved values from PlayerPrefs via AudioManager
        if (AudioManager.instance != null)
        {
            bgmToggle.isOn = AudioManager.instance.IsBGMEnabled();
            sfxToggle.isOn = AudioManager.instance.IsSFXEnabled();
        }

        // Add listeners
        bgmToggle.onValueChanged.AddListener(OnBgmToggleChanged);
        sfxToggle.onValueChanged.AddListener(OnSfxToggleChanged);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        if (AudioManager.instance != null)
            AudioManager.instance.PlayButtonClick();
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        if (AudioManager.instance != null)
            AudioManager.instance.PlayButtonClick();
    }

    private void OnBgmToggleChanged(bool value)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetBGMEnabled(value);
            AudioManager.instance.PlayButtonClick(); // feedback sound
        }
    }

    private void OnSfxToggleChanged(bool value)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetSFXEnabled(value);
            AudioManager.instance.PlayButtonClick(); // feedback sound
        }
    }
}

