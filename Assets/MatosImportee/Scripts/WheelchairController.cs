using UnityEngine;

public class WheelchairController : MonoBehaviour
{
    [Header("Réglages de mouvement")]
    public float vitesseAvance = 3f;
    public float vitesseRotation = 120f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Aucun Rigidbody trouvé sur PlayerObject !");
        }
    }

    void FixedUpdate()
    {
        float avance = 0f;
        float rotation = 0f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            avance = 1f;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            avance = -1f;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rotation = 1f;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rotation = -1f;
        }

        // Empêche le Rigidbody de garder une rotation parasite
        rb.angularVelocity = Vector3.zero;

        if (rotation != 0f)
        {
            Quaternion rotationDelta = Quaternion.Euler(
                0f,
                rotation * vitesseRotation * Time.fixedDeltaTime,
                0f
            );

            rb.MoveRotation(rb.rotation * rotationDelta);
        }

        if (avance != 0f)
        {
            Vector3 mouvement = transform.forward * avance * vitesseAvance * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + mouvement);
        }
    }
}