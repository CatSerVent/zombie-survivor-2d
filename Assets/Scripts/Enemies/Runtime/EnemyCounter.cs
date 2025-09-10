using UnityEngine;
using System;

public class EnemyCounter : MonoBehaviour
{
    public static EnemyCounter I;
    public int Alive { get; private set; }
    public Action<int> OnAliveChanged;

    private void Awake() {I = this;Alive = 0;}

    public static void Add()
    {
        if (I == null) return;
        I.Alive++;
        I.OnAliveChanged?.Invoke(I.Alive);
    }

    public static void Remove()
    {
        if(I==null) return;
        I.Alive = Mathf.Max(0, I.Alive - 1);
        I.OnAliveChanged?.Invoke(I.Alive);
    }
}
