using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager manager;

    [Header("UI Elements")]
    public GameObject endScreen;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI coinRewardText;
    public TextMeshProUGUI attemptsText;

    [Header("Game Data")]
    public Savedata data;
    public int score;
    public int winScore = 50;
    private int rewardAmount = 200;
    private int maxAttempts = 3;
    private int remainingAttempts;
    private bool isGameOver = false;

    private void Awake()
    {
        manager = this;
        SaveSystem.Initialize();
        data = new Savedata(0);

        remainingAttempts = PlayerPrefs.GetInt("Attempts", maxAttempts);
        UpdateAttemptsUI();

        if (AudioManager.instance != null)
            AudioManager.instance.PlayGameMusic(); // 🎵 Play game BGM
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        remainingAttempts--;
        PlayerPrefs.SetInt("Attempts", remainingAttempts);
        PlayerPrefs.Save();

        if (AudioManager.instance != null)
            AudioManager.instance.PlayPlayerDeath(); // 🔊 Lose sound

        ShowEndScreen(false);
    }

    public void GameWin()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (AudioManager.instance != null)
            AudioManager.instance.PlayWinSound(); // 🏆 Win sound

        ShowEndScreen(true);
        StartCoroutine(ReturnToMenuAfterDelay());
    }

    private void ShowEndScreen(bool won)
    {
        endScreen.SetActive(true);
        titleText.text = won ? "🏆 You Won!" : "💀 Game Over";
        titleText.color = won ? Color.green : Color.red;

        scoreText.text = "Score: " + score.ToString();

        string loadedData = SaveSystem.Load("Save");
        if (loadedData != null)
            data = JsonUtility.FromJson<Savedata>(loadedData);

        if (data.highscore < score)
            data.highscore = score;

        highscoreText.text = "Highscore: " + data.highscore.ToString();
        SaveSystem.Save("save", JsonUtility.ToJson(data));

        if (won)
            AddCoins(rewardAmount);
        else
            coinRewardText.text = "No Coins Earned";

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateAttemptsUI()
    {
        if (attemptsText != null)
            attemptsText.text = "Attempts Left: " + remainingAttempts;
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        if (score >= winScore && !isGameOver)
            GameWin();
    }

    private IEnumerator ReturnToMenuAfterDelay()
    {
        yield return new WaitForSeconds(5f);
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
