using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LeaderLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform textObject;
    public Transform targetPoint;
    // Nueva propiedad: Define el porcentaje del desplazamiento vertical antes del quiebre.
    // 0.7f es el 70% que solicitas.
    public float verticalBreakPercentage = 0.7f;
    [Tooltip("Distancia fija de descenso para indicadores inferiores")]
    public float fixedDropDistance = 0.2f;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        // Seguimos usando 3 puntos para el quiebre de 90 grados.
        lineRenderer.positionCount = 3;

        if (targetPoint == null)
        {
            targetPoint = transform;
        }
    }
    void LateUpdate()
    {
        if (lineRenderer == null || textObject == null || targetPoint == null)
            return;

        RectTransform rectTransform = textObject.GetComponent<RectTransform>();
        if (rectTransform == null) return;

        Vector3 startPos = targetPoint.position;
        Vector3 endPos_Center = textObject.position;

        // PASO 1: Calcular el borde inferior del panel (sin cambios)
        float halfHeight = rectTransform.rect.height * 0.5f;
        Vector3 localOffset = new Vector3(0, -halfHeight, 0);
        Vector3 endPos_Target = rectTransform.TransformPoint(localOffset);

        Vector3 endPos = endPos_Target;

        // PASO 2: CÁLCULO ADAPTATIVO DEL QUIEBRE
        // ============================================================

        float breakY;
        bool isAboveOrigin = endPos_Center.y > startPos.y;

        if (isAboveOrigin)
        {
            // CASO 1: Indicador SUPERIOR - Ascender al nivel del borde inferior
            breakY = endPos_Target.y; // Usar la Y del borde inferior directamente
        }
        else
        {
            // CASO 2: Indicador INFERIOR - Descender distancia fija
            float fixedDropDistance = 0.2f;
            breakY = startPos.y - fixedDropDistance;

            // No bajar más allá del borde inferior del panel
            breakY = Mathf.Max(breakY, endPos_Target.y);
        }

        // Punto 2: Fin del tramo vertical (codo)
        Vector3 point2 = new Vector3(startPos.x, breakY, startPos.z);

        // Punto 3: CRÍTICO - Usar endPos_Target (borde inferior) en lugar de endPos_Center
        Vector3 point3 = new Vector3(endPos_Target.x, breakY, endPos_Target.z);

        // PASO 3: Asignar Posiciones
        lineRenderer.positionCount = 3;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, point2);
        lineRenderer.SetPosition(2, point3); // Ahora llegará exactamente al borde
    }
    // ... código anterior (definiciones) ...
    //void LateUpdate()
    //{
    //    // 1. Verificación de Referencias
    //    if (lineRenderer == null || textObject == null || targetPoint == null)
    //        return;

    //    RectTransform rectTransform = textObject.GetComponent<RectTransform>();
    //    if (rectTransform == null) return;

    //    Vector3 startPos = targetPoint.position;
    //    Vector3 endPos_Center = textObject.position; // Centro del BackgroundPanel

    //    // 1. CÁLCULO DEL BORDE INFERIOR DEL PANEL (Snap Point)
    //    float halfHeight = rectTransform.rect.height * 0.5f;
    //    Vector3 localOffset = new Vector3(0, -halfHeight, 0);
    //    Vector3 endPos_Target = rectTransform.TransformPoint(localOffset);

    //    // Altura Y a la que la línea debe terminar (borde inferior)
    //    float endY_Snap = endPos_Target.y;
    //    float breakY; // Variable para la altura del quiebre (Punto 2)

    //    // 2. CÁLCULO ADAPTATIVO DEL QUIEBRE VERTICAL (breakY)

    //    // Indicador Superior (Texto más alto que el origen)
    //    if (endPos_Center.y >= startPos.y)
    //    {
    //        // La línea sube. El quiebre Y se alinea con el punto de contacto final.
    //        breakY = endY_Snap;
    //    }
    //    // Indicador Inferior (Texto más bajo que el origen)
    //    else
    //    {
    //        // 2.1. Cálculo del desvío fijo (ej. 0.2 unidades abajo del origen)
    //        float fixedDescentY = startPos.y - 0.2f;

    //        // 2.2. Aseguramos que el desvío fijo NUNCA sea superior al punto de contacto.
    //        // Si el punto de contacto (endY_Snap) está más abajo que el desvío fijo, usamos el punto de contacto.
    //        if (endY_Snap < fixedDescentY)
    //        {
    //            breakY = endY_Snap; // El texto está tan bajo que no necesitamos el desvío extra.
    //        }
    //        else
    //        {
    //            breakY = fixedDescentY; // Usamos el desvío fijo (0.2f) para alejarnos de la pieza.
    //        }
    //    }

    //    // 3. DEFINICIÓN DE PUNTOS DE LA LÍNEA (3 Puntos)

    //    // Punto 2: Fin del tramo vertical
    //    Vector3 point2 = new Vector3(startPos.x, breakY, startPos.z);

    //    // Punto 3: El punto de contacto final (Borde Inferior del Panel)
    //    // Usamos X y Z del CENTRO del panel, y la altura FINAL de contacto (endY_Snap).
    //    Vector3 point3 = new Vector3(endPos_Center.x, endY_Snap, endPos_Center.z);

    //    // 4. ASIGNAR POSICIONES FINALES
    //    lineRenderer.positionCount = 3;

    //    lineRenderer.SetPosition(0, startPos);        // P1: Origen (Target 3D)
    //    lineRenderer.SetPosition(1, point2);          // P2: Codo Vertical (Define el desvío)
    //    lineRenderer.SetPosition(2, point3);          // P3: Punto de Contacto (Borde Inferior)
    //}


    //void LateUpdate()
    //{
    //    if (lineRenderer == null || textObject == null || targetPoint == null)
    //        return;

    //    Vector3 startPos = targetPoint.position;
    //    Vector3 endPos = textObject.position;

    //    // =========================================================
    //    // CALCULAR EL PUNTO DE QUIEBRE (Punto 2)
    //    // =========================================================

    //    // 1. Calcular la coordenada Y (vertical) del punto de quiebre (Punto 2).
    //    // Usamos Lerp para obtener un punto al 70% de la distancia vertical entre startPos y endPos.
    //    float breakY = Mathf.Lerp(startPos.y, endPos.y, verticalBreakPercentage);

    //    // 2. Definir la posición del quiebre (Punto 2).
    //    // - X: Misma posición X que el Target Point (startPos.x). Esto crea el primer tramo vertical.
    //    // - Y: La posición Y calculada al 70% (breakY).
    //    // - Z: Misma posición Z que el Target Point (startPos.z). Usamos startPos.z para mantener la línea 'pegada' al objeto si es necesario.
    //    Vector3 point2 = new Vector3(startPos.x, breakY, startPos.z);


    //    // 3. Definir la posición del segundo codo (Punto 3 - que es el final de la línea recta).
    //    // - X: La misma posición X que el Text Object (endPos.x).
    //    // - Y: La posición Y calculada al 70% (breakY).
    //    // - Z: La misma posición Z que el Text Object (endPos.z).
    //    Vector3 point3 = new Vector3(endPos.x, breakY, endPos.z);

    //    // Ahora necesitamos 4 puntos para este tipo de quiebre (Vertical -> Horizontal -> Texto)
    //    lineRenderer.positionCount = 4;

    //    // =========================================================
    //    // 4. ASIGNAR POSICIONES AL LINE RENDERER
    //    // =========================================================

    //    lineRenderer.SetPosition(0, startPos); // Punto 1: Target Point (Inicio Vertical)
    //    lineRenderer.SetPosition(1, point2);   // Punto 2: Fin del tramo vertical
    //    lineRenderer.SetPosition(2, point3);   // Punto 3: Fin del tramo horizontal (alineado con el texto)
    //    lineRenderer.SetPosition(3, endPos);   // Punto 4: Text Object (Etiqueta de Texto)
    //}
}
