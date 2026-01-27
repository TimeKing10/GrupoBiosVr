using UnityEngine;

public class MaterialTitilante : MonoBehaviour
{
    public Renderer rend;
    public Color colorEmision = Color.green;
    public float velocidad = 2f;

    private Material mat;

    void Start()
    {
        if (rend == null)
            rend = GetComponent<Renderer>();

        mat = rend.material; // instancia segura
        mat.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        float intensidad = Mathf.PingPong(Time.time * velocidad, 1f);
        mat.SetColor("_EmissionColor", colorEmision * intensidad);
    }
}
