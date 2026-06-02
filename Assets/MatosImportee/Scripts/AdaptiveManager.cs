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
    public int seuilCollisionsFacile = 3;          // collisions avant de faciliter (et tout arrêter)
    public float tempsAvantAjoutDifficulte = 12f;  // secondes sans collision avant le mode difficulté
    public float delaiEntreApparitions = 1f;       // délai entre 2 apparitions en chaîne
    public float distanceDeRevelation = 12f;       // distance max pour révéler un obstacle devant
    public int maxObstaclesAdaptatifs = 5;          // nombre TOTAL d'obstacles révélables
    public float delaiAntiDoubleCollision = 0.5f;   // ignore les collisions trop rapprochées (1 contact = 1 comptage)

    [Header("Durée d'affichage des messages adaptatifs (secondes)")]
    public float dureeMessageAdaptatif = 3f;

    [Header("Message original et couleurs")]
    [TextArea(2, 3)]
    public string messageOriginal = "Mission : rejoignez l'hôpital par un chemin accessible.";
    public Color couleurOriginale = Color.white;
    public Color couleurAdaptation = new Color(1f, 0.7f, 0.2f);

    // ---------- État interne ----------
    private int nombreCollisions = 0;                        // nb d'obstacles CLASSIQUES touchés
    private float tempsDepuisDerniereCollision = 0f;
    private float tempsDerniereCollisionEnregistree = -999f; // pour l'anti-rebond
    private bool modeDifficulteActive = false;               // la chaîne d'apparitions tourne-t-elle ?
    private bool ajoutDifficulteAutorise = true;             // a-t-on encore le droit d'augmenter la difficulté ?
    private int nombreReveles = 0;                           // nb d'obstacles adaptatifs déjà révélés
    private bool facilitationActivee = false;
    private Coroutine chaineCoroutine;                       // coroutine de la chaîne d'apparitions
    private Coroutine coroutineMessage;                      // coroutine de retour au message d'origine

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

        // Sécurité : tous les obstacles adaptatifs commencent CACHÉS
        foreach (GameObject obs in obstaclesCaches)
            if (obs != null) obs.SetActive(false);
    }

    void Update()
    {
        if (GoalTrigger.missionTerminee) return;

        // Temps écoulé depuis la dernière collision
        tempsDepuisDerniereCollision += Time.deltaTime;

        // --- DÉCLENCHEMENT DU MODE DIFFICULTÉ ---
        // Conditions : pas déjà actif, encore autorisé, assez longtemps sans collision, pas au max
        if (!modeDifficulteActive
            && ajoutDifficulteAutorise
            && tempsDepuisDerniereCollision >= tempsAvantAjoutDifficulte
            && nombreReveles < maxObstaclesAdaptatifs)
        {
            modeDifficulteActive = true;
            chaineCoroutine = StartCoroutine(ChaineApparitions());
        }
    }

    // === Appelé par ObstacleMessage quand le joueur percute un obstacle CLASSIQUE ===
    public void SignalerCollision()
    {
        if (GoalTrigger.missionTerminee) return;

        // ANTI-REBOND : un seul contact peut déclencher OnCollisionEnter plusieurs fois.
        // On ignore les appels trop rapprochés -> 1 vraie collision = 1 seul comptage.
        if (Time.time - tempsDerniereCollisionEnregistree < delaiAntiDoubleCollision) return;
        tempsDerniereCollisionEnregistree = Time.time;

        nombreCollisions++;
        tempsDepuisDerniereCollision = 0f; // RESET : il faudra de nouveau attendre sans collision

        // Collision -> on coupe immédiatement le mode difficulté et la chaîne
        modeDifficulteActive = false;
        if (chaineCoroutine != null) StopCoroutine(chaineCoroutine);

        Debug.Log("Collisions totales : " + nombreCollisions);

        // 3 collisions -> on facilite ET on arrête DÉFINITIVEMENT l'ajout de difficulté
        if (nombreCollisions >= seuilCollisionsFacile && !facilitationActivee)
            FaciliterParcours();
    }

    // === MODE TROP DIFFICILE : on aide le joueur, une seule fois, définitivement ===
    private void FaciliterParcours()
    {
        facilitationActivee = true;
        ajoutDifficulteAutorise = false; // on n'augmentera PLUS JAMAIS la difficulté
        modeDifficulteActive = false;
        if (chaineCoroutine != null) StopCoroutine(chaineCoroutine);

        // On retire les obstacles SECONDAIRES (les pièges ne sont pas dans ce tableau)
        foreach (GameObject obs in obstaclesASupprimer)
            if (obs != null) obs.SetActive(false);

        Debug.Log("ADAPTATION : parcours facilité — ajout de difficulté désactivé définitivement");
        AfficherMessageAdaptatif("La ville s'adapte : certains obstacles ont été retirés pour faciliter votre parcours.");
    }

    // === MODE TROP FACILE : apparition EN CHAÎNE, 1 obstacle toutes les "delaiEntreApparitions" ===
    private IEnumerator ChaineApparitions()
    {
        while (modeDifficulteActive
               && ajoutDifficulteAutorise
               && nombreReveles < maxObstaclesAdaptatifs
               && nombreReveles < obstaclesCaches.Length)
        {
            RevelerProchainObstacleDevant(); // tente de révéler 1 obstacle DEVANT le joueur
            yield return new WaitForSeconds(delaiEntreApparitions);
        }
        modeDifficulteActive = false;
    }

    // Révèle UN obstacle adaptatif, seulement s'il est DEVANT le joueur et proche
    private void RevelerProchainObstacleDevant()
{
    if (joueur == null) return;

    GameObject meilleur = null;
    float meilleureDistance = float.MaxValue;
    Vector3 avant = joueur.forward; avant.y = 0f;

    foreach (GameObject obs in obstaclesCaches)
    {
        if (obs == null || obs.activeSelf) continue; // nul ou déjà révélé

        Vector3 vers = obs.transform.position - joueur.position;
        vers.y = 0f;
        float distance = vers.magnitude;

        if (distance > distanceDeRevelation) continue;                      // trop loin
        if (Vector3.Dot(avant.normalized, vers.normalized) <= 0f) continue; // derrière le joueur

        // On garde le plus PROCHE parmi ceux qui sont devant + à portée
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

    // === Affichage temporaire d'un message adaptatif ===
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