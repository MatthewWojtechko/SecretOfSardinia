using UnityEditor;
using UnityEngine;

public class HeightmapExporter : EditorWindow
{
    [MenuItem("Custom/Export Heightmaps")]
    private static void Init()
    {
        HeightmapExporter window = GetWindow<HeightmapExporter>();
        window.titleContent = new GUIContent("Heightmap Exporter");
        window.Show();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Export Heightmaps"))
        {
            ExportAllHeightmaps();
        }
    }

    private void ExportAllHeightmaps()
    {
        Terrain[] terrains = FindObjectsOfType<Terrain>();

        foreach (Terrain terrain in terrains)
        {
            TerrainData terrainData = terrain.terrainData;
            string exportPath = "Assets/" + terrain.name + "_Heightmap.png";

            float[,] heightmap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

            ExportHeightmapPNG(heightmap, exportPath);

            Debug.Log("Heightmap exported for terrain: " + terrain.name);
        }

        Debug.Log("All heightmaps exported successfully.");
    }

    private void ExportHeightmapPNG(float[,] heightmap, string path)
    {
        int width = heightmap.GetLength(0);
        int height = heightmap.GetLength(1);

        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        Color[] colors = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float heightValue = heightmap[x, y];
                colors[y * width + x] = new Color(heightValue, heightValue, heightValue);
            }
        }

        texture.SetPixels(colors);
        texture.Apply();

        byte[] pngBytes = texture.EncodeToPNG();

        System.IO.File.WriteAllBytes(path, pngBytes);

        DestroyImmediate(texture);
    }
}
