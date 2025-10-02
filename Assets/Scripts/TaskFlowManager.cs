using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TaskFlowManager : MonoBehaviour
{
    // --- Referencias UI ---
    [Header("UI References")]
    public GameObject currentTaskDisplay;
    public TextMeshProUGUI taskText;
    public Image taskImage;
    public Button completeButton;

    // ¡OPTIMIZACIÓN ESCALABLE para Pointers!
    // Contiene los objetos padre de los pointers para CADA tarea (Índice 0 = Tarea 1, Índice 1 = Tarea 2, etc.)
    [Header("Pointer Containers")]
    public List<GameObject> allPointersContainers;

    // --- Definición de la Tarea (Clase interna) ---
    [System.Serializable]
    public class Task
    {
        public string description;
        public Sprite image;
    }

    // --- La Lista de Tareas ---
    [Header("Task List")]
    public List<Task> tasks = new List<Task>();

    private int currentTaskIndex = 0; // Seguimiento de la tarea actual

    void Start()
    {
        // Asegúrate de que el botón llama a la función para pasar a la siguiente tarea.
        completeButton.onClick.RemoveAllListeners();
        completeButton.onClick.AddListener(CompleteTask);

        Debug.Log("Sistema de Tareas iniciado. Total de tareas cargadas: " + tasks.Count);

        // Opcional: Desactiva todos los contenedores de pointers al inicio para tener una escena limpia.
        SetAllPointersActive(false);

        // Carga la primera tarea al inicio
        LoadCurrentTask();
    }

    // Función de utilidad para activar/desactivar todos los punteros
    private void SetAllPointersActive(bool active)
    {
        if (allPointersContainers != null)
        {
            foreach (GameObject container in allPointersContainers)
            {
                if (container != null)
                {
                    container.SetActive(active);
                }
            }
        }
    }

    // Función principal para cargar la tarea en la UI
    void LoadCurrentTask()
    {
        // ----------------------------------------------------
        // LÓGICA DE CARGA Y VISIBILIDAD DE UI
        // ----------------------------------------------------
        if (currentTaskIndex < tasks.Count)
        {
            // Hay tareas pendientes, ¡mostramos la actual!
            Task currentTask = tasks[currentTaskIndex];

            // 1. Actualiza el Contenido de la UI:
            taskText.text = currentTask.description;
            if (currentTask.image != null)
            {
                taskImage.sprite = currentTask.image;
                taskImage.enabled = true;
            }
            else
            {
                taskImage.enabled = false;
            }
            currentTaskDisplay.SetActive(true);

            // ----------------------------------------------------
            // LÓGICA DE VISIBILIDAD DE POINTERS OPTIMIZADA
            // ----------------------------------------------------

            // 1. Desactiva todos los contenedores para asegurar que solo el correcto esté visible.
            SetAllPointersActive(false);

            // 2. Activa SOLO el contenedor correspondiente a la tarea actual.
            if (allPointersContainers != null && currentTaskIndex < allPointersContainers.Count)
            {
                GameObject currentPointers = allPointersContainers[currentTaskIndex];
                if (currentPointers != null)
                {
                    currentPointers.SetActive(true);
                }
            }
        }
        else
        {
            // Tareas completadas: Oculta el panel principal y desactiva todos los punteros.
            taskText.text = "¡Todas las tareas completadas! 🎉";
            taskImage.enabled = false;
            completeButton.gameObject.SetActive(false);

            // Desactiva cualquier pointer que pudiera haber quedado activo.
            SetAllPointersActive(false);
        }
    }

    // Esta función se llama al presionar el botón "Hecho"
    public void CompleteTask()
    {
        // Avanza al siguiente índice
        currentTaskIndex++;

        Debug.Log("CompleteTask() llamado. Índice NUEVO: " + currentTaskIndex);

        // Carga la siguiente tarea (o finaliza si ya no hay más)
        LoadCurrentTask();
    }
}