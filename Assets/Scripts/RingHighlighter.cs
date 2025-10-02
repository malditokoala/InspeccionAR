using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(LineRenderer))]
public class RingHighlighter : MonoBehaviour
{
    [Header("Destino")]
    public Transform target;

    [Header("Geometría (a refDistance)")]
    public float radius = 0.02f;                // radio base (metros)
    public Vector2 ellipseScale = Vector2.one;  // (1,1)=círculo
    [Range(12, 256)] public int segments = 64;
    public float yOffset = 0.0f;

    [Header("Orientación")]
    public bool billboardToCamera = true;
    public Camera overrideCamera;               // opcional (si tu ARCamera no tiene la tag MainCamera)

    [Header("Apariencia")]
    public Material material;                   // opaco
    public Color color = Color.red;
    public float width = 0.01f;                 // grosor base (a refDistance)

    [Header("Auto-escala por distancia")]
    public bool scaleWithDistance = true;
    public float refDistance = 0.20f;           // 20 cm
    public float minMul = 0.5f;
    public float maxMul = 4f;

    [Header("Efectos (opcionales)")]
    public bool pulse = false;
    public float pulseSpeed = 2.0f;
    [Range(0f, 1f)] public float pulseAmount = 0.35f;

    LineRenderer lr;
    Camera cam;
    float baseRadius, baseWidth;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        cam = overrideCamera ? overrideCamera : Camera.main;

        lr.useWorldSpace = true;
        lr.alignment = LineAlignment.View;
        lr.textureMode = LineTextureMode.Stretch;
        RebuildPositionsArray();

        EnsureOpaqueMaterial();
        ApplyColor(color);

        baseRadius = radius;
        baseWidth = width;
    }

    void OnValidate()
    {
        if (!lr) lr = GetComponent<LineRenderer>();
        RebuildPositionsArray();
    }

    void RebuildPositionsArray()
    {
        if (!lr) return;
        lr.positionCount = Mathf.Max(3, segments + 1); // cerrar el círculo
    }

    void LateUpdate()
    {
        if (!target) { lr.enabled = false; return; }
        lr.enabled = true;

        if (!cam) cam = overrideCamera ? overrideCamera : Camera.main;

        // multiplicador según distancia cámara–anchor
        float mul = 1f;
        if (scaleWithDistance && cam)
        {
            float d = Vector3.Distance(cam.transform.position, target.position);
            float rd = Mathf.Max(0.0001f, refDistance);
            mul = Mathf.Clamp(d / rd, minMul, maxMul);
        }

        float r = baseRadius * mul;
        float w = baseWidth * mul;
        lr.widthMultiplier = pulse ? w * (1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount) : w;

        // billboard
        Quaternion rot = Quaternion.identity;
        if (billboardToCamera && cam)
            rot = Quaternion.LookRotation(cam.transform.forward, Vector3.up);

        Vector3 center = target.position + new Vector3(0, yOffset, 0);

        // dibujar círculo/elipse
        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments * Mathf.PI * 2f;
            float x = Mathf.Cos(t) * r * ellipseScale.x;
            float y = Mathf.Sin(t) * r * ellipseScale.y;
            lr.SetPosition(i, center + rot * new Vector3(x, y, 0));
        }
    }

    void EnsureOpaqueMaterial()
    {
        if (material != null)
        {
            lr.material = material;
            if (lr.material.renderQueue >= 2500) lr.material.renderQueue = 2000; // Geometry
            return;
        }

        bool usingSRP = GraphicsSettings.currentRenderPipeline != null;
        Shader sh = usingSRP ? Shader.Find("Universal Render Pipeline/Unlit")
                             : Shader.Find("Unlit/Color");
        if (!sh) sh = Shader.Find("Standard");
        lr.material = new Material(sh);
        if (lr.material.renderQueue >= 2500) lr.material.renderQueue = 2000;
    }

    void ApplyColor(Color c)
    {
        var m = lr.material; if (!m) return;
        if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", c);
        else if (m.HasProperty("_Color")) m.SetColor("_Color", c);
        else m.color = c;
    }
}
