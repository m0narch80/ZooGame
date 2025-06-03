using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MoonGeneratorSimple : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile normalTile;        // Small moon path tile
    public Tile bigCraterTile;     // Large crater tile (covers multiple grid tiles)
    public int width = 100;
    public int height = 100;
    public int craterSpacing = 6;  // Minimum spacing between big craters
    public int craterSize = 3;     // How many tiles wide the big crater tile covers
    public int craterCount = 10;   // Number of craters to place

    private List<Vector2Int> craterPositions = new List<Vector2Int>();

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        // Fill entire map with normal moon tiles
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), normalTile);
            }
        }

        // Randomly place big crater tiles
        int attempts = 0;
        while (craterPositions.Count < craterCount && attempts < 1000)
        {
            Vector2Int pos = new Vector2Int(Random.Range(0, width - craterSize), Random.Range(0, height - craterSize));
            if (IsFarFromOtherCraters(pos))
            {
                craterPositions.Add(pos);
                PlaceBigCrater(pos);
            }
            attempts++;
        }

        Debug.Log("Moon map generated with " + craterPositions.Count + " big craters.");
    }

    bool IsFarFromOtherCraters(Vector2Int pos)
    {
        foreach (var p in craterPositions)
        {
            if (Vector2Int.Distance(p, pos) < craterSpacing)
                return false;
        }
        return true;
    }

    void PlaceBigCrater(Vector2Int pos)
    {
        for (int x = 0; x < craterSize; x++)
        {
            for (int y = 0; y < craterSize; y++)
            {
                tilemap.SetTile(new Vector3Int(pos.x + x, pos.y + y, 0), bigCraterTile);
            }
        }
    }
}
