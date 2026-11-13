using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;

    [Header("Contenedor de Imágenes")]
    [SerializeField] private List<Image> heartImages = new List<Image>();

    /// <summary>
    /// Actualiza visualmente los corazones según la vida actual.
    /// </summary>
    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (i < currentHealth)
            {
                heartImages[i].sprite = fullHeartSprite;
                heartImages[i].enabled = true;
            }
            else
            {
                heartImages[i].sprite = emptyHeartSprite;
                heartImages[i].enabled = true;
            }
        }
    }
}