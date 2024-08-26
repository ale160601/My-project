using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidadMovimiento = 5f;
    public float fuerzaSalto = 7f;
    public float dashForce = 500f;
    public float dashCooldown = 2f;
    private bool canDash = true;
    public Transform camara;
    public float sensibilidadCamara = 100f;
    public float distanciaCamara = 5f;
    public float alturaCamara = 2f;
    public float gravedadAdicional = 10f;
    public LayerMask Plataformas;
    private Rigidbody rb;
    private bool enElSuelo;
    private float rotacionCamaraX = 0f;
    private float rotacionCamaraY = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        MoverPersonaje();
        ControlarCamara();
        if (Input.GetMouseButtonDown(0))
        {
            DashForward();
        }
    }

    private void FixedUpdate()
    {
        AplicarGravedadAdicional();
    }

    private void MoverPersonaje()
    {
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        float movimientoVertical = Input.GetAxis("Vertical");

        Vector3 direccionMovimiento = camara.forward * movimientoVertical + camara.right * movimientoHorizontal;
        direccionMovimiento.y = 0f;
        if (direccionMovimiento.magnitude >= 0.1f)
        {
            Quaternion rotacionDeseada = Quaternion.LookRotation(direccionMovimiento);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, rotacionDeseada, Time.deltaTime * velocidadMovimiento));
            rb.MovePosition(transform.position + direccionMovimiento.normalized * velocidadMovimiento * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && enElSuelo)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }
    }

    private void ControlarCamara()
    {
        rotacionCamaraX += Input.GetAxis("Mouse X") * sensibilidadCamara * Time.deltaTime;
        rotacionCamaraY -= Input.GetAxis("Mouse Y") * sensibilidadCamara * Time.deltaTime;
        rotacionCamaraY = Mathf.Clamp(rotacionCamaraY, -40f, 85f);
        Quaternion rotacionCamara = Quaternion.Euler(rotacionCamaraY, rotacionCamaraX, 0f);
        Vector3 posicionCamara = transform.position - (rotacionCamara * Vector3.forward * distanciaCamara) + Vector3.up * alturaCamara;
        camara.position = posicionCamara;
        camara.LookAt(transform.position + Vector3.up * alturaCamara);
    }

    private void AplicarGravedadAdicional()
    {
        rb.AddForce(Physics.gravity * rb.mass * gravedadAdicional * Time.fixedDeltaTime, ForceMode.Acceleration);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (Plataformas == (Plataformas | (1 << collision.gameObject.layer)))
        {
            enElSuelo = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (Plataformas == (Plataformas | (1 << collision.gameObject.layer)))
        {
            enElSuelo = false;
        }
    }
    private void DashForward()
    {
        //solo permite hacer dash si no esta en cooldown
        if (canDash)
        {
            Vector3 dashDirection = transform.forward; //usa la direccion hacia adelante del jugador actual
            rb.AddForce(dashDirection * dashForce, ForceMode.Impulse); //aplica la fuerza para el dash
            StartCoroutine(DashCooldownRoutine()); //inicia el cooldown
        }
    }

    IEnumerator DashCooldownRoutine()
    {
        canDash = false; //Deshabilita el dash
        yield return new WaitForSeconds(dashCooldown); //espera el tiempo del cooldownn
        canDash = true; //habilita nuevamente el dash
    }
}