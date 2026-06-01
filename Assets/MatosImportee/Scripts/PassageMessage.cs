using UnityEngine;
using TMPro;

public class PassageMessage : MonoBehaviour
{
    [Header("Message à afficher (passage)")]
    [TextArea(3, 5)]
    public string messagePassage = "Ce passage n'est pas accessible ou Passage accessible...";

    [Header("Texte UI à mettre à jour")]
    public TMP_Text messageUI;

    [Header("Couleur du message (passage)")]
    public Color couleurPassage = new Color(1f, 0.6f, 0.2f); // orange

    [Header("Message d'origine (quand on s'éloigne)")]
    [TextArea(2, 3)]
    public string messageOriginal = "Mission : rejoignez l'hôpital par un chemin accessible.";

    [Header("Couleur du message d'origine")]
    public Color couleurOriginale = Color.white;

    private void OnCollisionEnter(Collision collision)
    {
        Afficher(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Afficher(other.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) Restaurer();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) Restaurer();
    }

    private void Afficher(GameObject objetTouche)
    {
        if (GoalTrigger.missionTerminee) return;
        if (!objetTouche.CompareTag("Player")) return;

        if (messageUI != null)
        {
            messageUI.text = messagePassage;
            messageUI.color = couleurPassage;
        }
        // Volontairement : PAS de SignalerCollision() → le piège ne compte pas.
    }

    private void Restaurer()
    {
        if (GoalTrigger.missionTerminee) return;

        if (messageUI != null)
        {
            messageUI.text = messageOriginal;
            messageUI.color = couleurOriginale;
        }
    }
}