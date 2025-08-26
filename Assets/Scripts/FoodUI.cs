using UnityEngine;
using UnityEngine.UI;

public class FoodUI : MonoBehaviour
{
    [Header("UI")]
    public RawImage displayImage; // Imagen en pantalla

    [Header("Sprites de alimentos (mismo orden que GenerarAlimento.prefabs)")]
    public Sprite[] foodSprites;

    [Header("Referencias")]
    public GenerarAlimento generador;
    public MachineManager machineManager;  // referencia al manager

    private int currentIndex = 0;

    void Start()
    {
        if (generador != null)
            currentIndex = generador.prefabSeleccionado;

        SelectCurrent();
    }

    public void Previous()
    {
        if (generador == null) return;

        currentIndex = (currentIndex - 1 + foodSprites.Length) % foodSprites.Length;
        ApplySelection();
    }

    public void Next()
    {
        if (generador == null) return;

        currentIndex = (currentIndex + 1) % foodSprites.Length;
        ApplySelection();
    }

    private void ApplySelection()
    {
        // Cambiar sprite en UI
        if (displayImage != null && foodSprites.Length > 0)
            displayImage.texture = foodSprites[currentIndex].texture;

        // Avisar al generador
        generador.SeleccionarPrefab(currentIndex);

        // Avisar al MachineManager → activar manualOverride
        if (machineManager != null && !string.IsNullOrEmpty(generador.machineId))
        {
            machineManager.ResetOverrideTimer(generador.machineId);
        }

        Debug.Log($"✅ Manual → seleccionado {foodSprites[currentIndex]?.name}");
    }

    private void SelectCurrent()
    {
        if (foodSprites.Length == 0 || displayImage == null) return;

        displayImage.texture = foodSprites[currentIndex].texture;
    }
}
