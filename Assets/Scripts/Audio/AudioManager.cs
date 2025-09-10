using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager I;

    public AudioSource sfxSource;
    public AudioClip fireClip;
    public AudioClip levelupClip;

    public AudioSource hitSource;
    public AudioClip hitClip;
    public float hitMinInterval = 0.05f;
    float lastHitTime = -999f;

    public AudioSource deathSource;
    public AudioClip deathClip;
    public float deathMinInterval = 0.05f;
    float lastDeathTime = -999f;

    void Awake()
    {
        if (I == null) I = this;
    }

    public void PlayFire()
    {
        if (sfxSource && fireClip) sfxSource.PlayOneShot(fireClip);
    }

    public void PlayLevelUp()
    {
        if (sfxSource && levelupClip) sfxSource.PlayOneShot(levelupClip);
    }

    public void PlayHitUnique()
    {
        if (!hitSource || !hitClip) return;
        float now = Time.unscaledTime;
        if (hitSource.isPlaying && (now - lastHitTime) < hitMinInterval) return;
        hitSource.clip = hitClip;
        hitSource.Play();
        lastHitTime = now;
    }

    public void PlayEnemyDeathUnique()
    {
        if (!deathSource || !deathClip) return;
        float now = Time.unscaledTime;
        if (deathSource.isPlaying && (now - lastDeathTime) < deathMinInterval) return;
        deathSource.clip = deathClip;
        deathSource.Play();
        lastDeathTime = now;
    }
}
