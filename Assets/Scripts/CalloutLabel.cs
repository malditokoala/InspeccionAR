using UnityEngine;
using UnityEngine.Rendering; // Para GraphicsSettings (detectar URP/Built-in)

[DisallowMultipleComponent]
[RequireComponent(typeof(LineRenderer))]
public class CalloutLabel : MonoBehaviour
{
    [Header("Objetivo en el mundo (ancla)")]
    public Transform targetWorld;

    [Header("Ajustes de línea")]
    [Min(0.0001f)] public float lineWidth = 0.01f;
    public Color lineColor = new Color(1f, 0f, 1f, 1f); // magenta
    public int sortingOrder = 0;                         // Orden de render (no afecta profundidad)
    public Material lineMaterial;                        // opcional: arrastra uno opaco aquí

    private LineRenderer lr;
    private RectTransform rect;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        rect = GetComponent<RectTransform>();

        // Propiedades básicas del LineRenderer
        lr.useWorldSpace = true;
        lr.positionCount = 2;
        lr.widthMultiplier = lineWidth;
        lr.alignment = LineAlignment.View;        // bueno para UI sobre cámara
        lr.textureMode = LineTextureMode.Stretch;
        lr.numCornerVertices = 2;
        lr.numCapVertices = 2;

        // Asegurar material OPACO (con ZWrite On / ZTest LEqual)
        EnsureOpaqueMaterial();

        // Sorting order (no confundir con profundidad)
        var rend = GetComponent<Renderer>();
        if (rend) rend.sortingOrder = sortingOrder;
    }

    void LateUpdate()
    {
        if (!targetWorld) return;

        // En Canvas "Screen Space - Camera", rect.position está en mundo (plano del canvas)
        Vector3 labelWorldPos = rect.position;

        lr.SetPosition(0, labelWorldPos);
        lr.SetPosition(1, targetWorld.position);
    }

    private void EnsureOpaqueMaterial()
    {
        // Si el usuario ya asignó un material opaco, úsalo
        if (lineMaterial != null)
        {
            lr.material = lineMaterial;
            SetMaterialColorSafe(lr.material, lineColor);
            ForceOpaqueQueueIfNeeded(lr.material);
            return;
        }

        // Crear un material opaco automáticamente según el pipeline
        Shader sh = null;
        bool usingSRP = GraphicsSettings.currentRenderPipeline != null;

        if (usingSRP)
        {
            // URP/HDRP: Unlit opaco
            sh = Shader.Find("Universal Render Pipeline/Unlit");
        }
        else
        {
            // Built-in: Unlit/Color opaco
            sh = Shader.Find("Unlit/Color");
        }

        if (sh == null)
        {
            Debug.LogWarning("CalloutLabel: No se encontró un shader Unlit opaco. Asignando material por defecto.");
            lr.material = new Material(Shader.Find("Standard")); // fallback (opaco)
        }
        else
        {
            lr.material = new Material(sh);
        }

        SetMaterialColorSafe(lr.material, lineColor);
        ForceOpaqueQueueIfNeeded(lr.material);
    }

    private void SetMaterialColorSafe(Material m, Color c)
    {
        // Algunas shaders usan _BaseColor (URP), otras _Color (Built-in/Standard)
        if (m == null) return;

        if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", c);
        else if (m.HasProperty("_Color")) m.SetColor("_Color", c);
        else m.color = c; // última opción
    }

    private void ForceOpaqueQueueIfNeeded(Material m)
    {
        if (m == null) return;

        // Si el shader es transparente (cola ~3000) lo forzamos a geometría (~2000)
        // Solo funciona si el shader soporta opaco; con Unlit/Color o URP/Unlit está OK.
        if (m.renderQueue >= 2500) m.renderQueue = 2000; // Geometry
        // En URP, asegúrate que el material esté en "Surface Type = Opaque" si editas a mano.
    }
}
