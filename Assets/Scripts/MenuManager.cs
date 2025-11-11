using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI coinText;      // Shows current coins
    public TextMeshProUGUI entryFeeText;  // Shows entry fee text

    private int coins;
    private int entryFee = 100;

    private void Start()
    {
        // Load saved coins (default to 0)
        coins = PlayerPrefs.GetInt("Coins", 0);
        UpdateUI();
    }

    public void PlayGame(string sceneName)
    {
        if (coins >= entryFee)
        {
            // Deduct entry fee
            coins -= entryFee;
            PlayerPrefs.SetInt("Coins", coins);
            PlayerPrefs.Save();

            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.Log("❌ Not enough coins to start the game!");
            entryFeeText.text = "Not enough coins! Need " + entryFee;
            entryFeeText.color = Color.red;
        }
    }

    private void Update()
    {
        // Keyboard cheat: +1000 coins
        if (Input.GetKeyDown(KeyCode.C))
        {
            AddCoins(1000);
            Debug.Log("💰 Cheat Activated! +1000 Coins");
        }
    }

    // 👇 Button-based cheat: +1000 coins
    public void CheatAddCoinsButton()
    {
        AddCoins(1000);
        Debug.Log("💵 Cheat Button Pressed! +1000 Coins");
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
        coinText.text = "Coins: " + coins.ToString();
        entryFeeText.text = "Entry Fee: " + entryFee.ToString();
        entryFeeText.color = Color.black; // reset color if previously red
    }
}
