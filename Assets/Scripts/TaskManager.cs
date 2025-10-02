using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    // Una referencia al GameObject (el Panel de la tarea) que queremos ocultar.
    public GameObject taskPanel;
    // Referencia al texto de la tarea, para tacharlo (opcional, para mejor feedback).
    public TextMeshProUGUI taskText;

    // Esta funci�n se llamar� cuando se presione el bot�n "Hecho".
    public void CompleteTask()
    {
        // 1. Ocultar el panel de la tarea.
        if (taskPanel != null)
        {
            taskPanel.SetActive(false);
            Debug.Log("Tarea completada: " + taskText.text);
        }

        // 2. (Opcional) Implementar la l�gica de la ventana emergente/popup aqu�.
        // Podr�as mostrar un panel diferente que dice: "�Est�s seguro? S�/No".
        // Por ahora, simplemente la ocultamos.
    }
}
