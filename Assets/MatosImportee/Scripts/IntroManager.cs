using UnityEngine;

public class IntroManager : MonoBehaviour
{
    [Header("Le panneau d'intro")]
    public GameObject introPanel;

    void Start()
    {
        if (introPanel != null) introPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Commencer()
    {
        if (introPanel != null) introPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}