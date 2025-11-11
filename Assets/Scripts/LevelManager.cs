using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager manager;

    [Header("UI Elements")]
    public GameObject endScreen; // used for both win/lose
    public TextMeshProUGUI titleText;   // “You Won!” / “Game Over!”
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI coinRewardText;
    public TextMeshProUGUI attemptsText;

    [Header("Game Data")]
    public Savedata data;
    public int score;
    public int winScore = 50; // 👈 Win condition
    private int rewardAmount = 200;
    private int maxAttempts = 3;
    private int remainingAttempts;

    private bool isGameOver = false;

    private void Awake()
    {
        manager = this;
        SaveSystem.Initialize();
        data = new Savedata(0);

        // Load attempts
        remainingAttempts = PlayerPrefs.GetInt("Attempts", maxAttempts);
        UpdateAttemptsUI();
    }

    // 🟥 LOSE CONDITION — called when player dies
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        remainingAttempts--;
        PlayerPrefs.SetInt("Attempts", remainingAttempts);
        PlayerPrefs.Save();

        ShowEndScreen(false); // false = lose
    }

    // 🟩 WIN CONDITION — called when player achieves a goal
    public void GameWin()
    {
        if (isGameOver) return;
        isGameOver = true;

        ShowEndScreen(true); // true = win
        StartCoroutine(ReturnToMenuAfterDelay()); // 👈 Auto return to Menu
    }

    private void ShowEndScreen(bool won)
    {
        endScreen.SetActive(true);

        // Title
        titleText.text = won ? "🏆 You Won!" : "💀 Game Over";
        titleText.color = won ? Color.green : Color.red;

        // Score & Highscore
        scoreText.text = "Score: " + score.ToString();

        string loadedData = SaveSystem.Load("Save");
        if (loadedData != null)
            data = JsonUtility.FromJson<Savedata>(loadedData);

        if (data.highscore < score)
            data.highscore = score;

        highscoreText.text = "Highscore: " + data.highscore.ToString();
        SaveSystem.Save("save", JsonUtility.ToJson(data));

        // Handle reward only for win
        if (won)
            AddCoins(rewardAmount);
        else
            coinRewardText.text = "No Coins Earned";

        // Handle attempts
        UpdateAttemptsUI();

        if (remainingAttempts <= 0 && !won)
        {
            coinRewardText.text = "No Attempts Left!";
            coinRewardText.color = Color.red;
            PlayerPrefs.SetInt("Attempts", maxAttempts);
            PlayerPrefs.Save();
        }
    }

    private void AddCoins(int amount)
    {
        int currentCoins = PlayerPrefs.GetInt("Coins", 0);
        currentCoins += amount;
        PlayerPrefs.SetInt("Coins", currentCoins);
        PlayerPrefs.Save();

        coinRewardText.text = "+ " + amount + " Coins!";
        coinRewardText.color = Color.yellow;
    }

    public void ReplayGame()
    {
        if (remainingAttempts > 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }
    }

    private void UpdateAttemptsUI()
    {
        if (attemptsText != null)
            attemptsText.text = "Attempts Left: " + remainingAttempts;
    }

    // 🟢 Increase score and check win condition
    public void IncreaseScore(int amount)
    {
        score += amount;

        // Check for win condition
        if (score >= winScore && !isGameOver)
        {
            GameWin();
        }
    }

    // 🕒 Automatically return to Menu after win
    private IEnumerator ReturnToMenuAfterDelay()
    {
        yield return new WaitForSeconds(5f); // Wait 5 seconds
        SceneManager.LoadScene("Menu");
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

