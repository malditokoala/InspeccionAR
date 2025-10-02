using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    // Una referencia al GameObject (el Panel de la tarea) que queremos ocultar.
    public GameObject taskPanel;
    // Referencia al texto de la tarea, para tacharlo (opcional, para mejor feedback).
    public TextMeshProUGUI taskText;

    // Esta función se llamará cuando se presione el botón "Hecho".
    public void CompleteTask()
    {
        // 1. Ocultar el panel de la tarea.
        if (taskPanel != null)
        {
            taskPanel.SetActive(false);
            Debug.Log("Tarea completada: " + taskText.text);
        }

        // 2. (Opcional) Implementar la lógica de la ventana emergente/popup aquí.
        // Podrías mostrar un panel diferente que dice: "¿Estás seguro? Sí/No".
        // Por ahora, simplemente la ocultamos.
    }
}
