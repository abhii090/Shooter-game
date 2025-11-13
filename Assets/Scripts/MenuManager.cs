using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI entryFeeText;
    public Button playButton;
    public GameObject settingsPanel;
    public Toggle bgmToggle;
    public Toggle sfxToggle;

    private int coins;
    private int entryFee = 100;

    private void Start()
    {
        // Safety: log which references are missing (helps debug NullReferenceException)
        LogNullReferences();

        // Initialize coins if needed
        if (!PlayerPrefs.HasKey("Coins"))
            PlayerPrefs.SetInt("Coins", 500);

        coins = PlayerPrefs.GetInt("Coins", 0);
        UpdateUI();

        // Assign Play button safely
        if (playButton != null)
        {
            // remove previous listeners to avoid duplicates
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(OnPlayButtonPressed);
        }
        else
        {
            Debug.LogWarning("[MenuManager] playButton is not assigned in the inspector.");
        }

        // Settings panel: ensure it's not null
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        // Setup toggles only if they exist and AudioManager exists
        if (bgmToggle != null)
        {
            bool isOn = AudioManager.instance != null ? AudioManager.instance.IsBGMEnabled() : true;
            bgmToggle.isOn = isOn;
            bgmToggle.onValueChanged.RemoveAllListeners();
            bgmToggle.onValueChanged.AddListener(OnBgmToggleChanged);
        }
        else
        {
            Debug.Log("[MenuManager] bgmToggle not assigned (optional).");
        }

        if (sfxToggle != null)
        {
            bool isOn = AudioManager.instance != null ? AudioManager.instance.IsSFXEnabled() : true;
            sfxToggle.isOn = isOn;
            sfxToggle.onValueChanged.RemoveAllListeners();
            sfxToggle.onValueChanged.AddListener(OnSfxToggleChanged);
        }
        else
        {
            Debug.Log("[MenuManager] sfxToggle not assigned (optional).");
        }

        // Start menu music if possible
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayMenuMusic();
        }
    }

    private void LogNullReferences()
    {
        if (coinText == null) Debug.LogError("[MenuManager] coinText is NULL. Assign it in the Inspector.");
        if (entryFeeText == null) Debug.LogWarning("[MenuManager] entryFeeText is NULL. (optional but recommended)");
        if (playButton == null) Debug.LogError("[MenuManager] playButton is NULL. Assign it in the Inspector.");
        if (settingsPanel == null) Debug.LogWarning("[MenuManager] settingsPanel is NULL. (optional)");
        if (bgmToggle == null) Debug.LogWarning("[MenuManager] bgmToggle is NULL. (optional)");
        if (sfxToggle == null) Debug.LogWarning("[MenuManager] sfxToggle is NULL. (optional)");
    }

    private void OnPlayButtonPressed()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.PlayButtonClick();

        if (coins >= entryFee)
        {
            coins -= entryFee;
            PlayerPrefs.SetInt("Coins", coins);
            PlayerPrefs.Save();

            // Optionally tell LevelManager to auto-start
            PlayerPrefs.SetInt("StartGame", 1);
            SceneManager.LoadScene("Game");
        }
        else
        {
            Debug.Log("Not enough coins to play.");
            if (entryFeeText != null)
            {
                entryFeeText.text = "Not enough coins! Need " + entryFee;
                entryFeeText.color = Color.red;
            }
        }
    }

    private void OnBgmToggleChanged(bool on)
    {
        if (AudioManager.instance != null)
            AudioManager.instance.SetBGMEnabled(on);

        if (AudioManager.instance != null)
            AudioManager.instance.PlayButtonClick();
    }

    private void OnSfxToggleChanged(bool on)
    {
        if (AudioManager.instance != null)
            AudioManager.instance.SetSFXEnabled(on);

        if (AudioManager.instance != null)
            AudioManager.instance.PlayButtonClick();
    }

    private void Update()
    {
        // Optional cheat
        if (Input.GetKeyDown(KeyCode.C))
        {
            AddCoins(1000);
        }
    }

    public void CheatAddCoinsButton()
    {
        AddCoins(1000);
    }

    private void AddCoins(int amount)
    {
        coins += amount;
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (coinText != null)
            coinText.text = "Coins: " + coins.ToString();

        if (entryFeeText != null)
        {
            entryFeeText.text = "Entry Fee: " + entryFee.ToString();
            entryFeeText.color = Color.black;
        }
    }
}
