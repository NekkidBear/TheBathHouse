// Item.cs
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Buff,
        Debuff,
        Weapon,
        Treasure
    }

    public ItemType type;
    public float value;
    public string itemName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UseItem(other.gameObject);
            Destroy(gameObject);
        }
    }

    void UseItem(GameObject player)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats == null) return;

        switch (type)
        {
            case ItemType.Buff:
                ApplyBuff(playerStats);
                break;
            case ItemType.Debuff:
                ApplyDebuff(playerStats);
                break;
            case ItemType.Weapon:
                EquipWeapon(player);
                break;
            case ItemType.Treasure:
                CollectTreasure(player);
                break;
        }
    }

    void ApplyBuff(PlayerStats playerStats)
    {
        // Implement buff logic here
        Debug.Log($"Applied buff: {itemName}");
    }

    void ApplyDebuff(PlayerStats playerStats)
    {
        // Implement debuff logic here
        Debug.Log($"Applied debuff: {itemName}");
    }

    void EquipWeapon(GameObject player)
    {
        // Implement weapon equipping logic here
        Debug.Log($"Equipped weapon: {itemName}");
    }

    void CollectTreasure(GameObject player)
    {
        // Implement treasure collection logic here
        Debug.Log($"Collected treasure: {itemName}");
    }
}