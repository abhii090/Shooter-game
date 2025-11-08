using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager manager;

    public GameObject deathScreen;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;

    public Savedata data;

    public int score;

    private void Awake()
    {
        manager = this;
        SaveSystem.Initialize();
        data = new Savedata(0);
    }

    public void GameOver()
    {
        Debug.Log("GameOver Called");
        deathScreen.SetActive(true);
        scoreText.text = "Score: " + score.ToString();

        string loadedData = SaveSystem.Load("Save");
        if (loadedData != null)
        {
            data = JsonUtility.FromJson<Savedata>(loadedData);
        }

        // Removed extra semicolon and fixed logic
        if (data.highscore < score)
        {
            data.highscore = score;
        }

        highscoreText.text = "Highscore: " + data.highscore.ToString();

        // Fixed capitalization mismatch (SaveData → saveData)
        string saveData = JsonUtility.ToJson(data);
        SaveSystem.Save("save", saveData);
    }

    // Probably meant "ReplayGame" — fixed typo
    public void ReplayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
    }
}

[System.Serializable] // Fixed capitalization
public class Savedata
{
    public int highscore;

    public Savedata(int _hs)
    {
        highscore = _hs;
    }
}
