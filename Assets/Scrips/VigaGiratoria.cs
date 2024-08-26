using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VigaGiratoria : MonoBehaviour
{
    public float velocidadRotacion = 30f; // Velocidad de rotación en grados por segundo
    public float multiplicadorFuerza = 10f; // Multiplicador para ajustar la fuerza del impacto

    private void Update()
    {
        // Rota el objeto alrededor de su propio eje vertical (eje Y)
        transform.Rotate(Vector3.up, velocidadRotacion * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision col)
    {
        // Verifica si el objeto colisionado tiene un Rigidbody
        Rigidbody rb = col.collider.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Calcula la dirección del impacto
            Vector3 direccionImpacto = (col.transform.position - transform.position).normalized;

            // Calcula la fuerza del impacto en función de la velocidad de rotación
            float fuerzaImpacto = velocidadRotacion * multiplicadorFuerza;

            // Aplica la fuerza en la dirección del impacto
            rb.AddForce(direccionImpacto * fuerzaImpacto, ForceMode.Impulse);
        }
    }
}
