using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int potatoScoreValue = 0;
    public int WinReachNumber = 1;
    public static bool isGameOver = false;
    public static bool isGameWon = false;
    public GameObject sceneHUD;
    public GameObject winMenu;
    public GameObject gameOverMenu;
    Text score;
    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<Text>();
        potatoScoreValue = 0;
    }

    private void Awake()
    {
        sceneHUD.SetActive(true);
        winMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        isGameWon = false;

    }

    // Update is called once per frame
    void Update()
    {
        score.text = "" + potatoScoreValue;
        if (potatoScoreValue >= WinReachNumber)
        {
            isGameWon = true;
        }
        if (isGameOver)
        {

            sceneHUD.SetActive(false);
            PauseGame();
            gameOverMenu.SetActive(true);

        }

        if (isGameWon)
        {
            sceneHUD.SetActive(false);
            PauseGame();
            winMenu.SetActive(true);

        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
