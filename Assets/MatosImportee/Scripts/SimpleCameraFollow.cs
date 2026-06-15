using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    [Header("Cible à suivre ")]
    public Transform cible;

    [Header("Position de la caméra")]
    public Vector3 offset = new Vector3(0f, 2.5f, -4f);
    public float hauteurRegard = 1f;

    [Header("Lissage (plus grand = plus rapide)")]
    public float vitesseSuivi = 8f;
    public float vitesseRotation = 8f;

    void LateUpdate()
    {
        if (cible == null) return;

        Quaternion orientationY = Quaternion.Euler(0f, cible.eulerAngles.y, 0f);
        Vector3 positionVoulue = cible.position + orientationY * offset;
        transform.position = Vector3.Lerp(transform.position, positionVoulue, vitesseSuivi * Time.deltaTime);

        Vector3 pointVise = cible.position + Vector3.up * hauteurRegard;
        Quaternion rotationVoulue = Quaternion.LookRotation(pointVise - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationVoulue, vitesseRotation * Time.deltaTime);
    }
}