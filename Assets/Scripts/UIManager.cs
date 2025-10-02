using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Animator (opcional)")]
    public Animator anim;
    public string indexParam = "NextAnim"; // nombre del parámetro entero en tu Animator
    public int currentAnim;                // se sincroniza con el índice del paso

    [Header("Prefabs / Referencias")]
    public CalloutLabel labelPrefab;       // tu prefab Label_Base con CalloutLabel
    public RingHighlighter ringPrefab;     // prefab del anillo resaltador
    public ArrowBillboard arrowPrefab;     // (opcional) prefab de flecha 3D
    public Transform uiParentForLabels;    // Canvas_Callout.transform

    [System.Serializable]
    public class Step
    {
        public string id = "P1";
        [TextArea] public string title = "Título del paso";
        public Transform anchor;                 // ← Anchor_P1, Anchor_P2, etc.
        public Vector2 labelScreenOffset = new Vector2(120, 40);
        public float ringRadius = 0.02f;
        public Vector2 ringEllipse = Vector2.one;
    }

    [Header("Pasos de inspección")]
    public List<Step> steps = new List<Step>();

    // Instancias que se reutilizan
    private CalloutLabel label;
    private RingHighlighter ring;
    private ArrowBillboard arrow;

    // Índice del paso actual
    private int index = -1;

    void Start()
    {
        // Instanciar una vez y reutilizar
        if (labelPrefab) label = Instantiate(labelPrefab, uiParentForLabels ? uiParentForLabels : transform);
        if (ringPrefab) ring = Instantiate(ringPrefab);
        if (arrowPrefab) arrow = Instantiate(arrowPrefab);

        // Ocultar al inicio (si usas HideUntilTracked, se encenderán al trackear)
        if (label) label.gameObject.SetActive(false);
        if (ring) ring.gameObject.SetActive(false);
        if (arrow) arrow.gameObject.SetActive(false);

        if (steps == null || steps.Count == 0)
        {
            Debug.LogWarning("UIManager: no hay pasos configurados.");
            return;
        }

        index = 0;
        ShowStep(index);
    }

    public void Next()
    {
        Debug.Log("Click Next()");
        if (steps.Count == 0) return;
        index = (index + 1) % steps.Count;
        ShowStep(index);
    }

    // Mantengo tu nombre original "Previuos" para no romper tus OnClick ya creados
    public void Previuos()
    {
        if (steps.Count == 0) return;
        index = (index - 1 + steps.Count) % steps.Count;
        ShowStep(index);
    }
    // Alias con ortografía correcta, por si lo prefieres
    public void Previous() => Previuos();

    // ─────────────────────────────────────────────
    // AQUÍ está el método que preguntabas: ShowStep
    // ─────────────────────────────────────────────
    void ShowStep(int i)
    {
        var s = steps[i];
        if (!s.anchor)
        {
            Debug.LogWarning($"UIManager: el paso {i} no tiene anchor asignado.");
            return;
        }

        // 1) Callout (label + línea → anchor)
        if (label)
        {
            label.targetWorld = s.anchor;
            var rect = label.GetComponent<RectTransform>();
            if (rect) rect.anchoredPosition = s.labelScreenOffset;

            // Texto (TMP o UI.Text)
            var tmp = label.GetComponentInChildren<TMP_Text>(true);
            if (tmp) tmp.text = s.title;
            else
            {
                var legacy = label.GetComponentInChildren<Text>(true);
                if (legacy) legacy.text = s.title;
            }

            label.gameObject.SetActive(true);
        }

        // 2) Anillo resaltador
        if (ring)
        {
            ring.target = s.anchor;
            ring.radius = s.ringRadius;
            ring.ellipseScale = s.ringEllipse;
            ring.gameObject.SetActive(true);
        }

        // 3) Flecha 3D (opcional)
        if (arrow)
        {
            arrow.target = s.anchor;
            arrow.gameObject.SetActive(true);
        }

        // 4) (Opcional) sincroniza tu Animator por índice
        currentAnim = i;
        if (anim) anim.SetInteger(indexParam, currentAnim);

        Debug.Log($"UIManager.ShowStep {i + 1}/{steps.Count}: {s.id} - {s.title}");
    }
}
