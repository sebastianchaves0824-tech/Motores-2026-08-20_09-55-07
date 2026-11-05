using UnityEngine;
using UnityEngine.UI;

public class HeartsHealthUI : MonoBehaviour
{
    [Header("Imágenes de los Corazones")]
    [Tooltip("Arrastrá los 3 objetos Image de los corazones en orden")]
    [SerializeField] private Image[] corazones;

    [Header("Sprites (Opcional si usás 2 imágenes distintas)")]
    [SerializeField] private Sprite corazonLleno;
    [SerializeField] private Sprite corazonVacio;

    [Header("Colores (Si usás un solo sprite y querés tintarlo)")]
    [SerializeField] private Color colorActivo = Color.white;
    [SerializeField] private Color colorApagado = new Color(0.2f, 0.2f, 0.2f, 0.4f);

    public void ActualizarCorazones(int vidasRestantes)
    {
        for (int i = 0; i < corazones.Length; i++)
        {
            if (corazones[i] == null) continue;

            if (i < vidasRestantes)
            {
                if (corazonLleno != null) corazones[i].sprite = corazonLleno;
                corazones[i].color = colorActivo;
            }
            else
            {
                if (corazonVacio != null) corazones[i].sprite = corazonVacio;
                corazones[i].color = colorApagado;
            }
        }
    }
}