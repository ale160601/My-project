using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VigaGiratoria : MonoBehaviour
{
    public float velocidadRotacion = 30f;
    public float multiplicadorFuerza = 10f;
    private void Update()
    {
        transform.Rotate(Vector3.up, velocidadRotacion * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision col)
    {
        Rigidbody rb = col.collider.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 direccionImpacto = (col.transform.position - transform.position).normalized;
            float fuerzaImpacto = velocidadRotacion * multiplicadorFuerza;
            rb.AddForce(direccionImpacto * fuerzaImpacto, ForceMode.Impulse);
        }
    }
}
