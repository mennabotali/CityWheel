using UnityEngine;
using TMPro;

public class GoalTrigger : MonoBehaviour
{
    [Header("Texte UI à mettre à jour")]
    public TMP_Text messageUI;

    [Header("Message de victoire")]
    [TextArea(2, 4)]
    public string messageVictoire = "Mission réussie : vous êtes arrivé à l'hôpital !";

    [Header("Couleur du message de victoire (vert)")]
    public Color couleurVictoire = new Color(0.3f, 0.9f, 0.3f);

    private bool dejaArrive = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Quelque chose est entré dans la zone : " + other.name);

        if (other.CompareTag("Player") && !dejaArrive)
        {
            Debug.Log("Mission réussie : vous êtes arrivé à l'hôpital !");

            if (messageUI != null)
            {
                messageUI.text = messageVictoire;
                messageUI.color = couleurVictoire;
            }

            dejaArrive = true;
        }
    }
}