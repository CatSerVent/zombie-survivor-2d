// Assets/Scripts/Debug/DebugSpawner.cs
using UnityEngine;
public class DebugSpawner : MonoBehaviour
{
    public GameObject enemyPrefab; public Transform player; public int count = 50; public float radius = 8f;
    [ContextMenu("Spawn Enemies")]
    void Spawn()
    {
        for (int i = 0; i < count; i++)
        {
            var r = Random.insideUnitCircle.normalized * radius;
            Instantiate(enemyPrefab, player.position + new Vector3(r.x, r.y, 0f), Quaternion.identity);
        }
    }
}
