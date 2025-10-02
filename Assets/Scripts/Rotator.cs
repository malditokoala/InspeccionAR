using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Define la velocidad de rotación en grados por segundo
    // (Ajusta este valor en el Inspector para controlar la velocidad)
    public float rotationSpeed = 100f;

    // Define sobre qué eje debe rotar el objeto (generalmente el eje Y para una rotación vertical)
    public Vector3 rotationAxis = Vector3.up;

    // Usamos Update para que rote en cada frame
    void Update()
    {
        // Multiplicamos la velocidad por Time.deltaTime para hacerla independiente del framerate.
        // Esto asegura que la rotación sea suave y constante en cualquier dispositivo.
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}
