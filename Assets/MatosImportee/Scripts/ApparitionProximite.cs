using System.Collections;
using UnityEngine;
using TMPro;

public class ApparitionProximite : MonoBehaviour
{
    [Header("Distance à laquelle l'obstacle apparaît")]
    public float distanceApparition = 6f;

    [Header("Message à l'apparition (laisser vide pour aucun)")]
    [TextArea(2, 4)]
    public string message = "Ce passage vient de se fermer. Cherchez un autre chemin.";
    public TMP_Text messageUI;
    public Color couleurMessage = new Color(1f, 0.4f, 0.4f);
    public float dureeMessage = 3f;
    [TextArea(2, 3)]
    public string messageOriginal = "Mission : rejoignez l'hôpital par un chemin accessible.";
    public Color couleurOriginale = Color.white;

    private Transform joueur;
    private bool apparu = false;
    private Renderer[] renderers;
    private Collider[] colliders;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) joueur = playerObj.transform;

        renderers = GetComponentsInChildren<Renderer>(true);
        colliders = GetComponentsInChildren<Collider>(true);

        DefinirVisible(false); // caché au départ (invisible + ne bloque pas)
    }

    void Update()
    {
        if (apparu || joueur == null) return;

        if (Vector3.Distance(joueur.position, transform.position) <= distanceApparition)
        {
            Apparaitre();
        }
    }

    private void Apparaitre()
    {
        apparu = true;
        DefinirVisible(true);
        Debug.Log("Obstacle apparu à proximité : " + gameObject.name);

        if (messageUI != null && !string.IsNullOrEmpty(message))
        {
            messageUI.text = message;
            messageUI.color = couleurMessage;
            StartCoroutine(RetourMessage());
        }
    }

    private IEnumerator RetourMessage()
    {
        yield return new WaitForSeconds(dureeMessage);
        if (messageUI != null && !GoalTrigger.missionTerminee)
        {
            messageUI.text = messageOriginal;
            messageUI.color = couleurOriginale;
        }
    }

    private void DefinirVisible(bool visible)
    {
        foreach (Renderer r in renderers) r.enabled = visible;
        foreach (Collider c in colliders) c.enabled = visible;
    }
}