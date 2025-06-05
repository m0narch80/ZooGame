using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
    public GameObject prefab;
    [Range(0f, 1f)] public float probability;
    public int width = 1;  // default for small tiles
    public int height = 1;
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
            int sizeX = tile.width;
            int sizeY = tile.height;

            for (int x = 0; x <= width - sizeX; x++)
            {
                for (int y = 0; y <= height - sizeY; y++)
                {
                    if (UnityEngine.Random.value <= tile.probability &&
                        CanPlaceBigTile(occupied, x, y, sizeX, sizeY))
                    {
                        Vector3 position = new Vector3(x - 0.5f, y - 0.5f, 0); // Adjust as needed
                        Instantiate(tile.prefab, position, Quaternion.identity, container);
                        MarkOccupied(occupied, x, y, sizeX, sizeY);
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

        // 3. Place small objects with random offset within tile, no overlap
        HashSet<Vector2Int> occupiedByObjects = new HashSet<Vector2Int>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int tilePos = new Vector2Int(x, y);

                if (!occupied[x, y] && !occupiedByObjects.Contains(tilePos))
                {
                    foreach (var obj in smallObjects)
                    {
                        if (UnityEngine.Random.value <= obj.probability)
                        {
                            // Randomize position within the tile
                            float offsetX = UnityEngine.Random.Range(0.1f, 0.5f);
                            float offsetY = UnityEngine.Random.Range(0.1f, 0.5f);
                            Vector3 position = new Vector3(x + offsetX, y + offsetY, -0.1f);

                            Instantiate(obj.prefab, position, Quaternion.identity, container);
                            occupiedByObjects.Add(tilePos); // prevent another object here
                            break;
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
