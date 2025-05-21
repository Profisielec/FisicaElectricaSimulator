using UnityEngine;

public class GizmoUIRotator : MonoBehaviour
{
    public Camera targetCamera;
    public RectTransform ejeX, ejeY, ejeZ;

    void Update()
    {
        if (targetCamera == null) return;

        Quaternion camRot = targetCamera.transform.rotation;

        ejeX.localRotation = Quaternion.Inverse(camRot) * Quaternion.Euler(90, 0, 0);
        ejeY.localRotation = Quaternion.Inverse(camRot) * Quaternion.Euler(0, 0, 90);
        ejeZ.localRotation = Quaternion.Inverse(camRot) * Quaternion.Euler(0, -90, 0);
    }
}
