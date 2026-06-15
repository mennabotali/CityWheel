using UnityEngine;
using TMPro;

public class AdaptPopupManager : MonoBehaviour
{
    [Header("Pop-up d'adaptation (COMPRIS)")]
    public GameObject adaptPanel;
    public TMP_Text adaptText;

    void Start()
    {
        if (adaptPanel != null) adaptPanel.SetActive(false);
    }

    public void Afficher(string texte)
    {
        if (GoalTrigger.missionTerminee) return;
        if (adaptText != null) adaptText.text = texte;
        if (adaptPanel != null) adaptPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Compris()
    {
        if (adaptPanel != null) adaptPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}