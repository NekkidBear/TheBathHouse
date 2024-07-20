// Enemy.cs
using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    public float moveSpeed = 3f;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackDamage = 10f;
    public float attackCooldown = 1f;

    private Transform player;
    private float lastAttackTime;
    private DungeonGenerator dungeonGenerator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        dungeonGenerator = FindObjectOfType<DungeonGenerator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            Vector3 nextPosition = CalculateNextMove();
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);

            if (distanceToPlayer <= attackRange && Time.time > lastAttackTime + attackCooldown)
            {
                Attack();
            }
        }
    }

    Vector3 CalculateNextMove()
    {
        Vector2Int currentPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
        Vector2Int playerPos = new Vector2Int(Mathf.RoundToInt(player.position.x), Mathf.RoundToInt(player.position.z));

        List<Vector2Int> path = AStar(currentPos, playerPos);

        if (path != null && path.Count > 1)
        {
            return new Vector3(path[1].x, transform.position.y, path[1].y);
        }

        return transform.position;
    }

    List<Vector2Int> AStar(Vector2Int start, Vector2Int goal)
    {
        var openSet = new List<Vector2Int> { start };
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var gScore = new Dictionary<Vector2Int, float> { [start] = 0 };
        var fScore = new Dictionary<Vector2Int, float> { [start] = HeuristicCostEstimate(start, goal) };

        while (openSet.Count > 0)
        {
            var current = openSet[0];
            foreach (var node in openSet)
            {
                if (fScore.ContainsKey(node) && fScore[node] < fScore[current])
                {
                    current = node;
                }
            }

            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }

            openSet.Remove(current);

            foreach (var neighbor in GetNeighbors(current))
            {
                var tentativeGScore = gScore[current] + 1;

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    float HeuristicCostEstimate(Vector2Int a, Vector2Int b)
    {
        return Vector2Int.Distance(a, b);
    }

    List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        var path = new List<Vector2Int> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }

    List<Vector2Int> GetNeighbors(Vector2Int pos)
    {
        var neighbors = new List<Vector2Int>();
        var directions = new Vector2Int[] {
            new Vector2Int(0, 1), new Vector2Int(1, 0),
            new Vector2Int(0, -1), new Vector2Int(-1, 0)
        };

        foreach (var dir in directions)
        {
            var newPos = pos + dir;
            if (newPos.x >= 0 && newPos.x < dungeonGenerator.width &&
                newPos.y >= 0 && newPos.y < dungeonGenerator.height &&
                dungeonGenerator.dungeon[newPos.x, newPos.y])
            {
                neighbors.Add(newPos);
            }
        }

        return neighbors;
    }

    void Attack()
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(attackDamage);
        }
        lastAttackTime = Time.time;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Implement death logic here (e.g., drop items, play sound, etc.)
        Destroy(gameObject);
    }
}