using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TextMeshPro textMeshA;
    public TextMeshPro textMeshB;

    public TileData tileData;

    public void SetValues(int valueA, int valueB)
    {
        tileData = new(valueA, valueB);

        if (textMeshA == null)
        {
            textMeshA = GetComponentInChildren<TextMeshPro>(); // Asume que es el primer hijo
        }
        if (textMeshB == null)
        {
            textMeshB = GetComponentsInChildren<TextMeshPro>()[1]; // Asume que el segundo hijo es el otro
        }
        UpdateText(); // Actualiza el texto después de asignar los valores
    }

    private void UpdateText()
    {
        if (textMeshA != null)
            textMeshA.text = tileData.valueA.ToString();

        if (textMeshB != null)
            textMeshB.text = tileData.valueB.ToString();
    }

}