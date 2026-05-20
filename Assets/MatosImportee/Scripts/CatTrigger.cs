using System.Collections;
using UnityEngine;
using TMPro;

public class CatTrigger : MonoBehaviour
{
    [Header("Texte UI à mettre à jour")]
    public TMP_Text messageUI;

    [Header("Message affiché quand on retrouve le chat")]
    [TextArea(2, 4)]
    public string messageChat = "Vous avez retrouvé votre compagnon ! Continuez votre chemin vers l'hôpital.";

    [Header("Couleur du message")]
    public Color couleurMessage = new Color(1f, 0.8f, 0.3f);

    [Header("Durée d'affichage du message (secondes)")]
    public float dureeMessage = 3f;

    [Header("Message original (revient après le délai)")]
    [TextArea(2, 3)]
    public string messageOriginal = "Mission : rejoignez l'hôpital par un chemin accessible.";
    public Color couleurOriginale = Color.white;

    [Header("Effet visuel (optionnel)")]
    public bool rotationContinue = true;
    public float vitesseRotation = 30f;

    private bool dejaTrouve = false;
    private Coroutine coroutineRetour;

    void Update()
    {
        if (rotationContinue)
        {
            transform.Rotate(Vector3.up, vitesseRotation * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
          if (GoalTrigger.missionTerminee) return;
        if (dejaTrouve) return;

        if (other.CompareTag("Player"))
        {
            dejaTrouve = true;
            Debug.Log("Le joueur a retrouvé le chat !");

            if (messageUI != null)
            {
                messageUI.text = messageChat;
                messageUI.color = couleurMessage;

                if (coroutineRetour != null) StopCoroutine(coroutineRetour);
                coroutineRetour = StartCoroutine(RetourMessageOriginal());
            }
        }
    }

    private IEnumerator RetourMessageOriginal()
    {
        yield return new WaitForSeconds(dureeMessage);
        if (GoalTrigger.missionTerminee) yield break;

        if (messageUI != null)
        {
            messageUI.text = messageOriginal;
            messageUI.color = couleurOriginale;
        }
    }
}