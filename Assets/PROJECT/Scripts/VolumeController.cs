using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private Slider volumeSlider;

    private const string PREF_VOLUMEN = "MasterVolumePref";

    private void Start()
    {
        // Recupera el volumen guardado (por defecto 1 = volumen máximo)
        float volumenGuardado = PlayerPrefs.GetFloat(PREF_VOLUMEN, 1f);

        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0.0001f; // Evita Log10(0) = -Infinito
            volumeSlider.maxValue = 1f;
            volumeSlider.value = volumenGuardado;
            volumeSlider.onValueChanged.AddListener(CambiarVolumen);
        }

        AplicarVolumen(volumenGuardado);
    }

    public void CambiarVolumen(float valorSlider)
    {
        AplicarVolumen(valorSlider);
        PlayerPrefs.SetFloat(PREF_VOLUMEN, valorSlider);
        PlayerPrefs.Save();
    }

    private void AplicarVolumen(float valor)
    {
        // Convierte el rango lineal (0.0001 a 1) a decibelios (-80 dB a 0 dB)
        float decibelios = Mathf.Log10(Mathf.Max(valor, 0.0001f)) * 20f;
        if (mainMixer != null)
        {
            mainMixer.SetFloat("MasterVolume", decibelios);
        }
    }
}