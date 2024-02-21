using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int potatoScoreValue = 0;
    public int WinReachNumber = 0;
    public int totalPotatoInScene = 0;
    public static int potatoRecolectedInScene = 0;
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
        isGameOver = false;

    }

    // Update is called once per frame
    void Update()
    {
        score.text = "" + potatoScoreValue;
        if (potatoScoreValue >= WinReachNumber && potatoRecolectedInScene == totalPotatoInScene)
        {
            isGameWon = true;
        }
        if (potatoScoreValue < WinReachNumber && potatoRecolectedInScene == totalPotatoInScene)
        {
            isGameOver = true;
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

    public void RestartScene()
    {
        potatoScoreValue = 0;
        potatoRecolectedInScene = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        

    }
}
