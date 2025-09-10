using UnityEngine;

public class CameraShakeChild : MonoBehaviour
{
    public static CameraShakeChild I;

    public float defaultDuration = 0.12f;
    public float defaultStrength = 0.25f;

    Vector3 baseLocalPos;
    float t;
    float curStrength;

    void Awake()
    {
        I = this;
        baseLocalPos = transform.localPosition;
    }

    void LateUpdate()
    {
        if (t > 0f)
        {
            t -= Time.unscaledDeltaTime;
            Vector2 jitter = Random.insideUnitCircle * curStrength;
            transform.localPosition = baseLocalPos + new Vector3(jitter.x, jitter.y, 0f);

            if (t <= 0f)
            {
                transform.localPosition = baseLocalPos;
            }
        }
    }

    public void Shake()
    {
        t = defaultDuration;
        curStrength = defaultStrength;
    }

    public void ShakeCustom(float duration, float strength)
    {
        t = duration;
        curStrength = strength;
    }
}
