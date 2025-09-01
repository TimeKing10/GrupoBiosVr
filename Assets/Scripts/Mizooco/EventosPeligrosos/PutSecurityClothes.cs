using UnityEngine;

public class PutClothes : MonoBehaviour
{
    [Tooltip("Tipo de prenda que este objeto le da al jugador")]
    [SerializeField] private ClothesMenu.ClothingType clothingType;

    private ClothesMenu clothesMenu;

    private void Awake()
    {
        clothesMenu = FindFirstObjectByType<ClothesMenu>();
        if (clothesMenu == null)
            Debug.LogWarning("No se encontró ClothesMenu en la escena.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Desaparece el objeto recogido
        gameObject.SetActive(false);

        // Activa la RawImage correspondiente
        if (clothesMenu != null)
        {
            clothesMenu.EquipClothing(clothingType);
            Debug.Log($"Prenda equipada: {clothingType}");
        }
    }
}
