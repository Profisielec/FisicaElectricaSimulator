using System.Collections.Generic;
using UnityEngine;

public class LineaEntreParticulas : MonoBehaviour
{
    public Material lineaMaterial;
    public float grosor = 0.02f;
    public bool mostrarLineas = true;
    public Font textoFont;
    [SerializeField] private Camera mainCamera; // <-- Ahora sí puedes asignarla desde el Inspector

    private List<GameObject> objetosLineas = new List<GameObject>();
    private ElectricParticle[] particulas;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        // Solo actualiza si mostrarLineas es true
        if (!mostrarLineas)
        {
            foreach (var obj in objetosLineas)
                obj.SetActive(false);
            return;
        }

        // Actualiza las líneas cada frame (puedes optimizar más si quieres)
        foreach (var obj in objetosLineas)
            Destroy(obj);
        objetosLineas.Clear();

        particulas = FindObjectsOfType<ElectricParticle>();
        GenerarLineasYTextos();
    }

    void GenerarLineasYTextos()
    {
        for (int i = 0; i < particulas.Length; i++)
        {
            for (int j = i + 1; j < particulas.Length; j++)
            {
                Vector3 pos1 = particulas[i].transform.position;
                Vector3 pos2 = particulas[j].transform.position;
                float distancia = Vector3.Distance(pos1, pos2);

                // Crear objeto con LineRenderer
                GameObject lineaObj = new GameObject("Linea");
                LineRenderer lr = lineaObj.AddComponent<LineRenderer>();
                lr.material = lineaMaterial;
                lr.startWidth = grosor;
                lr.endWidth = grosor;
                lr.positionCount = 2;
                lr.SetPosition(0, pos1);
                lr.SetPosition(1, pos2);
                lr.startColor = Color.white;
                lr.endColor = Color.white;
                lr.useWorldSpace = true;

                objetosLineas.Add(lineaObj);

                // Crear texto en el centro
                GameObject textoObj = new GameObject("TextoDistancia");
                TextMesh texto = textoObj.AddComponent<TextMesh>();
                texto.text = distancia.ToString("F2") + " m";
                texto.characterSize = 0.2f;
                texto.fontSize = 10;
                texto.color = Color.yellow;
                texto.font = textoFont;
                texto.anchor = TextAnchor.MiddleCenter;

                textoObj.transform.position = (pos1 + pos2) / 2;
                textoObj.transform.LookAt(mainCamera.transform);
                textoObj.transform.Rotate(0, 180f, 0);

                objetosLineas.Add(textoObj);
            }
        }
    }

    public void AlternarLineas()
    {
        mostrarLineas = !mostrarLineas;

        foreach (var obj in objetosLineas)
            obj.SetActive(mostrarLineas);
    }
}
