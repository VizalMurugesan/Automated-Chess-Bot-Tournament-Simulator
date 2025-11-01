using UnityEngine;
using UnityEngine.UI;

public class ChessBoard : MonoBehaviour
{
    public GridLayoutGroup gridLayout; // Assign in Inspector (the parent of all squares)

    void Start()
    {
        // Loop through each child under the GridLayoutGroup
        for (int i = 0; i < gridLayout.transform.childCount; i++)
        {
            Transform square = gridLayout.transform.GetChild(i);
            Debug.Log($"Square {i}: {square.name}");

            // Example: get the Image component
            Image img = square.GetComponent<Image>();
            if (img != null)
            {
                // Example use: color alternation
                img.color = (i + i / 8) % 2 == 0 ? Color.white : Color.gray;
            }
        }
    }


}
