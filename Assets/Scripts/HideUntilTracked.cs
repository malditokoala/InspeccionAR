using System.Collections;
using UnityEngine;
using Vuforia;

[DisallowMultipleComponent]
public class HideUntilTracked : MonoBehaviour
{
    [Header("Observer que emite el estado (ModelTarget)")]
    public ObserverBehaviour observer;     // arrastra tu ModelTargetBehaviour aquí

    [Header("Objetos a mostrar/ocultar")]
    public GameObject[] toToggle;          // ej. Label_Base, Label_Cavidad1, etc.

    [Header("Opciones")]
    public bool startHidden = true;
    public bool showOnExtendedTracked = true;
    public bool showOnLimited = false;     // usar LIMITED como "detección débil"
    public float showDelay = 0.10f;
    public float hideDelay = 0.20f;

    [Header("Debug")]
    public bool verboseLogging = true;     // activa/desactiva logs

    private Coroutine pending;

    void Awake()
    {
        if (observer == null) observer = GetComponentInParent<ObserverBehaviour>();
        if (startHidden) SetAll(false);

        if (observer != null)
        {
            observer.OnTargetStatusChanged += OnTargetStatusChanged;

            if (verboseLogging)
            {
                Debug.Log($"[HideUntilTracked] Observer: {observer.name}");
                Debug.Log($"[HideUntilTracked] Initial Status: {observer.TargetStatus.Status} / Info: {observer.TargetStatus.StatusInfo}");
                Debug.Log($"[HideUntilTracked] Options => startHidden={startHidden}, showOnExtendedTracked={showOnExtendedTracked}, showOnLimited={showOnLimited}, showDelay={showDelay}, hideDelay={hideDelay}");
            }

            UpdateVisibility(observer.TargetStatus);
        }
        else
        {
            Debug.LogWarning("[HideUntilTracked] No se encontró ObserverBehaviour en padres.");
        }
    }

    void OnDestroy()
    {
        if (observer != null)
            observer.OnTargetStatusChanged -= OnTargetStatusChanged;
    }

    void OnTargetStatusChanged(ObserverBehaviour _, TargetStatus status)
    {
        if (verboseLogging)
            Debug.Log($"[HideUntilTracked] OnTargetStatusChanged => Status={status.Status}, Info={status.StatusInfo}");

        UpdateVisibility(status);
    }

    void UpdateVisibility(TargetStatus status)
    {
        bool visible =
            status.Status == Status.TRACKED ||
            (showOnExtendedTracked && status.Status == Status.EXTENDED_TRACKED) ||
            (showOnLimited && status.Status == Status.LIMITED);

        if (verboseLogging)
            Debug.Log($"[HideUntilTracked] UpdateVisibility => visible={visible} for Status={status.Status} (Info={status.StatusInfo})");

        if (pending != null) StopCoroutine(pending);
        pending = StartCoroutine(ApplyAfterDelay(visible ? showDelay : hideDelay, visible));
    }

    IEnumerator ApplyAfterDelay(float delay, bool state)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);
        SetAll(state);
    }

    void SetAll(bool state)
    {
        if (verboseLogging)
        {
            int count = 0;
            foreach (var go in toToggle) if (go) count++;
            Debug.Log($"[HideUntilTracked] SetAll({state}) -> {count} objeto(s)");
        }

        foreach (var go in toToggle)
        {
            if (!go) continue;
            go.SetActive(state);
            if (verboseLogging) Debug.Log($"[HideUntilTracked] {(state ? "SHOW" : "HIDE")} {go.name}");
        }
    }
}
