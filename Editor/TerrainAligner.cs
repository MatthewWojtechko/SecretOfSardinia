using UnityEngine;
using UnityEditor;
using System;

public class TerrainAligner : EditorWindow
{
    Vector3 top    = new Vector3(0, 1, 1);
    Vector3 bottom = new Vector3(0, 1, -1);
    Vector3 left   = new Vector3(-1, 1, 0);
    Vector3 right  = new Vector3(1, 1, 0);

    Vector2 infoScrollPos = Vector2.zero;

    Terrain[] terrains;
    int terrainHeight = 0;
    


    [MenuItem("Custom Utilities/Terrain Aligner")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TerrainAligner));
    }

    private void OnGUI()
    {
        // Display currently selected terrains
        EditorGUILayout.LabelField("Currently Selected Terrains");
        string info = "";
        for (int i = 0; i < Selection.gameObjects.Length && i < 10; i++)
        {
            Debug.Log(i);
            info += getTerrainInfo(Selection.gameObjects[i]) + "\n\n";
        }

        infoScrollPos = EditorGUILayout.BeginScrollView(infoScrollPos, GUILayout.Height(100));
            EditorGUILayout.TextArea(info);
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Align Terrain"))
        {
            if (Selection.gameObjects.Length == 2)
            {
                alignTerrains(Selection.gameObjects[0] == Selection.activeTransform.gameObject ? Selection.gameObjects[1] : Selection.gameObjects[0], Selection.activeTransform.gameObject);
            }
            else
            {
                string issue;
                if (Selection.gameObjects.Length == 0)
                    issue = "You currently don't have anything selected in the scene. Select a terrain you want to move, then also select the terrain you want to move it to.";
                else if (Selection.gameObjects.Length > 2)
                    issue = "You have too many things selected. Only select 2 terrains, the first the terrain you want to move, and the second the one you want to move it to.";
                else
                    issue = "You only have one thing selected. Select two terrains at once. Group select by holding the shift key. Make sure the first terrain is the terrain you want to move and the second the one you want to move it to.";

                EditorUtility.DisplayDialog("Invalid Terrain Selection", issue, "Okay");
            }
        }

        EditorGUILayout.Space(60);

        // Modify Multiple Terrains
        if (setTerrains())
        {
            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Height Scale");
                terrainHeight = EditorGUILayout.IntSlider(terrainHeight, 0, 3000);
                if (GUILayout.Button("Set"))
                {
                    foreach (Terrain T in terrains)
                    {
                        setHeight(T, terrainHeight);
                    }
                }
            EditorGUILayout.EndHorizontal();
        }
    }

    void setHeight(Terrain t, float height)
    {
        Vector3 terrainSize = t.terrainData.size;
        PatchExtents extents = new PatchExtents();
        extents.min = 0;
        extents.max = height;
        t.terrainData.size = new Vector3(terrainSize.x, height, terrainSize.z);
        //t.terrainData.OverrideMinMaxPatchHeights(extents);
    }

    // Go through all game objects in the Selection and find all the terrains. Put them into the terrains array. Return whether any were found.
    bool setTerrains()
    {
        int numTerrains = 0;
        bool hasTerrains = false;
        if (Selection.gameObjects.Length > 0)
        {
            terrains = new Terrain[Selection.gameObjects.Length];
            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                Terrain tempTerrain = Selection.gameObjects[i].GetComponent<Terrain>();
                if (tempTerrain != null)
                {
                    terrains[numTerrains] = tempTerrain;
                    numTerrains++;
                    hasTerrains = true;
                }
            }
        }
        return hasTerrains;
    }

    string getTerrainInfo(GameObject G)
    {
        string result = "";
        if (G != null)
        {
            Terrain T = G.GetComponent<Terrain>();
            result = G.name;

            if (T != null && T.terrainData != null)
            {
                //Debug.Log(result);
                result += "\nSize:   " + T.terrainData.size +
                          "\nPos:    " + T.transform.position +
                          "\nTop:    " + T.topNeighbor?.name +
                          "\nBottom: " + T.bottomNeighbor?.name +
                          "\nLeft:  " + T.leftNeighbor?.name +
                          "\nRight:  " + T.rightNeighbor?.name;
            }
        }
        return result;
    }

    void alignTerrains(GameObject mover, GameObject stationary)
    {
        // Get terrains
        Terrain stationaryTerrain;
        Terrain moverTerrain;
        try
        {
             stationaryTerrain = stationary.GetComponent<Terrain>();
             moverTerrain = mover.GetComponent<Terrain>();
        }
        catch (Exception e)
        {
            EditorUtility.DisplayDialog("Invalid Terrain Selection", "You have selected at least one thing that isn't a terrain.", "Okay");
            return;
        }

        // Calculate the four positions we could potentially move the terrain to.
        Vector3 topPosition = getAdjacentPosition(stationaryTerrain, moverTerrain, top);
        Vector3 bottomPosition = getAdjacentPosition(stationaryTerrain, moverTerrain, bottom);
        Vector3 leftPosition = getAdjacentPosition(stationaryTerrain, moverTerrain, left);
        Vector3 rightPosition = getAdjacentPosition(stationaryTerrain, moverTerrain, right);

        // Find which position is the closest to the terrain we want to move.
        Vector3 moverPosition = moverTerrain.GetPosition();
        Vector3 closestPosition = topPosition;
        float smallestDistance = Mathf.Infinity;
        float tempDistance;

        checkPosition(topPosition);
        checkPosition(leftPosition);
        checkPosition(rightPosition);
        checkPosition(bottomPosition);

        // Move the terrain.
        moverTerrain.transform.position = closestPosition;

        // Update the terrain neighbors for both the terrain we moved and the one we didn't. They should both be neighbors to each other.
        Terrain stationaryNeighborTOP = stationaryTerrain.topNeighbor, stationaryNeighborBOTTOM = stationaryTerrain.bottomNeighbor, stationaryNeighborLEFT = stationaryTerrain.leftNeighbor, stationaryNeighborRIGHT = stationaryTerrain.rightNeighbor;
        Terrain moverNeighborTOP = moverTerrain.topNeighbor, moverNeighborBOTTOM = moverTerrain.bottomNeighbor, moverNeighborLEFT = moverTerrain.leftNeighbor, moverNeighborRIGHT = moverTerrain.rightNeighbor;
        if (closestPosition == topPosition)
        {
            stationaryNeighborTOP = moverTerrain;
            moverNeighborBOTTOM = stationaryTerrain;
        }
        else if (closestPosition == bottomPosition)
        {
            stationaryNeighborBOTTOM = moverTerrain;
            moverNeighborTOP = stationaryTerrain;
        }
        else if (closestPosition == leftPosition)
        {
            stationaryNeighborLEFT = moverTerrain;
            moverNeighborRIGHT = stationaryTerrain;
        }
        else if (closestPosition == right)
        {
            stationaryNeighborRIGHT = moverTerrain;
            moverNeighborLEFT = stationaryTerrain;
        }
        moverTerrain.SetNeighbors(moverNeighborLEFT, moverNeighborTOP, moverNeighborRIGHT, moverNeighborBOTTOM);
        stationaryTerrain.SetNeighbors(stationaryNeighborLEFT, stationaryNeighborTOP, stationaryNeighborRIGHT, stationaryNeighborBOTTOM);

        void checkPosition(Vector3 position)
        {
            tempDistance = Vector3.Distance(moverPosition, position);
            if (tempDistance < smallestDistance)
            {
                smallestDistance = tempDistance;
                closestPosition = position;
            }
        }
    }

    Vector3 getAdjacentPosition(Terrain stationary, Terrain mover, Vector3 direction)
    {
        Vector3 moverDimensions = mover.terrainData.size;

        Vector3 side = new Vector3(moverDimensions.x * direction.x, 0, moverDimensions.z * direction.z);
        return stationary.GetPosition() + side;
        // This assumes that both terrains are the same size!
    }

}
