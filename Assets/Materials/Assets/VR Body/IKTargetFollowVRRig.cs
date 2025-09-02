using UnityEngine;

[System.Serializable]
public class VRMap
{
    public Transform vrTarget;     // Cámara o controladores (entrada)
    public Transform ikTarget;     // Empty que mueve el rig (salida)
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    public float followSpeed = 10f; // velocidad de interpolación

    public void Map()
    {
        if (vrTarget == null || ikTarget == null) return;

        // Calculamos la posición y rotación deseada
        Vector3 targetPosition = vrTarget.TransformPoint(trackingPositionOffset);
        Quaternion targetRotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);

        // Suavizamos el movimiento
        ikTarget.position = Vector3.Lerp(ikTarget.position, targetPosition, Time.deltaTime * followSpeed);
        ikTarget.rotation = Quaternion.Slerp(ikTarget.rotation, targetRotation, Time.deltaTime * followSpeed);
    }
}

public class IKTargetFollowVRRig : MonoBehaviour
{
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    [Header("Head extra offset (backwards from camera)")]
    public Vector3 headOffset = new Vector3(0, 0, -0.1f);

    void LateUpdate()
    {
        if (head != null && head.vrTarget != null && head.ikTarget != null)
        {
            // La cabeza también usa Map, pero sumamos un offset adicional
            Vector3 oldOffset = head.trackingPositionOffset;
            head.trackingPositionOffset += headOffset;
            head.Map();
            head.trackingPositionOffset = oldOffset; // restauramos para no acumular
        }

        // Manos
        if (leftHand != null) leftHand.Map();
        if (rightHand != null) rightHand.Map();
    }
}
