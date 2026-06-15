using UnityEngine;

public class CheminTrigger : MonoBehaviour
{
    [Header("Numéro de ce chemin (1 ou 2)")]
    public int numeroChemin = 1;

    public static int cheminEmprunte = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && cheminEmprunte == 0)
        {
            cheminEmprunte = numeroChemin;
            Debug.Log("Chemin emprunté : " + numeroChemin);
        }
    }
}