using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private float rotacionCamaraX = 0f;
    private float rotacionCamaraY = 0f;
    public float alturaLimiteInferior = -10f;

    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        MoverPersonaje();
        ControlarCamara();
        VerificarAltura();

        if (Input.GetMouseButtonDown(0) && canDash)
        {
            DashForward();
        }


        float velocidadVertical = rb.velocity.y;
        animator.SetBool("isJumping", velocidadVertical > 0.1f);
        animator.SetBool("isFalling", velocidadVertical < -0.1f);
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

            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (Input.GetButtonDown("Jump") && rb.velocity.y <= 0.1f)
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

    private void DashForward()
    {
        Vector3 dashDirection = transform.forward;
        rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);
        StartCoroutine(DashCooldownRoutine());
    }

    IEnumerator DashCooldownRoutine()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void VerificarAltura()
    {
        float alturaUltimaCapa = -5f;

        if (transform.position.y < alturaUltimaCapa)
        {
            Perder();
        }
    }

    private void Perder()
    {
        Debug.Log("has perdido X.x");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //reinicia la escena actual
    }
}