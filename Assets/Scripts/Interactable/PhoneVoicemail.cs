/*
    Created by: Xander
    Description: Controls when the phone voicemail plays and pauses, as well as the button states
*/

using UnityEngine;

public class PhoneVoicemail : MonoBehaviour
{
    public AudioSource voicemailAudio;

    public GameObject playButton;
    public GameObject pauseButton;

    void Start()
    {
        playButton.SetActive(true);
        pauseButton.SetActive(false);
    }

    void Update()
    {
        // If the clip has finished playing,
        // show the Play button again.
        if (!voicemailAudio.isPlaying &&
            voicemailAudio.time > 0)
        {
            playButton.SetActive(true);
            pauseButton.SetActive(false);
        }
    }

    public void PlayVoicemail() // Plays the voicemail, update relevant buttons
    {
        voicemailAudio.Play();

        playButton.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void PauseVoicemail() // Pauses the voicemail, update relevant buttons
    {
        voicemailAudio.Pause();

        playButton.SetActive(true);
        pauseButton.SetActive(false);
    }
}