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

    public void SaveData()
    {

    }

    public void LoadData()
    {
        int key = GetLastLvl;
        
    }
    public void SaveLastNumber (int indexScene)
    {
        if (indexScene != 0)
        {
            PlayerPrefs.SetInt("lastLvl", indexScene);
        }
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
        string origString = GetLevels;
        levels = string.Empty;
        for (int i = 1; i < saveString.Length; i++)
        {
            if( i < origString.Length && (saveString[i] == 'A' || origString[i] == 'A'))
            {

            }
        }
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
            return PlayerPrefs.GetString("levels");
        }
    }
}
