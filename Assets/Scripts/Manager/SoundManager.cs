using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager: MonoBehaviour
{

    [SerializeField] GameObject prefabSound;
    [SerializeField] AudioMixerGroup mainMixer;
    [SerializeField] AudioClip clickClip;
    [SerializeField] List<AudioClip> audioCollectList = new List<AudioClip>();
    [SerializeField] List<AudioClip> audioDeathList = new List<AudioClip>();
    [SerializeField] AudioClip victoryClip;
    [SerializeField] AudioClip mainMenuTheme;
    [SerializeField] List<AudioClip> bgMusicList = new List<AudioClip>();
    [SerializeField] AudioClip playerSpawnClip;
    [SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip enemyStepClip;
    [SerializeField] AudioClip playerStepClip;
    [SerializeField] AudioClip playersDeathClip;
    [SerializeField] AudioClip boxIntectactClip;
    [SerializeField] AudioClip blockInteractClip;
    [SerializeField] AudioClip trampolineInteractClip;
    [SerializeField] AudioClip arrowInteractClip;
    [SerializeField] AudioClip fallingPlatformClip;
    [SerializeField] AudioClip stoneImpactClip;
    [SerializeField] AudioClip attackClip;
    [SerializeField] AudioClip destroyBulletClip;
    [SerializeField] AudioClip birdFlyClip;
    [SerializeField] AudioClip visibleClip, invisibleClip;


    AudioSource bgMusicPlayer;
    SaveManager saveManager;

    #region Синглтон
    public static SoundManager _instance;
    public static SoundManager Instance { get { return _instance; } }
    #endregion
    private void Awake()
    {
        #region Синглтон
        if (Instance != null && Instance != this)
        {

            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        #endregion
        DontDestroyOnLoad(gameObject);
        bgMusicPlayer = GetComponent<AudioSource>();
    }
    private void Start()
    {
        saveManager = SaveManager.Instance;
        if (SceneManager.GetActiveScene().buildIndex == 0 && saveManager.SoundOn)
        {            
            mainMixer.audioMixer.SetFloat("volume", 0);      
            PlayMainMenuTheme();
        }
        else
        {           
            mainMixer.audioMixer.SetFloat("volume", -80);
        }
        
    }


    public void PlaySoundAtPoint(Vector3 point, AudioClip clip)
    {

    }

    public void PlayCLick()
    {
        InstantiateSound(clickClip);
    }
    public void PlayCollect()
    {
        int index = Random.Range(0, audioCollectList.Count);
        InstantiateSound(audioCollectList[index]);
    }
    public void PlayDeath()
    {
        int index = Random.Range(0, audioDeathList.Count);
        InstantiateSound(audioDeathList[index]);
    }
    public void PlayVictory()
    {
        InstantiateSound(victoryClip);
    }
    public void PlayMainMenuTheme()
    {
        if (bgMusicPlayer.clip != mainMenuTheme)
        {
            bgMusicPlayer.clip = mainMenuTheme;
        }
        if (!bgMusicPlayer.isPlaying)
        {
            bgMusicPlayer.Play();
        }
    }
    public void PlayRandomSong()
    {
        bgMusicPlayer.clip = bgMusicList[Random.Range(0, bgMusicList.Count)];
        if (!bgMusicPlayer.isPlaying)
        {
            bgMusicPlayer.Play();
        }
    }
    
    public void PlayJump()
    {
        InstantiateSound(jumpClip);
    }
    #region Enemy
    public void PlayEnemyStep()
    {
        InstantiateSound(enemyStepClip);     
    }
    public void PlayAttack()
    {
        InstantiateSound(attackClip);
    }
    public void PlayDestroyingBullet()
    {
        InstantiateSound(destroyBulletClip);
    }
    public void PlayBirdFly()
    {
        InstantiateSound(birdFlyClip);
    }
    public void SetVisible()
    {
        InstantiateSound(visibleClip);
    }
    public void SetInvisible()
    {
        InstantiateSound(invisibleClip);
    }
    #endregion
    #region traps
    public void PlayBoxInteract()
    {
        InstantiateSound(boxIntectactClip);
    }
    public void PlayBlockInteract()
    {
        InstantiateSound(blockInteractClip);
    }
    public void PlayTrampolineInteract()
    {
        InstantiateSound(trampolineInteractClip);
    }
    public void PlayArrowInteract()
    {
        InstantiateSound(arrowInteractClip);
    }    
    public void PlayFallingPlatform()
    {
        InstantiateSound(fallingPlatformClip);
    }

    public void PlayStoneImpact()
    {
        InstantiateSound(stoneImpactClip);
    }
    #endregion
    #region Player
    public void PlayPlayerStep()
    {
        InstantiateSound(playerStepClip);
    }
    public void PlayerSpawnSound()
    {
        InstantiateSound(playerSpawnClip);
    }
    public void PlayersDeath()
    {
        InstantiateSound(playersDeathClip);
    }
    #endregion
    private void InstantiateSound(AudioClip clip)
    {
        AudioSource audioSource = Instantiate(prefabSound).GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(audioSource.gameObject, clip.length);
    }
    public void ChangeSoundLevel(bool flag)
    {
        if (flag)
        {
            mainMixer.audioMixer.SetFloat("volume", 0);
        }
        else
        {
            mainMixer.audioMixer.SetFloat("volume", -80);
        }
    }

}
