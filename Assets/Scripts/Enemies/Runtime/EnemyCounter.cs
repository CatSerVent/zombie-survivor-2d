using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 현재 살아있는 Enemy 수를 추적.
/// </summary>
[DisallowMultipleComponent]
public sealed class EnemyCounter : MonoBehaviour
{
    public static EnemyCounter I;

    private HashSet<Enemy> enemies = new HashSet<Enemy>();

    void Awake() => I = this;

    public int Alive => enemies.Count;

    public void Add(Enemy e)
    {
        if (e != null) enemies.Add(e);
    }

    public void Remove(Enemy e)
    {
        if (e != null) enemies.Remove(e);
    }
}
