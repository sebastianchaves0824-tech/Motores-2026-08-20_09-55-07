using UnityEngine;
using UnityEngine.Audio;

public class VozGameOver : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] gameoverVoiceLines;
    [SerializeField] private AudioMixerGroup grupoMixer;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (gameoverVoiceLines != null && gameoverVoiceLines.Length > 0 && audioSource != null) 
        {
            GameOverVoiceLine();
        }

        
        if (audioSource != null && grupoMixer != null)
        {
            audioSource.outputAudioMixerGroup = grupoMixer;
        }
    }

    

    private void GameOverVoiceLine()
    {
      int randomIndex = Random.Range(0, gameoverVoiceLines.Length);
      AudioClip clipToPlay = gameoverVoiceLines[randomIndex];
      audioSource.PlayOneShot(clipToPlay);
    }
}
