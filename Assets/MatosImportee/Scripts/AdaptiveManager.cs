using System.Collections;
using UnityEngine;
using TMPro;

public class AdaptiveManager : MonoBehaviour
{
    [Header("Texte UI pour les messages adaptatifs")]
    public TMP_Text messageUI;

    [Header("Référence au joueur")]
    public Transform joueur;

    [Header("Obstacles cachés à révéler si trop facile")]
    public GameObject[] obstaclesCaches;

    [Header("Obstacles à masquer si trop difficile")]
    public GameObject[] obstaclesASupprimer;

    [Header("Réglages adaptativité")]
    public int seuilCollisionsFacile = 3;
    public float tempsAvantAjoutDifficulte = 30f;
    public float distanceDeRevelation = 8f;

    [Header("Durée d'affichage des messages adaptatifs (secondes)")]
    public float dureeMessageAdaptatif = 3f;

    [Header("Message original et couleurs")]
    [TextArea(2, 3)]
    public string messageOriginal = "Mission : rejoignez l'hôpital par un chemin accessible.";
    public Color couleurOriginale = Color.white;
    public Color couleurAdaptation = new Color(1f, 0.7f, 0.2f);

    private int nombreCollisions = 0;
    private float tempsDepuisDemarrage = 0f;
    private bool facilitationActivee = false;
    private Coroutine coroutineRetour;

    void Start()
    {
        tempsDepuisDemarrage = 0f;

        if (joueur == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) joueur = playerObj.transform;
        }
    }

    void Update()
    {
        tempsDepuisDemarrage += Time.deltaTime;

        if (tempsDepuisDemarrage > tempsAvantAjoutDifficulte && nombreCollisions == 0)
        {
            EssayerRevelerObstaclesProches();
        }
    }

    public void SignalerCollision()
    {
        nombreCollisions++;
        Debug.Log("Collisions totales : " + nombreCollisions);

        if (nombreCollisions >= seuilCollisionsFacile && !facilitationActivee)
        {
            FaciliterParcours();
        }
    }

    private void FaciliterParcours()
    {
        facilitationActivee = true;
        Debug.Log("ADAPTATION : Parcours facilité — obstacles retirés");

        foreach (GameObject obs in obstaclesASupprimer)
        {
            if (obs != null) obs.SetActive(false);
        }

        AfficherMessageAdaptatif("La ville s'adapte : certains obstacles ont été retirés pour faciliter votre parcours.");
    }

    private void EssayerRevelerObstaclesProches()
    {
        if (joueur == null) return;

        foreach (GameObject obs in obstaclesCaches)
        {
            if (obs != null && !obs.activeSelf)
            {
                float distance = Vector3.Distance(joueur.position, obs.transform.position);

                if (distance <= distanceDeRevelation)
                {
                    obs.SetActive(true);
                    Debug.Log("ADAPTATION : Obstacle révélé à proximité — " + obs.name);

                    AfficherMessageAdaptatif("La ville change : un nouvel obstacle apparaît sur votre chemin.");
                }
            }
        }
    }

    private void AfficherMessageAdaptatif(string texte)
    {
        if (messageUI == null) return;

        messageUI.text = texte;
        messageUI.color = couleurAdaptation;

        if (coroutineRetour != null)
        {
            StopCoroutine(coroutineRetour);
        }

        coroutineRetour = StartCoroutine(RetourMessageOriginal());
    }

    private IEnumerator RetourMessageOriginal()
    {
        yield return new WaitForSeconds(dureeMessageAdaptatif);

        if (messageUI != null)
        {
            messageUI.text = messageOriginal;
            messageUI.color = couleurOriginale;
        }
    }
}