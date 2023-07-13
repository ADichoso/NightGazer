using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class UIController : MonoBehaviour
{
    #region Singleton
    public static UIController sharedInstance;

    void Awake()
    {
        sharedInstance = this;
        if (this != sharedInstance)
        {
            Debug.Log("Warning! More than 1 instance of UIController has been detected");
        }
    }
    #endregion

    [SerializeField] public GameObject messagePanel;
    [SerializeField] public GameObject endGamePanel;
    [SerializeField] public Button messageButton;
    [SerializeField] public TextMeshProUGUI highScore;
    public bool isPaused = false;

    public void showLevelCompletePanel(int level_index)
    {
        isPaused = true;
        messagePanel.SetActive(true);

        messageButton.GetComponentInChildren<TextMeshProUGUI>().text = "Next Level?";
        messageButton.onClick.AddListener(delegate { goToNextLevel(level_index); });
    }

    public void showGameOverPanel(int level_index)
    {
        isPaused = true;
        messagePanel.SetActive(true);

        messageButton.GetComponentInChildren<TextMeshProUGUI>().text = "Try Again?";
        messageButton.onClick.AddListener(delegate { goToNextLevel(level_index); });
    }

    void goToNextLevel(int level_index)
    {
        SoundController.sharedInstance.playSound(SoundController.sharedInstance.buttonClick, false);

        messagePanel.SetActive(false);
        LevelController.sharedInstance.StartLevel(level_index);
    }

    public void showEndScreen()
    {
        string highScoreText = "";

        for (int i = 0; i < TimeController.sharedInstance.totalTimes.Length; i++)
        {
            highScoreText += "Level " + (i + 1).ToString() + ": " + TimeController.sharedInstance.totalTimes[i] + "\n";
        }

        highScore.text = highScoreText;

        isPaused = true;
        endGamePanel.SetActive(true);
    }

    public void OnRestartButton()
    {
        SoundController.sharedInstance.playSound(SoundController.sharedInstance.buttonClick, false);
        SceneManager.LoadScene(1);
    }

    public void OnQuitButton()
    {
        SoundController.sharedInstance.playSound(SoundController.sharedInstance.buttonClick, false);
        SceneManager.LoadScene(0);
    }

}
