using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidadMovimiento = 5f;     // Velocidad de movimiento del personaje
    public float fuerzaSalto = 7f;             // Fuerza del salto
    public Transform camara;                   // Referencia a la cámara en la escena

    public float sensibilidadCamara = 100f;    // Sensibilidad de rotación de la cámara
    public float distanciaCamara = 5f;         // Distancia de la cámara al personaje
    public float alturaCamara = 2f;            // Altura de la cámara sobre el personaje

    public float gravedadAdicional = 10f;      // Fuerza adicional para simular una mayor gravedad
    public LayerMask Plataformas;            // Capa para identificar las plataformas

    private Rigidbody rb;                      // Componente Rigidbody del personaje
    private bool enElSuelo;                    // Verifica si el personaje está en el suelo
    private float rotacionCamaraX = 0f;        // Rotación en el eje X (horizontal)
    private float rotacionCamaraY = 0f;        // Rotación en el eje Y (vertical)

    private void Start()
    {
        // Obtener la referencia al componente Rigidbody
        rb = GetComponent<Rigidbody>();

        // Bloquear el cursor para que no se vea ni se salga de la ventana
        Cursor.lockState = CursorLockMode.Locked;

        // Restringir la rotación del Rigidbody
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // Movimiento del personaje
        MoverPersonaje();

        // Controlar la cámara en tercera persona
        ControlarCamara();
    }

    private void FixedUpdate()
    {
        // Aplicar gravedad adicional
        AplicarGravedadAdicional();
    }

    private void MoverPersonaje()
    {
        // Movimiento horizontal y vertical usando las teclas WASD o flechas
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        float movimientoVertical = Input.GetAxis("Vertical");

        // Dirección de movimiento basada en la cámara
        Vector3 direccionMovimiento = camara.forward * movimientoVertical + camara.right * movimientoHorizontal;
        direccionMovimiento.y = 0f; // Eliminar cualquier movimiento vertical

        // Mover al personaje en la dirección actual
        if (direccionMovimiento.magnitude >= 0.1f)
        {
            // Aplicar movimiento
            rb.MovePosition(transform.position + direccionMovimiento.normalized * velocidadMovimiento * Time.deltaTime);
        }

        // Salto
        if (Input.GetButtonDown("Jump") && enElSuelo)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }
    }

    private void ControlarCamara()
    {
        // Obtener la rotación de la cámara basada en el ratón
        rotacionCamaraX += Input.GetAxis("Mouse X") * sensibilidadCamara * Time.deltaTime;
        rotacionCamaraY -= Input.GetAxis("Mouse Y") * sensibilidadCamara * Time.deltaTime;

        // Limitar la rotación vertical para que no se voltee completamente
        rotacionCamaraY = Mathf.Clamp(rotacionCamaraY, -40f, 85f);

        // Rotar la cámara alrededor del personaje
        Quaternion rotacionCamara = Quaternion.Euler(rotacionCamaraY, rotacionCamaraX, 0f);
        Vector3 posicionCamara = transform.position - (rotacionCamara * Vector3.forward * distanciaCamara) + Vector3.up * alturaCamara;

        // Mover la cámara a la nueva posición
        camara.position = posicionCamara;

        // Hacer que la cámara mire siempre al personaje
        camara.LookAt(transform.position + Vector3.up * alturaCamara);
    }

    private void AplicarGravedadAdicional()
    {
        // Aplicar una fuerza adicional hacia abajo para simular una mayor gravedad
        rb.AddForce(Physics.gravity * rb.mass * gravedadAdicional * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    private void OnCollisionStay(Collision collision)
    {
        // Verificar si el jugador está en contacto con plataformas
        if (Plataformas == (Plataformas | (1 << collision.gameObject.layer)))
        {
            enElSuelo = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Verificar si el jugador ha salido del contacto con plataformas
        if (Plataformas == (Plataformas | (1 << collision.gameObject.layer)))
        {
            enElSuelo = false;
        }
    }
}