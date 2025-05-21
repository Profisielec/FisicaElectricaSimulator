using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LineaDeFuerzaManager : MonoBehaviour
{
    public GameObject lineaPrefab;
    public float k = 1f;
    public float escalaDistancia = 1000f;

    void Update()
    {
        ParticulaConCarga[] todas = FindObjectsOfType<ParticulaConCarga>();
        List<ParticulaConCarga> particulas = new List<ParticulaConCarga>();

        foreach (var p in todas)
        {
            if (p.gameObject.activeInHierarchy && p.tag != "BaseParticula")
            {
                particulas.Add(p);
            }
        }

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < particulas.Count; i++)
        {
            for (int j = i + 1; j < particulas.Count; j++)
            {
                Transform p1 = particulas[i].transform;
                Transform p2 = particulas[j].transform;

                GameObject linea = Instantiate(lineaPrefab, transform);

                LineRenderer lr = linea.GetComponent<LineRenderer>();
                if (lr != null)
                {
                    Vector3 posA = new Vector3(p1.position.x, p1.position.y, 0);
                    Vector3 posB = new Vector3(p2.position.x, p2.position.y, 0);

                    lr.positionCount = 2;
                    lr.SetPosition(0, posA);
                    lr.SetPosition(1, posB);
                    lr.startWidth = 300f;
                    lr.endWidth = 300f;
                }

                BoxCollider2D col = linea.GetComponent<BoxCollider2D>();
                if (col != null)
                {
                    Vector3 direccion = (p2.position - p1.position).normalized;
                    float longitud = Vector3.Distance(p1.position, p2.position);
                    float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

                    linea.transform.position = (p1.position + p2.position) / 2f;
                    linea.transform.rotation = Quaternion.Euler(0, 0, angulo);
                    col.size = new Vector2(longitud, 200f);
                    col.offset = Vector2.zero;
                    col.isTrigger = true;
                }

                if (linea.GetComponent<Rigidbody2D>() == null)
                {
                    Rigidbody2D rb = linea.AddComponent<Rigidbody2D>();
                    rb.bodyType = RigidbodyType2D.Kinematic;
                    rb.simulated = true;
                    rb.gravityScale = 0f;
                }

                float distanciaReal = Vector3.Distance(p1.position, p2.position);
                float distanciaEscalada = distanciaReal / escalaDistancia;
                float carga1 = particulas[i].carga;
                float carga2 = particulas[j].carga;
                float magnitudFuerza = k * Mathf.Abs(carga1 * carga2) / (distanciaEscalada * distanciaEscalada);

                // Crear texto automáticamente en la línea
                GameObject textoGO = new GameObject("TextoFuerza");
                textoGO.transform.SetParent(linea.transform);
                textoGO.transform.localPosition = Vector3.zero;

                TextMeshPro textoTMP = textoGO.AddComponent<TextMeshPro>();
                textoTMP.fontSize = 3;
                textoTMP.alignment = TextAlignmentOptions.Center;
                textoTMP.color = Color.black;
                textoTMP.text = "F = " + magnitudFuerza.ToString("F2") + " N";
            }
        }
    }
}
