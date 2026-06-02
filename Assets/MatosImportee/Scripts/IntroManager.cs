using UnityEngine;

public class IntroManager : MonoBehaviour
{
    [Header("Le panneau d'intro")]
    public GameObject introPanel;

    void Start()
    {
        if (introPanel != null) introPanel.SetActive(true);
        Time.timeScale = 0f; // tout est en pause pendant la lecture
    }

    public void Commencer()
    {
        if (introPanel != null) introPanel.SetActive(false);
        Time.timeScale = 1f; // on relance le jeu
    }
}