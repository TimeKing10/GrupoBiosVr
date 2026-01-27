using UnityEngine;

public class ActivarGuia : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject objetoGuia;

    [Header("Contador")]
    [SerializeField] private int contador = 0;

    private void Start()
    {
        ActualizarEstado();
    }

    // ➕ Sumar 1 al contador
    public void Sumar()
    {
        contador++;
        ActualizarEstado();
    }

    // ➖ Restar 1 al contador
    public void Restar()
    {
        contador--;

        if (contador < 0)
            contador = 0;

        ActualizarEstado();
    }

    // 🔄 Activa o desactiva el objeto según el contador
    private void ActualizarEstado()
    {
        if (objetoGuia == null)
            return;

        objetoGuia.SetActive(contador == 2);
    }

    // (Opcional) Para debug
    public int GetContador()
    {
        return contador;
    }
}
