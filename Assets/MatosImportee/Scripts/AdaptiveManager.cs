using System.Collections;
using UnityEngine;
using TMPro;

public class AdaptiveManager : MonoBehaviour
{
    [Header("Texte UI pour les messages adaptatifs")]
    public TMP_Text messageUI;

    [Header("Référence au joueur (vide = trouvé via le tag Player)")]
    public Transform joueur;

    [Header("Obstacles ADAPTATIFS secondaires (cachés au départ, révélés si trop facile)")]
    public GameObject[] obstaclesCaches;

    [Header("Obstacles SECONDAIRES retirés si le joueur galère (JAMAIS les pièges)")]
    public GameObject[] obstaclesASupprimer;

    [Header("Réglages adaptativité")]
    public int seuilCollisionsFacile = 3;
    public float tempsAvantAjoutDifficulte = 12f;
    public float delaiEntreApparitions = 1f;
    public float distanceDeRevelation = 12f;
    public int maxObstaclesAdaptatifs = 9;

    [Header("Anti-rebond : ignore les collisions trop rapprochées (1 contact = 1 comptage)")]
    public float delaiAntiDoubleCollision = 0.5f;

    [Header("Durée d'affichage des messages adaptatifs (secondes)")]
    public float dureeMessageAdaptatif = 3f;

    [Header("Message original et couleurs")]
    [TextArea(2, 3)]
    public string messageOriginal = "Mission : rejoignez l'hôpital par un chemin accessible.";
    public Color couleurOriginale = Color.white;
    public Color couleurAdaptation = new Color(1f, 0.7f, 0.2f);

    [Header("Pop-up d'adaptation (COMPRIS)")]
    public AdaptPopupManager popup;

    private int nombreCollisions = 0;
    private float tempsDepuisDerniereCollision = 0f;
    private float tempsDerniereCollisionEnregistree = -999f;
    private bool modeDifficulteActive = false;
    private bool ajoutDifficulteAutorise = true;
    private int nombreReveles = 0;
    private bool facilitationActivee = false;
    private Coroutine chaineCoroutine;
    private Coroutine coroutineMessage;

    public int CollisionsTotales { get { return nombreCollisions; } }

    void Start()
    {
        tempsDepuisDerniereCollision = 0f;
        ajoutDifficulteAutorise = true;
        modeDifficulteActive = false;

        if (joueur == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) joueur = playerObj.transform;
        }

        foreach (GameObject obs in obstaclesCaches)
            if (obs != null) obs.SetActive(false);
    }

    void Update()
    {
        if (GoalTrigger.missionTerminee) return;

        tempsDepuisDerniereCollision += Time.deltaTime;

        if (!modeDifficulteActive
            && ajoutDifficulteAutorise
            && tempsDepuisDerniereCollision >= tempsAvantAjoutDifficulte
            && nombreReveles < maxObstaclesAdaptatifs)
        {
            modeDifficulteActive = true;
            chaineCoroutine = StartCoroutine(ChaineApparitions());
        }
    }

    public void SignalerCollision()
    {
        if (GoalTrigger.missionTerminee) return;

        if (Time.time - tempsDerniereCollisionEnregistree < delaiAntiDoubleCollision) return;
        tempsDerniereCollisionEnregistree = Time.time;

        nombreCollisions++;
        tempsDepuisDerniereCollision = 0f;

        modeDifficulteActive = false;
        if (chaineCoroutine != null) StopCoroutine(chaineCoroutine);

        Debug.Log("Collisions totales : " + nombreCollisions);

        if (nombreCollisions >= seuilCollisionsFacile && !facilitationActivee)
            FaciliterParcours();
    }

    private void FaciliterParcours()
    {
        facilitationActivee = true;
        ajoutDifficulteAutorise = false;
        modeDifficulteActive = false;
        if (chaineCoroutine != null) StopCoroutine(chaineCoroutine);

        foreach (GameObject obs in obstaclesASupprimer)
            if (obs != null) obs.SetActive(false);

        Debug.Log("ADAPTATION : parcours facilité — ajout de difficulté désactivé définitivement");

        if (popup != null)
            popup.Afficher("La ville s'adapte : certains obstacles ont été retirés pour faciliter votre parcours.");
    }

    private IEnumerator ChaineApparitions()
    {
        while (modeDifficulteActive
               && ajoutDifficulteAutorise
               && nombreReveles < maxObstaclesAdaptatifs
               && nombreReveles < obstaclesCaches.Length)
        {
            RevelerProchainObstacleDevant();
            yield return new WaitForSeconds(delaiEntreApparitions);
        }
        modeDifficulteActive = false;
    }

    private void RevelerProchainObstacleDevant()
    {
        if (joueur == null) return;

        GameObject meilleur = null;
        float meilleureDistance = float.MaxValue;
        Vector3 avant = joueur.forward; avant.y = 0f;

        foreach (GameObject obs in obstaclesCaches)
        {
            if (obs == null || obs.activeSelf) continue;

            Vector3 vers = obs.transform.position - joueur.position;
            vers.y = 0f;
            float distance = vers.magnitude;

            if (distance > distanceDeRevelation) continue;
            if (Vector3.Dot(avant.normalized, vers.normalized) <= 0f) continue;

            if (distance < meilleureDistance)
            {
                meilleureDistance = distance;
                meilleur = obs;
            }
        }

        if (meilleur != null)
        {
            meilleur.SetActive(true);
            nombreReveles++;
            Debug.Log("ADAPTATION : obstacle adaptatif révélé — " + meilleur.name);
            AfficherMessageAdaptatif("La ville change : un nouvel obstacle apparaît sur votre chemin.");
        }
    }

    private void AfficherMessageAdaptatif(string texte)
    {
        if (GoalTrigger.missionTerminee || messageUI == null) return;

        messageUI.text = texte;
        messageUI.color = couleurAdaptation;

        if (coroutineMessage != null) StopCoroutine(coroutineMessage);
        coroutineMessage = StartCoroutine(RetourMessageOriginal());
    }

    private IEnumerator RetourMessageOriginal()
    {
        yield return new WaitForSeconds(dureeMessageAdaptatif);
        if (GoalTrigger.missionTerminee) yield break;
        if (messageUI != null)
        {
            messageUI.text = messageOriginal;
            messageUI.color = couleurOriginale;
        }
    }
}