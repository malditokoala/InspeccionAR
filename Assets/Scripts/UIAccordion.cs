using UnityEngine;

public class UIAccordion : MonoBehaviour
{
    // Referencia al panel completo que queremos mostrar/ocultar.
    public GameObject panelToToggle;

    // Esta funci�n se llamar� con el clic del bot�n.
    public void TogglePanel()
    {
        // Muestra o esconde el panel, invirtiendo su estado actual.
        if (panelToToggle != null)
        {
            bool isActive = panelToToggle.activeSelf;
            panelToToggle.SetActive(!isActive);
        }
    }
}
