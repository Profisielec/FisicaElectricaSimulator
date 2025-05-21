using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemGenerator : MonoBehaviour
{
    public GameObject particlePrefab;
    public int numPositiveParticles = 5;
    public int numNegativeParticles = 5;
    public float spawnRadius = 10.0f;
    public float minInitialDistance = 3.0f;

    void Start()
    {
        if (particlePrefab == null)
        {
            Debug.LogError("Necesitas asignar un prefab de partícula!");
        }
    }

    public void EmpezarGeneracion()
    {
        StartCoroutine(SpawnParticlesWithDelay());
    }

    IEnumerator SpawnParticlesWithDelay()
    {
        List<Vector3> usedPositions = new List<Vector3>();

        int total = numPositiveParticles + numNegativeParticles;
        int countPos = 0;
        int countNeg = 0;

        for (int i = 0; i < total; i++)
        {
            bool crearPositiva = false;

            // Alternar positiva y negativa si quedan disponibles
            if ((i % 2 == 0 && countPos < numPositiveParticles) || countNeg >= numNegativeParticles)
                crearPositiva = true;

            Vector3 randomPos = GetRandomPositionWithMinDistance(usedPositions);
            usedPositions.Add(randomPos);

            GameObject particle = Instantiate(particlePrefab, randomPos, Quaternion.identity);
            ElectricParticle ep = particle.GetComponent<ElectricParticle>();
            if (ep == null)
                ep = particle.AddComponent<ElectricParticle>();

            if (crearPositiva)
            {
                ep.charge = Random.Range(0.5f, 1.5f);
                particle.name = "Positive_" + countPos;
                countPos++;
            }
            else
            {
                ep.charge = Random.Range(-1.5f, -0.5f);
                particle.name = "Negative_" + countNeg;
                countNeg++;
            }

            ep.mass = Random.Range(1.0f, 3.0f);

            float scale = 0.5f + (ep.mass / 3.0f);
            particle.transform.localScale = new Vector3(scale, scale, scale);

            yield return new WaitForSeconds(0.1f);
        }
    }

    Vector3 GetRandomPositionWithMinDistance(List<Vector3> existingPositions)
    {
        Vector3 randomPos;
        bool validPosition = false;
        int maxAttempts = 30;
        int attempts = 0;

        do
        {
            randomPos = Random.insideUnitSphere * spawnRadius;
            validPosition = true;

            foreach (Vector3 pos in existingPositions)
            {
                if (Vector3.Distance(randomPos, pos) < minInitialDistance)
                {
                    validPosition = false;
                    break;
                }
            }

            attempts++;

        } while (!validPosition && attempts < maxAttempts);

        return randomPos;
    }
}
