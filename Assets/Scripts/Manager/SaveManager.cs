using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveManager: MonoBehaviour
{
    #region Синглтон
    public static SaveManager _instance;
    public static SaveManager Instance { get { return _instance; } }
    #endregion

    [SerializeField] int lastLvl = 1;
    [SerializeField] string levels;
    [SerializeField] int fruitCounter;
    [SerializeField] int localization;
    [SerializeField] bool soundOn = false;

    SoundManager soundManager;
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
        LoadData();
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        soundManager = SoundManager.Instance;
    }
    public void SaveData()
    {
        SaveLevels();
    }

    public void LoadData()
    {
        lastLvl = GetLastLvl;
        fruitCounter = GetFruitCounter;
        localization = PlayerPrefs.GetInt("localization");
        soundOn = PlayerPrefs.GetInt("sound")>0? true : false;
        LoadLevels();
    }
    private void LoadLevels()
    {
        levels = PlayerPrefs.GetString("levels");
        string loadLevels = string.Empty;
        for (int i = 0; i < levels.Length; i++)
        {
            if( (i < lastLvl && levels[i] == 'B')|| levels[i] == 'A' )
            {
                loadLevels += 'A';
            }
            else
            {
                loadLevels += 'B';
            }
        }
        if(levels.Length < SceneManager.sceneCountInBuildSettings)
        {
            for (int i = levels.Length; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                loadLevels += 'B';
            }
        }
        levels = loadLevels;
    }

    public void SaveLastNumber (int indexScene)
    {
        if (indexScene != 0)
        {
            PlayerPrefs.SetInt("lastLvl", indexScene);
            lastLvl = indexScene;
        }
    }
    public void IncreaseFruitCounter(int count)
    {
        fruitCounter += count;
        PlayerPrefs.SetInt("fruitCounter", fruitCounter);
    }
    private void SaveSound()
    {
        if (soundOn)
        {
            PlayerPrefs.SetInt("sound", 1);
            
        }
        else
        {
            PlayerPrefs.SetInt("sound", 0);
        }
        soundManager.ChangeSoundLevel(soundOn);
    }
    public void SaveLevels()
    {
        string saveString = string.Empty;
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings;  i++)
        {
            if (i <= lastLvl)
            {
                saveString += "A";
            }
            else
            {
                saveString += "B";
            }
        }
        string origString = levels;
        levels = string.Empty;
        for (int i = 0; i < saveString.Length; i++)
        {
            if(( i < origString.Length && origString[i] == 'A') || saveString[i] == 'A')
            {
                levels += 'A';
            }
            else
            {
                levels += 'B';
            }
        }
        PlayerPrefs.SetString("levels", levels);
    }
    public void ChangeLocalization(int index)
    {
        PlayerPrefs.SetInt("localization", index);
        localization = index;
    }
    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }



    public int GetLastLvl 
    { 
        get 
        {
            int key = PlayerPrefs.GetInt("lastLvl");
            if (key > 1)
            {
                lastLvl = key;
            }
            return lastLvl; 
        } 
    }

    public string GetLevels
    {
        get
        {
            return levels;
        }
    }
    public int GetFruitCounter
    {
        get
        {
            return PlayerPrefs.GetInt("fruitCounter");
        }
    }
    public int GetLocaliztionIndex
    {
        get { return localization; }
    }

    public bool SoundOn
    {
        get { return soundOn; }
        set 
        { 
            soundOn = value;
            SaveSound();
        }
    }
}
