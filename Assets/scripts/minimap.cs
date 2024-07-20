// MiniMap.cs
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public RawImage miniMapImage;
    public int miniMapSize = 128;
    public Color unexploredColor = Color.black;
    public Color exploredColor = Color.white;
    public Color playerColor = Color.red;
    public Color enemyColor = Color.yellow;
    public Color goodNPCColor = Color.green;
    public Color itemColor = Color.blue;

    private Texture2D miniMapTexture;
    private bool[,] exploredTiles;
    private DungeonGenerator dungeonGenerator;
    private Transform player;

    public bool isEnabled = false;

    void Start()
    {
        dungeonGenerator = FindObjectOfType<DungeonGenerator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        miniMapTexture = new Texture2D(dungeonGenerator.width, dungeonGenerator.height);
        miniMapImage.texture = miniMapTexture;

        exploredTiles = new bool[dungeonGenerator.width, dungeonGenerator.height];

        miniMapImage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isEnabled)
        {
            UpdateMiniMap();
        }
    }

    void UpdateMiniMap()
    {
        for (int x = 0; x < dungeonGenerator.width; x++)
        {
            for (int y = 0; y < dungeonGenerator.height; y++)
            {
                if (exploredTiles[x, y])
                {
                    miniMapTexture.SetPixel(x, y, exploredColor);
                }
                else
                {
                    miniMapTexture.SetPixel(x, y, unexploredColor);
                }
            }
        }

        // Draw player
        int playerX = Mathf.RoundToInt(player.position.x);
        int playerY = Mathf.RoundToInt(player.position.z);
        miniMapTexture.SetPixel(playerX, playerY, playerColor);

        // Draw enemies
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            int enemyX = Mathf.RoundToInt(enemy.transform.position.x);
            int enemyY = Mathf.RoundToInt(enemy.transform.position.z);
            miniMapTexture.SetPixel(enemyX, enemyY, enemyColor);
        }

        // Draw good NPCs
        foreach (var npc in FindObjectsOfType<GoodNPC>())
        {
            int npcX = Mathf.RoundToInt(npc.transform.position.x);
            int npcY = Mathf.RoundToInt(npc.transform.position.z);
            miniMapTexture.SetPixel(npcX, npcY, goodNPCColor);
        }

        // Draw items
        foreach (var item in FindObjectsOfType<Item>())
        {
            int itemX = Mathf.RoundToInt(item.transform.position.x);
            int itemY = Mathf.RoundToInt(item.transform.position.z);
            miniMapTexture.SetPixel(itemX, itemY, itemColor);
        }

        miniMapTexture.Apply();
    }

    public void EnableMiniMap()
    {
        isEnabled = true;
        miniMapImage.gameObject.SetActive(true);
    }
}