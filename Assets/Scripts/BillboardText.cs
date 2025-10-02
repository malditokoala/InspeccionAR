using UnityEngine;

public class BillboardText : MonoBehaviour
{
    private Transform mainCameraTransform;

    void Start()
    {
        // Encuentra la cámara principal al inicio del juego.
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCameraTransform = mainCamera.transform;
        }
        else
        {
            Debug.LogError("No se encontró la Main Camera. Asegúrate de que la cámara esté tageada.");
            enabled = false;
        }
    }

    // Usamos LateUpdate para asegurar que la rotación ocurre DESPUÉS de que la cámara se ha movido
    // (ya sea la webcam, la cámara del teléfono o el controlador FPS).
    void LateUpdate()
    {
        if (mainCameraTransform == null)
            return;

        // Opción más robusta: Ignora la rotación del texto en sus ejes X y Z, 
        // y solo rota en Y para mirar a la cámara.
        // Esto previene que el texto se incline o se invierta (patas arriba).

        // 1. Obtener la rotación necesaria para mirar a la cámara.
        Quaternion lookRotation = Quaternion.LookRotation(transform.position - mainCameraTransform.position);

        // 2. Aplicar solo la rotación Y (el eje vertical).
        float targetYRotation = lookRotation.eulerAngles.y;

        // 3. Aplicar la nueva rotación, manteniendo X y Z en 0 (para que siempre esté plano).
        transform.rotation = Quaternion.Euler(0f, targetYRotation, 0f);
    }
}
