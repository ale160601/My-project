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
        yield return new WaitForSeconds(tiempoAntesDeDesaparecer);
        rend.enabled = false;
        colllider.enabled = false;
        activada = false;
    }
}
