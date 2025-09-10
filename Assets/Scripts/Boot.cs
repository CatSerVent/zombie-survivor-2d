// Assets/Scripts/Boot.cs
using UnityEngine;
public class Boot : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }
}
