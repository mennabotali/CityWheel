using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndManager : MonoBehaviour
{
    [Header("Le panneau de fin (caché au départ)")]
    public GameObject endPanel;

    [Header("Le grand texte du panneau de fin")]
    public TMP_Text endText;

    [Header("Le texte du chrono (pour récupérer le temps final)")]
    public TMP_Text chronoText;

    [Header("Référence à l'AdaptiveManager (pour le nombre d'obstacles)")]
    public AdaptiveManager adaptiveManager;

    private bool finAffichee = false;

    void Start()
    {
        if (endPanel != null) endPanel.SetActive(false); // caché tant qu'on n'est pas arrivé
        PassageMessage.passagesBloques = 0; // réinitialise le compteur de passages bloqués
    }

    void Update()
    {
        // Quand la mission est terminée, on affiche l'écran de fin une seule fois
        if (GoalTrigger.missionTerminee && !finAffichee)
        {
            AfficherFin();
        }
    }

    private void AfficherFin()
    {
        finAffichee = true;

        // Récupère les statistiques
        string temps = (chronoText != null) ? chronoText.text : "--:--";
        int obstacles = (adaptiveManager != null) ? adaptiveManager.CollisionsTotales : 0;
        int passages = PassageMessage.passagesBloques;

        if (endText != null)
        {
            endText.text =
                "MISSION ACCOMPLIE !!!\n\n" +
                "Vous avez rejoint l'hôpital !!!\n\n" +
                "Temps : " + temps + "\n" +
                "Obstacles rencontrés : " + obstacles + "\n" +
                "Passages fermés rencontrés : " + passages + "\n\n" +
                "Les trottoirs sans rampe, les passages fermés et les détours que vous venez de vivre sont, pour une personne en fauteuil roulant, une réalité quotidienne.";
        }

        // Affiche le panneau et met le jeu en pause
        if (endPanel != null) endPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // Appelé par le bouton "Rejouer"
    public void Rejouer()
    {
        Time.timeScale = 1f;
        GoalTrigger.missionTerminee = false; // on remet le compteur de mission à zéro
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // recharge la scène
    }
}