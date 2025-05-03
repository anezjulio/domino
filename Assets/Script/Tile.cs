using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int valueA;
    public int valueB;

    public TextMeshPro textMeshA;
    public TextMeshPro textMeshB;


    public void SetValues(int leftValue, int rightValue)
    {
        valueA = leftValue;
        valueB = rightValue;

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
            textMeshA.text = valueA.ToString();

        if (textMeshB != null)
            textMeshB.text = valueB.ToString();
    }

}