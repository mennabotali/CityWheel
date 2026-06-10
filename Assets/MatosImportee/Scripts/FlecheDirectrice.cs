using UnityEngine;

public class FlecheDirectrice : MonoBehaviour
{
      [Header("Inclinaison vers le bas (pour mieux voir la direction)")]
    public float inclinaison = 22f;

    [Header("Le joueur (le fauteuil) à suivre")]
    public Transform joueur;

    [Header("La cible vers laquelle pointer (l'hôpital)")]
    public Transform hopital;

    [Header("Hauteur de la flèche au-dessus du joueur")]
    public float hauteur = 2.5f;

    void LateUpdate()
    {
        if (joueur == null || hopital == null) return;

        // 1. La flèche flotte au-dessus du fauteuil
        transform.position = joueur.position + Vector3.up * hauteur;

        // 2. Elle pointe vers l'hôpital, en restant horizontale
        Vector3 cibleAplatie = new Vector3(hopital.position.x, transform.position.y, hopital.position.z);
        transform.LookAt(cibleAplatie);
        transform.Rotate(inclinaison, 0f, 0f, Space.Self); // penche la flèche pour qu'on voie le dessus
    }
}