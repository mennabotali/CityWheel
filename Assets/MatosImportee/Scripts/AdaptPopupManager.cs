using UnityEngine;
using TMPro;

public class AdaptPopupManager : MonoBehaviour
{
    public GameObject adaptPanel;
    public TMP_Text adaptText;
    public AdaptPopupManager popup;

    void Start()
    {
        if (adaptPanel != null) adaptPanel.SetActive(false);
    }

    public void Afficher(string texte)
    {
        if (GoalTrigger.missionTerminee) return;
        if (adaptText != null) adaptText.text = texte;
        if (adaptPanel != null) adaptPanel.SetActive(true);
        Time.timeScale = 0f; // pause
    }

    public void Compris()
    {
        if (adaptPanel != null) adaptPanel.SetActive(false);
        Time.timeScale = 1f; // reprise
    }
}