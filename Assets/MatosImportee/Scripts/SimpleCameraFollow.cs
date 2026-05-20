using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    [Header("Cible à suivre")]
    public Transform cible;

    [Header("Décalage caméra")]
    public Vector3 offset = new Vector3(0f, 4f, -7f);

    [Header("Hauteur du regard")]
    public float hauteurRegard = 1f;

    void LateUpdate()
    {
        if (cible == null)
        {
            return;
        }

        transform.position = cible.position + offset;
        transform.LookAt(cible.position + Vector3.up * hauteurRegard);
    }
}