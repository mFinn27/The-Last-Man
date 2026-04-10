using UnityEngine;
using System.Collections;
[System.Serializable]
public class SoundClip
{
    public AudioClip clip;
    [Range(0f, 2f)] public float volume = 1f;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("--- Audio Sources ---")]
    [SerializeField] private AudioSource sourceNhacNen;
    [SerializeField] private AudioSource sourceHieuUngAmThanh;
    [SerializeField] private AudioSource sourceNhacNenBoss;

    [Header("--- Volume TỔNG ---")]
    [Range(0f, 1f)] public float globalBgmVolume = 1f;
    [Range(0f, 1f)] public float globalSfxVolume = 1f;

    [Header("--- Volume MẶC ĐỊNH ---")]
    [Range(0f, 1f)] public float menuVolume = 1f;
    [Range(0f, 1f)] public float gameplayVolume = 0.4f;
    [Range(0f, 1f)] public float bossVolume = 0.8f;

    [Header("--- Nhạc nền ---")]
    public AudioClip nhacNenMenu;
    public AudioClip nhacNenGameplay;
    public AudioClip nhacNenBoss;

    [Header("--- Âm Lượng Riêng ---")]
    public SoundClip hieuUngCanhBaoBoss;
    public SoundClip hieuUngClick;
    public SoundClip hieuUngNhatCoin;
    public SoundClip hieuUngTrungDonEnemy;
    public SoundClip hieuUngTrungDonPlayer;
    public SoundClip hieuUngMuaDo;
    public SoundClip hieuUngVuNo;
    private float currentBaseBgmVolume = 1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            globalBgmVolume = PlayerPrefs.GetFloat("Saved_BgmVolume", 1f);
            globalSfxVolume = PlayerPrefs.GetFloat("Saved_SfxVolume", 1f);
        }
        else { Destroy(gameObject); }
    }

    public void SetGlobalBGMVolume(float value)
    {
        globalBgmVolume = value;
        PlayerPrefs.SetFloat("Saved_BgmVolume", value);

        if (sourceNhacNen != null)
        {
            sourceNhacNen.volume = currentBaseBgmVolume * globalBgmVolume;
        }
    }

    public void SetGlobalSFXVolume(float value)
    {
        globalSfxVolume = value;
        PlayerPrefs.SetFloat("Saved_SfxVolume", value);
    }

    public void PlayBGM(AudioClip clip, float targetVolume)
    {
        if (clip == null) return;

        currentBaseBgmVolume = targetVolume;
        float finalVolume = targetVolume * globalBgmVolume;

        if (sourceNhacNen.clip == clip)
        {
            sourceNhacNen.volume = finalVolume;
            if (!sourceNhacNen.isPlaying) sourceNhacNen.Play();
            return;
        }

        sourceNhacNen.Stop();
        sourceNhacNen.clip = clip;
        sourceNhacNen.volume = finalVolume;
        sourceNhacNen.loop = true;
        sourceNhacNen.Play();
    }

    public void PlayMenuBGM() => PlayBGM(nhacNenMenu, menuVolume);
    public void PlayGameplayBGM() => PlayBGM(nhacNenGameplay, gameplayVolume);

    public void PlaySFX(SoundClip sound)
    {
        if (sound == null || sound.clip == null) return;

        float finalVolume = globalSfxVolume * sound.volume;
        sourceHieuUngAmThanh.PlayOneShot(sound.clip, finalVolume);
    }

    public void PlayClickSFX() => PlaySFX(hieuUngClick);
    public void PlayCoinSFX() => PlaySFX(hieuUngNhatCoin);
    public void PlayEnemyHitSFX() => PlaySFX(hieuUngTrungDonEnemy);
    public void PlayPlayerHitSFX() => PlaySFX(hieuUngTrungDonPlayer);
    public void PlayEquipSFX() => PlaySFX(hieuUngMuaDo);
    public void PlayExplosionSFX() => PlaySFX(hieuUngVuNo);

    public void TriggerBossWave()
    {
        StartCoroutine(BossAppearanceRoutine());
    }

    private IEnumerator BossAppearanceRoutine()
    {
        sourceNhacNen.Stop();

        if (hieuUngCanhBaoBoss != null && hieuUngCanhBaoBoss.clip != null)
        {
            float warningVol = hieuUngCanhBaoBoss.volume * globalSfxVolume;
            sourceNhacNenBoss.PlayOneShot(hieuUngCanhBaoBoss.clip, warningVol);
            yield return new WaitForSecondsRealtime(hieuUngCanhBaoBoss.clip.length);
        }
        else
        {
            yield return new WaitForSecondsRealtime(2f);
        }

        PlayBGM(nhacNenBoss, bossVolume);
    }

    public float GetBossWarningLength()
    {
        return (hieuUngCanhBaoBoss != null && hieuUngCanhBaoBoss.clip != null) ? hieuUngCanhBaoBoss.clip.length : 2f;
    }

    public void StopAllGameplaySounds()
    {
        StopAllCoroutines();
        if (sourceNhacNenBoss != null) sourceNhacNenBoss.Stop();
    }
}