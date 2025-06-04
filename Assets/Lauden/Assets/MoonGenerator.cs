using System;
using UnityEngine;

[System.Serializable]
public class TileData
{
    public GameObject prefab;
    [Range(0f, 1f)] public float probability;
}

[System.Serializable]
public class ObjectData
{
    public GameObject prefab;
    [Range(0f, 1f)] public float probability;
}

public class MoonGenerator : MonoBehaviour
{
    private Transform container; // Holds generated tiles and objects for easy cleanup

    public TileData[] bigTiles;
    public TileData[] smallTiles;
    public ObjectData[] smallObjects;

    public void GenerateMoon()
    {
        UnityEngine.Debug.Log("Moon generation started!");

        // Clear previous generation if it exists
        if (container != null)
        {
            Destroy(container.gameObject);
        }

        container = new GameObject("MoonContainer").transform;

        int width = 100;
        int height = 100;

        bool[,] occupied = new bool[width, height];  // Tracks occupied grid cells

        // 1. Place big tiles
        foreach (var tile in bigTiles)
        {
            for (int x = 0; x < width - 3 + 1; x++)  // Adjust 3 for your big tile size
            {
                for (int y = 0; y < height - 3 + 1; y++)
                {
                    if (UnityEngine.Random.value <= tile.probability && CanPlaceBigTile(occupied, x, y, 3, 3))
                    {
                        Vector3 position = new Vector3(x + -0.5f, y + -0.5f, 0);
                        Instantiate(tile.prefab, position, Quaternion.identity, container);
                        MarkOccupied(occupied, x, y, 3, 3);
                    }
                }
            }
        }

        // 2. Fill in small tiles in unoccupied cells
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!occupied[x, y])
                {
                    var tile = SelectTileByProbability(smallTiles);
                    if (tile != null)
                    {
                        // Adjust position based on small tile pivot
                        // If pivot is Center, shift by +0.5f to align grid; if Bottom-Left, no shift
                        Vector3 position = new Vector3(x, y, 0);  // Use (x+0.5f, y+0.5f, 0) if pivot is Center
                        Instantiate(tile.prefab, position, Quaternion.identity, container);
                    }
                }
            }
        }

        // 3. Place small objects individually based on their own probabilities
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!occupied[x, y])
                {
                    foreach (var obj in smallObjects)
                    {
                        if (UnityEngine.Random.value <= obj.probability)
                        {
                            Vector3 position = new Vector3(x, y, -0.1f);
                            Instantiate(obj.prefab, position, Quaternion.identity, container);
                            break; // only place one object per tile
                        }
                    }
                }
            }
        }
    }

    private bool CanPlaceBigTile(bool[,] occupied, int startX, int startY, int sizeX, int sizeY)
    {
        for (int x = startX; x < startX + sizeX; x++)
        {
            for (int y = startY; y < startY + sizeY; y++)
            {
                if (occupied[x, y]) return false;
            }
        }
        return true;
    }

    private void MarkOccupied(bool[,] occupied, int startX, int startY, int sizeX, int sizeY)
    {
        for (int x = startX; x < startX + sizeX; x++)
        {
            for (int y = startY; y < startY + sizeY; y++)
            {
                occupied[x, y] = true;
            }
        }
    }

    private bool IsOccupiedByBigTile(bool[,] occupied, int x, int y)
    {
        return occupied[x, y];
    }

    private TileData SelectTileByProbability(TileData[] tiles)
    {
        float total = 0f;
        foreach (var tile in tiles) total += tile.probability;
        float roll = UnityEngine.Random.value * total;
        float cumulative = 0f;
        foreach (var tile in tiles)
        {
            cumulative += tile.probability;
            if (roll <= cumulative) return tile;
        }
        return null;
    }

    private ObjectData SelectObjectByProbability(ObjectData[] objs)
    {
        float total = 0f;
        foreach (var obj in objs) total += obj.probability;
        float roll = UnityEngine.Random.value * total;
        float cumulative = 0f;
        foreach (var obj in objs)
        {
            cumulative += obj.probability;
            if (roll <= cumulative) return obj;
        }
        return null;
    }
}
