using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public Button startScreen;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lastScoreText;


    public bool gameStarted = false;
    private int score = 0;


    // Start is called before the first frame update
    void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
        startScreen.onClick.AddListener(StartScreen);

        startScreen.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        lastScoreText.gameObject.SetActive(false);
        UpdateScoreText();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartScreen()
    {
        gameStarted = true;
        startScreen.gameObject.SetActive(false);
    }
    public void ShowGameOver()
    {


        gameOverText.text = "Game Over";
        gameOverText.gameObject.SetActive(true);

        lastScoreText.text = "Last Score: " + score;
        lastScoreText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(false);

        restartButton.gameObject.SetActive(true);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }
    public void UpdateScoreText()
    {

        scoreText.text = "Score: " + score;
    }

}
