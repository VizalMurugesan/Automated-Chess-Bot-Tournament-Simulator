//(RangerD, 2025)
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChessTileManager))]
public class ChessTileManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector first (keeps all the public fields)
        DrawDefaultInspector();

        ChessTileManager manager = (ChessTileManager)target;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        // Add a custom button
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontStyle = FontStyle.Bold;
        buttonStyle.fontSize = 12;
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.fixedHeight = 35;

        GUI.backgroundColor = new Color(0.2f, 0.5f, 1f);

        if (GUILayout.Button(" Initialize Board", buttonStyle))
        {
            // Call the method
            manager.InitializeBoard();

            // Mark scene dirty so Unity saves the tilemap changes
            EditorUtility.SetDirty(manager);
            EditorUtility.SetDirty(manager.WhiteTilemap);
            EditorUtility.SetDirty(manager.BlackTilemap);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(manager.gameObject.scene);

            //Debug.Log("Chessboard initialized from custom inspector!");
        }
        EditorGUILayout.Space(5);

        GUI.backgroundColor = new Color(0.1f, 0.7f, 0.2f);
        if (GUILayout.Button("Adjust Pieces", buttonStyle))
        {
            manager.AdjustPieces();
            EditorUtility.SetDirty(manager);
            //Debug.Log("Adjusted all chess pieces to center of grid squares!");
        }
        GUI.backgroundColor = Color.white;


        GUI.backgroundColor = Color.white;
    }
}
