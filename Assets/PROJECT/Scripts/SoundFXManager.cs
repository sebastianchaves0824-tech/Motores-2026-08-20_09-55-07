using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundFXManager : MonoBehaviour
{
    
        public static SoundFXManager Instance { get; private set; }

        [Header("Pool Settings")]
        [SerializeField] private GameObject audioSourcePrefab;
        [SerializeField] private int poolSize = 20;

        [Header("Audio Mixer Integration")]
       
        [SerializeField] private AudioMixerGroup sfxMixerGroup;
        

        [Header("Audio Clips")]
        [SerializeField] private List<SoundData> soundClips;

        private Queue<GameObject> poolQueue = new Queue<GameObject>();
        private Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();

        private void Awake()
        {
        
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializePool();
                InitializeDictionary();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializePool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(audioSourcePrefab, transform);

                
                AudioSource source = obj.GetComponent<AudioSource>();
                if (source != null && sfxMixerGroup != null)
                {
                    source.outputAudioMixerGroup = sfxMixerGroup;
                }

                obj.SetActive(false);
                poolQueue.Enqueue(obj);
            }
        }

        private void InitializeDictionary()
        {
            foreach (var sound in soundClips)
            {
                if (!soundDictionary.ContainsKey(sound.name))
                {
                    soundDictionary.Add(sound.name, sound.clip);
                }
            }
        }

        public void Play3DSound(string soundName, Vector3 position, float volume = 1f)
        {
            if (!soundDictionary.TryGetValue(soundName, out AudioClip clip))
            {
                Debug.LogWarning($"SoundManager: El sonido '{soundName}' no existe.");
                return;
            }

            GameObject soundObj = GetPooledObject();
            soundObj.transform.position = position;
            soundObj.SetActive(true);

            AudioSource source = soundObj.GetComponent<AudioSource>();
            source.clip = clip;
            source.volume = volume;
            

            source.Play();

            StartCoroutine(ReturnToPoolAfterPlayback(soundObj, clip.length));
        }

        private GameObject GetPooledObject()
        {
            if (poolQueue.Count > 0)
            {
                return poolQueue.Dequeue();
            }
            else
            {
                Debug.LogWarning("SoundManager: Pool agotado, creando objeto extra.");
                GameObject obj = Instantiate(audioSourcePrefab, transform);

                
                AudioSource source = obj.GetComponent<AudioSource>();
                if (source != null && sfxMixerGroup != null)
                {
                    source.outputAudioMixerGroup = sfxMixerGroup;
                }

                return obj;
            }
        }

        private IEnumerator ReturnToPoolAfterPlayback(GameObject obj, float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            obj.SetActive(false);
            poolQueue.Enqueue(obj);
        }
    

    [System.Serializable]
    public struct SoundData
    {
        public string name;
        public AudioClip clip;
    }

    

}
