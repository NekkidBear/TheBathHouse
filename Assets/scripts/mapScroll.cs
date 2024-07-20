// MapScroll.cs
using UnityEngine;

public class MapScroll : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MiniMap miniMap = FindObjectOfType<MiniMap>();
            if (miniMap != null)
            {
                miniMap.EnableMiniMap();
                Destroy(gameObject);
            }
        }
    }
}
