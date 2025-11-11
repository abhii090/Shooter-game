using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager manager;

    public GameObject deathScreen;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI coinRewardText; // existing UI
    public TextMeshProUGUI attemptsText;   // 👈 new UI to display remaining attempts

    public Savedata data;
    public int score;

    private int rewardAmount = 200;
    private int maxAttempts = 3;
    private int remainingAttempts;

    private void Awake()
    {
        manager = this;
        SaveSystem.Initialize();
        data = new Savedata(0);

        // Load attempts (reset to 3 if not stored)
        remainingAttempts = PlayerPrefs.GetInt("Attempts", maxAttempts);
        UpdateAttemptsUI();
    }

    public void GameOver()
    {
        Debug.Log("GameOver Called");

        // Reduce one attempt
        remainingAttempts--;

        // Save remaining attempts
        PlayerPrefs.SetInt("Attempts", remainingAttempts);
        PlayerPrefs.Save();

        // Show death screen
        deathScreen.SetActive(true);
        scoreText.text = "Score: " + score.ToString();

        // Handle score & highscore
        string loadedData = SaveSystem.Load("Save");
        if (loadedData != null)
        {
            data = JsonUtility.FromJson<Savedata>(loadedData);
        }

        if (data.highscore < score)
        {
            data.highscore = score;
        }

        highscoreText.text = "Highscore: " + data.highscore.ToString();

        string saveData = JsonUtility.ToJson(data);
        SaveSystem.Save("save", saveData);

        // Add coins for playing
        AddCoinsOnGameOver();

        // Update attempts UI
        UpdateAttemptsUI();

        // If no attempts left, show message
        if (remainingAttempts <= 0)
        {
            coinRewardText.text = "No Attempts Left!";
            coinRewardText.color = Color.red;
            // Optionally reset attempts for next session
            PlayerPrefs.SetInt("Attempts", maxAttempts);
            PlayerPrefs.Save();
        }
    }

    private void AddCoinsOnGameOver()
    {
        int currentCoins = PlayerPrefs.GetInt("Coins", 0);
        currentCoins += rewardAmount;
        PlayerPrefs.SetInt("Coins", currentCoins);
        PlayerPrefs.Save();

        if (coinRewardText != null)
            coinRewardText.text = "+ " + rewardAmount.ToString() + " Coins!";
        else
            Debug.Log("💰 Reward: +" + rewardAmount + " coins added!");
    }

    public void ReplayGame()
    {
        // Reload current scene if attempts remain
        if (remainingAttempts > 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Debug.Log("❌ No attempts left. Returning to Menu.");
            SceneManager.LoadScene("Menu");
        }
    }

    private void UpdateAttemptsUI()
    {
        if (attemptsText != null)
        {
            attemptsText.text = "Attempts Left: " + remainingAttempts.ToString();
        }
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
    }
}

[System.Serializable]
public class Savedata
{
    public int highscore;

    public Savedata(int _hs)
    {
        highscore = _hs;
    }
}
