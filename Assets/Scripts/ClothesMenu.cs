using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ClothesMenu : MonoBehaviour
{
    public enum ClothingType
    {
        Helmet,       // Casco
        LeftBoot,     // Bota izquierda
        RightBoot,    // Bota derecha
        Headphones    // Auriculares
    }

    [System.Serializable]
    public class ClothingItemUI
    {
        public ClothingType type;
        public RawImage image; // RawImage en HUD
    }

    [Header("Prendas y sus RawImages")]
    [SerializeField] private List<ClothingItemUI> clothingItemsUI = new List<ClothingItemUI>();

    // Bools individuales
    public bool hasHelmet { get; private set; }
    public bool hasLeftBoot { get; private set; }
    public bool hasRightBoot { get; private set; }
    public bool hasHeadphones { get; private set; }

    private void Awake()
    {
        // Estado inicial: todo false y RawImages ocultas
        hasHelmet = false;
        hasLeftBoot = false;
        hasRightBoot = false;
        hasHeadphones = false;

        foreach (var item in clothingItemsUI)
        {
            if (item.image != null)
                item.image.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Activa la prenda correspondiente y muestra su RawImage
    /// </summary>
    public void EquipClothing(ClothingType type)
    {
        // Actualizamos el bool correspondiente
        switch (type)
        {
            case ClothingType.Helmet:
                hasHelmet = true;
                break;
            case ClothingType.LeftBoot:
                hasLeftBoot = true;
                break;
            case ClothingType.RightBoot:
                hasRightBoot = true;
                break;
            case ClothingType.Headphones:
                hasHeadphones = true;
                break;
        }

        // Actualiza la UI
        var item = clothingItemsUI.Find(i => i.type == type);
        if (item != null && item.image != null)
            item.image.gameObject.SetActive(true);
    }
}
