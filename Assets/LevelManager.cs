using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public static LevelManager manager;

    public GameObject deathScreen;

    public int score;



    private void Awake()
    {
        manager = this;
    }

    public void GameOver()
    {
        deathScreen.SetActive(true);
    }

     public void ReplyGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
