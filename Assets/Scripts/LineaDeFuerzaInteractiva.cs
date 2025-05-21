using UnityEngine;
using TMPro;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class LineaDeFuerzaInteractiva : MonoBehaviour
{
    private float fuerza;
    private TextMeshPro textoFuerza;
    private GameObject textoGO;

    public void Inicializar(float valorFuerza)
    {
        fuerza = valorFuerza;

        // Crear el objeto de texto solo si aún no existe
        if (textoGO == null)
        {
            textoGO = new GameObject("TextoFuerza");
            textoGO.transform.SetParent(transform);
            textoGO.transform.localPosition = Vector3.zero;

            textoFuerza = textoGO.AddComponent<TextMeshPro>();
            textoFuerza.fontSize = 3;
            textoFuerza.alignment = TextAlignmentOptions.Center;
            textoFuerza.color = Color.black;
        }

        textoFuerza.text = "";
        textoFuerza.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                bool visible = textoFuerza.gameObject.activeSelf;
                textoFuerza.text = "F = " + fuerza.ToString("F2") + " N";
                textoFuerza.gameObject.SetActive(!visible);
            }
        }
    }
}
