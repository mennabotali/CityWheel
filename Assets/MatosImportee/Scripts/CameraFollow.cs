using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Cible à suivre")]
    public Transform cible;

    [Header("Offset par rapport à la cible")]
    public Vector3 offset = new Vector3(0, 10, -10);

    [Header("Réglage de fluidité")]
    public float vitesseSuiveur = 5f;

    void LateUpdate()
    {
        if (cible == null) return;

        Vector3 positionVoulue = cible.position + offset;
        transform.position = Vector3.Lerp(transform.position, positionVoulue, vitesseSuiveur * Time.deltaTime);

        transform.LookAt(cible);
    }
}