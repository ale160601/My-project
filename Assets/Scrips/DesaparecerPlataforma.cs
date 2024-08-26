using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesaparecerPlataforma : MonoBehaviour
{
    public float tiempoAntesDeDesaparecer = 2f;
    private bool activada = false;
    private Renderer rend;
    private Collider colllider;
    private void Start()
    {
        rend = GetComponent<Renderer>();
        colllider = GetComponent<Collider>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !activada)
        {
            activada = true;
            StartCoroutine(Desaparecer());
        }
    }
    private IEnumerator Desaparecer()
    {
        // Esperar el tiempo antes de desaparecer
        yield return new WaitForSeconds(tiempoAntesDeDesaparecer);
        // Desactivar el Renderer y el Collider para simular desaparición
        rend.enabled = false;
        colllider.enabled = false;
        // Resetear la plataforma para futuras colisiones
        activada = false;
    }
}
