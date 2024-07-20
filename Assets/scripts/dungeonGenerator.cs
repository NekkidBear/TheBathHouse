// DungeonGenerator.cs
using UnityEngine;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    public int width = 50;
    public int height = 50;
    public int roomSizeMin = 3;
    public int roomSizeMax = 10;
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public int maxEnemiesPerFloor = 10;
    public int maxItemsPerFloor = 5;
    public int maxGoodNPCsPerFloor = 2;
    public GameObject[] enemyPrefabs;
    public GameObject[] itemPrefabs;
    public GameObject[] buffItemPrefabs;
    public GameObject[] debuffItemPrefabs;
    public GameObject[] weaponPrefabs;
    public GameObject[] treasurePrefabs;
    public GameObject goodNPCPrefab;
    public GameObject mapScrollPrefab;

    [System.NonSerialized]
    public bool[,] dungeon;

    void Start()
    {
        GenerateDungeon();
        PlaceEntities();
        PlaceMapScroll();
    }

    void GenerateDungeon()
    {
        dungeon = new bool[width, height];
        Vector2Int startPos = new Vector2Int(width / 2, height / 2);
        DepthFirstSearch(startPos);
        CreateDungeonMesh();
    }

    void DepthFirstSearch(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height)
            return;

        dungeon[pos.x, pos.y] = true;

        List<Vector2Int> directions = new List<Vector2Int>
        {
            new Vector2Int(0, 1), new Vector2Int(1, 0),
            new Vector2Int(0, -1), new Vector2Int(-1, 0)
        };
        directions = ShuffleList(directions);

        foreach (Vector2Int dir in directions)
        {
            Vector2Int newPos = pos + dir;
            if (newPos.x >= 0 && newPos.x < width && newPos.y >= 0 && newPos.y < height && !dungeon[newPos.x, newPos.y])
            {
                DepthFirstSearch(newPos);
            }
        }
    }

    List<T> ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
        return list;
    }

    void CreateDungeonMesh()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (dungeon[x, y])
                {
                    Instantiate(floorPrefab, new Vector3(x, 0, y), Quaternion.identity);
                }
                else
                {
                    Instantiate(wallPrefab, new Vector3(x, 1, y), Quaternion.identity);
                }
            }
        }
    }

    void PlaceEntities()
    {
        List<Vector2Int> emptyTiles = GetEmptyTiles();

        // Place enemies
        for (int i = 0; i < maxEnemiesPerFloor; i++)
        {
            if (emptyTiles.Count > 0)
            {
                Vector2Int pos = emptyTiles[Random.Range(0, emptyTiles.Count)];
                GameObject enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                Instantiate(enemy, new Vector3(pos.x, 1, pos.y), Quaternion.identity);
                emptyTiles.Remove(pos);
            }
        }

        // Place items (including buffs, debuffs, weapons, and treasures)
        for (int i = 0; i < maxItemsPerFloor; i++)
        {
            if (emptyTiles.Count > 0)
            {
                Vector2Int pos = emptyTiles[Random.Range(0, emptyTiles.Count)];
                GameObject item = GetRandomItem();
                Instantiate(item, new Vector3(pos.x, 0.5f, pos.y), Quaternion.identity);
                emptyTiles.Remove(pos);
            }
        }

        // Place good NPCs
        for (int i = 0; i < maxGoodNPCsPerFloor; i++)
        {
            if (emptyTiles.Count > 0)
            {
                Vector2Int pos = emptyTiles[Random.Range(0, emptyTiles.Count)];
                Instantiate(goodNPCPrefab, new Vector3(pos.x, 1, pos.y), Quaternion.identity);
                emptyTiles.Remove(pos);
            }
        }
    }

    GameObject GetRandomItem()
    {
        int itemType = Random.Range(0, 5);
        switch (itemType)
        {
            case 0: return buffItemPrefabs[Random.Range(0, buffItemPrefabs.Length)];
            case 1: return debuffItemPrefabs[Random.Range(0, debuffItemPrefabs.Length)];
            case 2: return weaponPrefabs[Random.Range(0, weaponPrefabs.Length)];
            case 3: return treasurePrefabs[Random.Range(0, treasurePrefabs.Length)];
            default: return itemPrefabs[Random.Range(0, itemPrefabs.Length)];
        }
    }

    List<Vector2Int> GetEmptyTiles()
    {
        List<Vector2Int> emptyTiles = new List<Vector2Int>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (dungeon[x, y])
                {
                    emptyTiles.Add(new Vector2Int(x, y));
                }
            }
        }
        return emptyTiles;
    }

    void PlaceMapScroll()
    {
        List<Vector2Int> emptyTiles = GetEmptyTiles();
        if (emptyTiles.Count > 0)
        {
            Vector2Int scrollPosition = emptyTiles[Random.Range(0, emptyTiles.Count)];
            Instantiate(mapScrollPrefab, new Vector3(scrollPosition.x, 0.5f, scrollPosition.y), Quaternion.identity);
        }
    }
}