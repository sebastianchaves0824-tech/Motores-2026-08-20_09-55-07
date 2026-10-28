using UnityEngine;
using UnityEngine.Audio;

public class AdaptiveMusic : MonoBehaviour
{
    [SerializeField] private AudioMixerSnapshot fastSnapshot;
    [SerializeField] private float transitionTime = 1.5f;

    [SerializeField] private AudioMixerSnapshot normalSnapshot;

    private void OnTriggerEnter(Collider other) 
    { 
      if(other.CompareTag("Player") && normalSnapshot != null) 
        { 
          fastSnapshot.TransitionTo(transitionTime);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player") && normalSnapshot != null) 
        {
            normalSnapshot.TransitionTo(transitionTime);
        }
    }
}
