using UnityEngine;
using TMPro;

public class ObstacleMessage : MonoBehaviour
{
    [Header("Message pédagogique à afficher")]
    [TextArea(3, 5)]
    public string messageObstacle = "Obstacle détecté.";

    [Header("Texte UI à mettre à jour")]
    public TMP_Text messageUI;

    [Header("Couleur du message d'obstacle (rouge)")]
    public Color couleurObstacle = new Color(1f, 0.4f, 0.4f);

    [Header("Message qui revient quand on quitte l'obstacle")]
    [TextArea(2, 3)]
    public string messageOriginal = "Mission : rejoignez l'hôpital par un chemin accessible.";

    [Header("Couleur du message original")]
    public Color couleurOriginale = Color.white;

    private AdaptiveManager manager;

    void Start()
    {
        manager = FindFirstObjectByType<AdaptiveManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        AfficherMessage(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        AfficherMessage(other.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) RestaurerMessage();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) RestaurerMessage();
    }

    private void AfficherMessage(GameObject objetTouche)
    {
        if (GoalTrigger.missionTerminee) return;

        if (objetTouche.CompareTag("Player"))
        {
            if (messageUI != null)
            {
                messageUI.text = messageObstacle;
                messageUI.color = couleurObstacle;
            }

            if (manager != null)
            {
                manager.SignalerCollision();
            }

            Debug.Log("Obstacle pédagogique touché : " + gameObject.name);
        }
    }

    private void RestaurerMessage()
    {
        if (GoalTrigger.missionTerminee) return;

        if (messageUI != null)
        {
            messageUI.text = messageOriginal;
            messageUI.color = couleurOriginale;
        }
    }
}