using UnityEngine;

public class BillboardText : MonoBehaviour
{
    private Transform mainCameraTransform;

    void Start()
    {
        // Encuentra la c�mara principal al inicio del juego.
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCameraTransform = mainCamera.transform;
        }
        else
        {
            Debug.LogError("No se encontr� la Main Camera. Aseg�rate de que la c�mara est� tageada.");
            enabled = false;
        }
    }

    // Usamos LateUpdate para asegurar que la rotaci�n ocurre DESPU�S de que la c�mara se ha movido
    // (ya sea la webcam, la c�mara del tel�fono o el controlador FPS).
    void LateUpdate()
    {
        if (mainCameraTransform == null)
            return;

        // Opci�n m�s robusta: Ignora la rotaci�n del texto en sus ejes X y Z, 
        // y solo rota en Y para mirar a la c�mara.
        // Esto previene que el texto se incline o se invierta (patas arriba).

        // 1. Obtener la rotaci�n necesaria para mirar a la c�mara.
        Quaternion lookRotation = Quaternion.LookRotation(transform.position - mainCameraTransform.position);

        // 2. Aplicar solo la rotaci�n Y (el eje vertical).
        float targetYRotation = lookRotation.eulerAngles.y;

        // 3. Aplicar la nueva rotaci�n, manteniendo X y Z en 0 (para que siempre est� plano).
        transform.rotation = Quaternion.Euler(0f, targetYRotation, 0f);
    }
}
