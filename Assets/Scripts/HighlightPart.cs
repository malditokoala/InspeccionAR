using UnityEngine;

public class HighlightPart : MonoBehaviour
{
    public Renderer targetRenderer;
    public Color emissionColor = new Color(1f, 0.2f, 0f);
    public float speed = 4f;
    public float intensity = 1.2f;
    Color baseColor;
    bool active;

    void Start()
    {
        if (!targetRenderer) targetRenderer = GetComponentInChildren<Renderer>();
        if (targetRenderer && targetRenderer.material.HasProperty("_EmissionColor"))
        {
            baseColor = targetRenderer.material.GetColor("_EmissionColor");
            DynamicGI.SetEmissive(targetRenderer, baseColor);
        }
    }

    void Update()
    {
        if (!targetRenderer) return;
        var m = targetRenderer.material;
        if (!m.HasProperty("_EmissionColor")) return;

        Color c = active
            ? emissionColor * (0.5f + 0.5f * (1f + Mathf.Sin(Time.time * speed)) * intensity)
            : baseColor;
        m.EnableKeyword("_EMISSION");
        m.SetColor("_EmissionColor", c);
    }

    public void SetActive(bool on) { active = on; }
}
