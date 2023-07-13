using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleScreenController : MonoBehaviour
{
    private void Start()
    {
        SoundController.sharedInstance.playBGM();
    }


    public void onPlayButton()
    {
        SoundController.sharedInstance.playSound(SoundController.sharedInstance.buttonClick, false);
        SceneManager.LoadScene(1);
    }

    public void onQuitButton()
    {
        SoundController.sharedInstance.playSound(SoundController.sharedInstance.buttonClick, false);
        Application.Quit();
    }
}
