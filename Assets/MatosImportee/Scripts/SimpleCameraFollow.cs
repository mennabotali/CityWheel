using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    [Header("Cible à suivre")]
    public Transform cible;                 // glisser PlayerObject ici

    [Header("Position de la caméra")]
    public Vector3 offset = new Vector3(0f, 2.5f, -4f); // hauteur + recul derrière la cible
    public float hauteurRegard = 1f;        // on regarde un peu au-dessus du sol

    [Header("Lissage (plus grand = plus rapide)")]
    public float vitesseSuivi = 8f;
    public float vitesseRotation = 8f;

    void LateUpdate()
    {
        if (cible == null) return;

        // L'offset tourne avec la cible (autour de Y) -> caméra toujours dans le dos
        Quaternion orientationY = Quaternion.Euler(0f, cible.eulerAngles.y, 0f);
        Vector3 positionVoulue = cible.position + orientationY * offset;
        transform.position = Vector3.Lerp(transform.position, positionVoulue, vitesseSuivi * Time.deltaTime);

        // La caméra regarde la cible (un peu au-dessus du sol)
        Vector3 pointVise = cible.position + Vector3.up * hauteurRegard;
        Quaternion rotationVoulue = Quaternion.LookRotation(pointVise - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationVoulue, vitesseRotation * Time.deltaTime);
    }
}