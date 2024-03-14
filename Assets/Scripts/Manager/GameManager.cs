using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager: MonoBehaviour
{
    [SerializeField] bool isGoalCollect;
    [SerializeField] bool isWin;
    [SerializeField] List<Transform> targets = new List<Transform>();
    [SerializeField] List<Transform> boxList = new List<Transform>();
    [SerializeField] bool isPaused;
    [SerializeField] Transform playerHolder;
    UIManager uiManager;
    SaveManager saveManager;
    SoundManager soundManager;
    #region Синглтон
    public static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
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
       
    }
    private void Start()
    {
        uiManager = UIManager.Instance;
        saveManager = SaveManager.Instance;
        soundManager = SoundManager.Instance;
        soundManager.PlayRandomSong();
        StartCoroutine(WaitingEndLoading());
    }
    IEnumerator WaitingEndLoading()
    {
        while (true)
        {
            if (uiManager.EndLoadScene == null)
            {
                playerHolder.gameObject.SetActive(true);
                soundManager.PlayerSpawnSound();
                if (isGoalCollect)
                {
                    if (targets.Count > 0)
                    {
                        uiManager.PrepareFruitPanel(targets.Count);
                    }
                    else
                    {
                        uiManager.PrepareFruitPanel("???");
                    }
                }
                else
                {
                    uiManager.PrepareTimerPanel();
                }
                break;
            }
            yield return null;
        }
    }
    IEnumerator WaitingEndPreparingToLoadScene(bool flag)
    {

        while (!uiManager.StartLoadScene.Ready)
        {
            yield return null;
        }
        if (flag)
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                saveManager.SaveLastNumber(nextSceneIndex);
                saveManager.SaveLevels();
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            saveManager.SaveLastNumber(SceneManager.GetActiveScene().buildIndex);
            saveManager.SaveLevels();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void RestartLevel()
    {
        uiManager.PrepareToLoad();
        
        
        StartCoroutine(WaitingEndPreparingToLoadScene(false));
    }
    public void WasCollected(Transform t)
    {
        if (targets.Contains(t))
        {
            targets.Remove(t);
            uiManager.ChangeCounter(targets.Count);
            saveManager.IncreaseFruitCounter(1);
        }
        if (targets.Count <= 0 && isGoalCollect && boxList.Count <= 0)
        {
            IsWin = true;
        }
        else if (targets.Count <= 0 && isGoalCollect && boxList.Count > 0)
        {
            uiManager.ChangeCounter("???");
        }
    }
    public void WinByFinish()
    {
        IsWin = true;
    }
    public void NeedToCollect(Transform t)
    {
        if (targets.Contains(t))
        {
            return;
        }
        else
        {
            targets.Add(t);
            uiManager.ChangeCounter(targets.Count);
        }
    }
    public void AddBox(Transform t)
    {
        if (!boxList.Contains(t))
        {
            boxList.Add(t);
        }
    }
    public void RemoveBox(Transform t)
    {
        if (boxList.Contains(t))
        {
            boxList.Remove(t);
        }
    }
    #region Аксессоры
    public bool IsWin
    {
        get { return isWin; }
        private set
        {
            isWin = value;
            uiManager.PrepareToLoad();
            soundManager.PlayVictory();
            StartCoroutine(WaitingEndPreparingToLoadScene(true));

        }
    }
    public Transform PlayerTransform
    {
        get { return playerHolder.GetChild(0); }
    }
    public bool IsPaused
    {
        get { return isPaused; }
        set
        {
            isPaused = value;
            if (isPaused)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }
    #endregion
}
