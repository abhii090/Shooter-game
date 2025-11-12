using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;   // Background Music Source
    public AudioSource sfxSource;   // Sound Effects Source

    [Header("Audio Clips - BGM")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    [Header("Audio Clips - SFX")]
    public AudioClip buttonClickSFX;
    public AudioClip shootSFX;
    public AudioClip playerDeathSFX;
    public AudioClip enemyDeathSFX;
    public AudioClip winSFX;

    private bool bgmEnabled = true;
    private bool sfxEnabled = true;

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Ensure AudioSources exist
        if (bgmSource == null)
            bgmSource = gameObject.AddComponent<AudioSource>();
        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();

        // Load saved preferences
        bgmEnabled = PlayerPrefs.GetInt("BGMEnabled", 1) == 1;
        sfxEnabled = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;
    }

    // 🎵 ===== BACKGROUND MUSIC =====
    public void PlayMenuMusic()
    {
        if (!bgmEnabled || menuMusic == null) return;
        if (bgmSource.clip == menuMusic && bgmSource.isPlaying) return;

        bgmSource.clip = menuMusic;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlayGameMusic()
    {
        if (!bgmEnabled || gameMusic == null) return;
        if (bgmSource.clip == gameMusic && bgmSource.isPlaying) return;

        bgmSource.clip = gameMusic;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopMusic()
    {
        if (bgmSource != null)
            bgmSource.Stop();
    }

    // 🔊 ===== SOUND TOGGLES =====
    public bool IsBGMEnabled() => bgmEnabled;
    public bool IsSFXEnabled() => sfxEnabled;

    public void SetBGMEnabled(bool enabled)
    {
        bgmEnabled = enabled;
        PlayerPrefs.SetInt("BGMEnabled", enabled ? 1 : 0);
        PlayerPrefs.Save();

        if (!enabled)
            bgmSource.Stop();
        else if (bgmSource.clip != null)
            bgmSource.Play();
    }

    public void SetSFXEnabled(bool enabled)
    {
        sfxEnabled = enabled;
        PlayerPrefs.SetInt("SFXEnabled", enabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    // 🎮 ===== SOUND EFFECTS =====
    public void PlayButtonClick()
    {
        if (sfxEnabled && buttonClickSFX != null)
            sfxSource.PlayOneShot(buttonClickSFX);
    }

    public void PlayShoot()
    {
        if (sfxEnabled && shootSFX != null)
            sfxSource.PlayOneShot(shootSFX);
    }

    public void PlayPlayerDeath()
    {
        if (sfxEnabled && playerDeathSFX != null)
            sfxSource.PlayOneShot(playerDeathSFX);
    }

    public void PlayEnemyDeath()
    {
        if (sfxEnabled && enemyDeathSFX != null)
            sfxSource.PlayOneShot(enemyDeathSFX);
    }

    public void PlayWinSound()
    {
        if (sfxEnabled && winSFX != null)
            sfxSource.PlayOneShot(winSFX);
    }

    // 🧩 Generic fallback for any SFX
    public void PlaySFX(AudioClip clip)
    {
        if (sfxEnabled && clip != null)
            sfxSource.PlayOneShot(clip);
    }
}
