using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    [SerializeField] private Material materialEncendido;
    [SerializeField] private Material materialApagado;
    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        Apagar(); // Al inicio está apagado
    }

    public void Prender()
    {
        if (rend != null && materialEncendido != null)
            rend.sharedMaterial = materialEncendido; // ✅ usar sharedMaterial
    }

    public void Apagar()
    {
        if (rend != null && materialApagado != null)
            rend.sharedMaterial = materialApagado; // ✅ usar sharedMaterial
    }
}
