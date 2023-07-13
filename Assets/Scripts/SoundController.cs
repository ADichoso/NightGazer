using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    #region Singleton
    public static SoundController sharedInstance;

    void Awake()
    {
        sharedInstance = this;
        if (this != sharedInstance)
        {
            Debug.Log("Warning! More than 1 instance of SoundController has been detected");
        }
    }
    #endregion

    public AudioSource SFXSource;
    public AudioSource BGMSource;

    public AudioClip buttonClick;
    public AudioClip flashlightClick;
    public AudioClip correctAnswerSound, wrongAnswerSound;

    public AudioClip BGMSound;
    public void playSound(AudioClip clip, bool isLoop)
    {
        if (SFXSource.clip != null) SFXSource.Stop();

        SFXSource.clip = clip;

        SFXSource.loop = isLoop;

        SFXSource.Play();
    }


    public void playBGM()
    {
        if (BGMSource.clip != null) BGMSource.Stop();

        BGMSource.clip = BGMSound;
        BGMSource.loop = true;
        BGMSource.Play();
    }

    public void stopBGM()
    {
        BGMSource.Stop();
    }

    public void OnButtonClick()
    {
        playSound(buttonClick, false);
    }
}
