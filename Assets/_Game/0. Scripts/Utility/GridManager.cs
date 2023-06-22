using UnityEngine;
using UnityEditor;

public class GridManager : MonoBehaviour
{
    public GameObject prefab;
    public int rows = 5;
    public int columns = 5;
    public float spacing = 1f;

    private void CreateGrid()
    {
        //Delete existing objects in the grid
        ClearGrid();

        Vector3 startPosition = transform.position - new Vector3((columns - 1) * spacing / 2f, 0f, (rows - 1) * spacing / 2f);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 spawnPosition = startPosition + new Vector3(col * spacing, 0f, row * spacing);
                GameObject newObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                newObject.transform.position = spawnPosition;
                newObject.transform.parent = transform;
            }
        }
    }

    private void ClearGrid()
    {
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            DestroyImmediate(child);
        }
    }

#if UNITY_EDITOR
    private void DrawButtonWarning()
    {
        if (GUILayout.Button("Create Grid", GUILayout.Height(30)))
        {
            bool userConfirmed = EditorUtility.DisplayDialog("Create Grid", "This will delete the existing objects in the grid. Are you sure you want to proceed?", "Create", "Cancel");
            if (userConfirmed)
            {
                CreateGrid();
            }
        }
        EditorGUILayout.HelpBox("Warning: This will delete the existing objects in the grid.", MessageType.Warning);
    }
#endif
}
