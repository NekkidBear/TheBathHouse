// GoodNPC.cs
using UnityEngine;

public class GoodNPC : MonoBehaviour
{
    public float rescueDistance = 2f;
    public float healthBonus = 20f;
    public float damageBonus = 5f;
    public float speedBonus = 1f;

    private bool isRescued = false;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!isRescued && Vector3.Distance(transform.position, player.position) <= rescueDistance)
        {
            RescueNPC();
        }
    }

    void RescueNPC()
    {
        isRescued = true;
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.IncreaseHealth(healthBonus);
            playerStats.IncreaseDamage(damageBonus);
            playerStats.IncreaseSpeed(speedBonus);
        }
        Debug.Log("NPC rescued! Player stats increased.");
        Destroy(gameObject);
    }
}
