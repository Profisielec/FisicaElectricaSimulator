using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricParticle : MonoBehaviour
{
    public float charge = 1.0f;
    public float mass = 1.0f;
    public bool isStatic = false;
    public float maxForce = 50.0f;
    public float boundaryRadius = 50.0f;
    public float minDistance = 0.5f;

    private Rigidbody rb;
    private static List<ElectricParticle> allParticles = new List<ElectricParticle>();
    public static float coulombConstant = 8.99f * Mathf.Pow(10, 9) * 0.000001f;

    public bool showField = false;
    private LineRenderer fieldLine;
    private int fieldLinePoints = 10;
    private float fieldLineLength = 5.0f;

    public bool showForceGizmo = true;
    private Vector3 debugForce;

    public GameObject vectorArrowPrefab;
    private GameObject vectorArrowInstance;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.useGravity = false;
        rb.drag = 0.5f;
        rb.angularDrag = 0.5f;
        rb.isKinematic = isStatic;
        rb.mass = mass;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.maxAngularVelocity = 7.0f;

        UpdateParticleColor();
        allParticles.Add(this);

        if (showField)
        {
            SetupFieldLines();
        }

        if (vectorArrowPrefab != null)
        {
            vectorArrowInstance = Instantiate(vectorArrowPrefab, transform.position, Quaternion.identity, transform);
            UpdateArrowColor();
        }
    }

    void UpdateParticleColor()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            if (charge > 0)
                renderer.material.color = new Color(1, 0, 0, 1);
            else if (charge < 0)
                renderer.material.color = new Color(0, 0, 1, 1);
            else
                renderer.material.color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
    }

    void SetupFieldLines()
    {
        fieldLine = gameObject.AddComponent<LineRenderer>();
        fieldLine.positionCount = fieldLinePoints;
        fieldLine.startWidth = 0.05f;
        fieldLine.endWidth = 0.01f;

        if (charge > 0)
            fieldLine.startColor = fieldLine.endColor = new Color(1, 0.5f, 0.5f, 0.5f);
        else
            fieldLine.startColor = fieldLine.endColor = new Color(0.5f, 0.5f, 1, 0.5f);
    }

    void OnDestroy()
    {
        allParticles.Remove(this);
    }

    void FixedUpdate()
    {
        ApplyElectricForces();  // Siempre se calcula

        if (!isStatic)
        {
            rb.AddForce(debugForce);
        }

        if (showField)
        {
            UpdateFieldLines();
        }

        UpdateForceVector(debugForce);  // Siempre se actualiza
    }

    void ApplyElectricForces()
    {
        Vector3 totalForce = Vector3.zero;

        foreach (ElectricParticle otherParticle in allParticles)
        {
            if (otherParticle == this)
                continue;

            Vector3 direction = otherParticle.transform.position - transform.position;
            float distance = direction.magnitude;

            if (distance < minDistance)
                distance = minDistance;

            Vector3 normalizedDirection = direction.normalized;
            float chargeProduct = charge * otherParticle.charge;
            float forceMagnitude = coulombConstant * Mathf.Abs(chargeProduct) / ((distance * distance) + 1);
            Vector3 force;

            if (chargeProduct < 0)
                force = normalizedDirection * forceMagnitude;
            else
                force = -normalizedDirection * forceMagnitude;

            totalForce += force;
        }

        debugForce = totalForce;
    }

    public static Vector3 CalculateElectricField(Vector3 point)
    {
        Vector3 fieldVector = Vector3.zero;

        foreach (ElectricParticle particle in allParticles)
        {
            Vector3 direction = point - particle.transform.position;
            float distance = direction.magnitude;

            if (distance < 0.1f)
                distance = 0.1f;

            float fieldMagnitude = coulombConstant * Mathf.Abs(particle.charge) / (distance * distance);
            Vector3 fieldContribution = direction.normalized * fieldMagnitude;

            if (particle.charge > 0)
                fieldVector += fieldContribution;
            else
                fieldVector -= fieldContribution;
        }

        return fieldVector;
    }

    void UpdateFieldLines()
    {
        if (fieldLine == null)
            return;

        Vector3[] positions = new Vector3[fieldLinePoints];
        Vector3 currentPos = transform.position;
        Vector3 fieldDirection = (charge > 0) ? Vector3.up : -Vector3.up;

        for (int i = 0; i < fieldLinePoints; i++)
        {
            positions[i] = currentPos;
            Vector3 electricField = CalculateElectricField(currentPos);

            if (electricField.magnitude < 0.01f)
                electricField = fieldDirection * 0.01f;

            fieldDirection = electricField.normalized;

            if (charge < 0)
                fieldDirection = -fieldDirection;

            currentPos += fieldDirection * (fieldLineLength / fieldLinePoints);
        }

        fieldLine.SetPositions(positions);
    }

    public void SetCharge(float newCharge)
    {
        charge = newCharge;
        UpdateParticleColor();
        UpdateArrowColor();

        if (showField && fieldLine != null)
        {
            if (charge > 0)
                fieldLine.startColor = fieldLine.endColor = new Color(1, 0.5f, 0.5f, 0.5f);
            else
                fieldLine.startColor = fieldLine.endColor = new Color(0.5f, 0.5f, 1, 0.5f);
        }
    }

    private void OnDrawGizmos()
    {
        if (showForceGizmo && Application.isPlaying)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, debugForce.normalized * 2);

#if UNITY_EDITOR
            UnityEditor.Handles.Label(transform.position + debugForce.normalized * 2.2f,
                debugForce.magnitude.ToString("F2"));
#endif
        }
    }

    void UpdateForceVector(Vector3 force)
    {
        if (vectorArrowInstance == null) return;

        float magnitude = force.magnitude;

        if (magnitude < 0.01f)
        {
            vectorArrowInstance.SetActive(false);
            return;
        }

        vectorArrowInstance.SetActive(true);

        Vector3 direction = force.normalized;

        vectorArrowInstance.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);

        float sphereRadius = 1f;
        vectorArrowInstance.transform.position = transform.position + direction * sphereRadius;

        float scaleFactor = 0.2f;
        vectorArrowInstance.transform.localScale = new Vector3(
            0.1f,
            0.2f,
            0.1f
        );
    }

    void UpdateArrowColor()
    {
        if (vectorArrowInstance == null) return;

        Renderer renderer = vectorArrowInstance.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            if (charge > 0)
                renderer.material.color = Color.red;
            else
                renderer.material.color = Color.blue;
        }
    }

    
    public Vector3 GetForce()
    {
        return debugForce;
    }
}
