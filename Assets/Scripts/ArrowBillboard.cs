using UnityEngine;

public class ArrowBillboard : MonoBehaviour
{
    [Header("Destino")]
    public Transform target;                    // Anchor del paso

    [Header("Posici�n relativa")]
    public Vector3 localOffset = new Vector3(0f, 0.02f, 0f); // 2 cm sobre el anchor

    [Header("Orientaci�n y animaci�n")]
    public bool faceCamera = true;              // mirar a la c�mara
    public float bobAmplitude = 0.005f;         // �bobbing� (sube/baja) 5 mm
    public float bobSpeed = 2.0f;

    private Camera cam;
    private Vector3 baseOffset;

    void Awake()
    {
        cam = Camera.main;
        baseOffset = localOffset;
    }

    void LateUpdate()
    {
        if (!target) { gameObject.SetActive(false); return; }
        gameObject.SetActive(true);

        // Bobbing (sube/baja suave)
        float bob = Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;
        Vector3 worldPos = target.position + target.TransformVector(baseOffset + new Vector3(0f, bob, 0f));
        transform.position = worldPos;

        // Mirar a la c�mara
        if (faceCamera && cam)
        {
            Vector3 toCam = (cam.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(toCam, Vector3.up);
        }
    }
}
