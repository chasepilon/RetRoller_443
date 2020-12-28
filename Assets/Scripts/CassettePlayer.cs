using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CassettePlayer : MonoBehaviour
{
    public Slider volumeController;
    public Animator cassetteAnimator;
    public Text trackText;
    public AudioSource[] audioClips;
    public string[] trackNames;

    int tapeIndex;
    int currentTrackPlaying;
    int unlockIndex;

    void Start()
    {
        tapeIndex = -1;
        currentTrackPlaying = 0;
        trackText.text = "Please use the double up arrows to select the next track";
        cassetteAnimator.SetBool("cassetteIsPlaying", false);
        unlockIndex = PlayerPrefs.GetInt("CassettesUnlocked");
        if (unlockIndex > audioClips.Length)
            unlockIndex = audioClips.Length;
    }

    void Update()
    {
        if(tapeIndex >= 0 )
        {
            trackText.text = trackNames[tapeIndex];
        }

        audioClips[tapeIndex].volume = volumeController.value;
    }

    public void NextTrack()
    {
        
        if (tapeIndex >= unlockIndex )
            tapeIndex = 0;
        else
            tapeIndex++;
    }

    public void PlayTrack()
    {
        if(tapeIndex >= 0)
        {
            if (audioClips[currentTrackPlaying].isPlaying)
                audioClips[currentTrackPlaying].Stop();

            cassetteAnimator.SetBool("cassetteIsPlaying", true);
            audioClips[tapeIndex].Play();
            currentTrackPlaying = tapeIndex;
        }
    }

    public void StopTrack()
    {
        if(tapeIndex >= 0)
        {
            cassetteAnimator.SetBool("cassetteIsPlaying", false);
            audioClips[tapeIndex].Stop();
        }
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
