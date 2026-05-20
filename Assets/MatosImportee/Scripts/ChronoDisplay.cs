using UnityEngine;
using TMPro;

public class ChronoDisplay : MonoBehaviour
{
    [Header("Texte UI du chrono")]
    public TMP_Text chronoText;

    [Header("Référence au manager adaptatif")]
    public AdaptiveManager manager;

    [Header("Couleurs")]
    public Color couleurNormale = Color.white;
    public Color couleurProche = new Color(1f, 0.7f, 0.2f);
    public Color couleurDepasse = new Color(0.3f, 0.9f, 0.3f);

    [Header("Seuil de proximité (secondes avant le seuil)")]
    public float secondesAvantAlerte = 5f;

    private float tempsEcoule = 0f;
    private float seuilAdaptation = 30f;

    void Start()
    {
        if (manager != null)
        {
            seuilAdaptation = manager.tempsAvantAjoutDifficulte;
        }
    }

    void Update()
    {
        tempsEcoule += Time.deltaTime;

        int minutes = Mathf.FloorToInt(tempsEcoule / 60f);
        int secondes = Mathf.FloorToInt(tempsEcoule % 60f);

        if (chronoText != null)
        {
            chronoText.text = string.Format("{0:00}:{1:00}", minutes, secondes);

            if (tempsEcoule >= seuilAdaptation)
            {
                chronoText.color = couleurDepasse;
            }
            else if (tempsEcoule >= seuilAdaptation - secondesAvantAlerte)
            {
                chronoText.color = couleurProche;
            }
            else
            {
                chronoText.color = couleurNormale;
            }
        }
    }
}