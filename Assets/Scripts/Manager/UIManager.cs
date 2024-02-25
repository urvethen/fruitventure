
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager: MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goalText;
    [SerializeField] Transform goalPanel;
    [SerializeField] Transform questPanel;
    [SerializeField] Image pauseButton;
    [SerializeField] List<Sprite> pauseSprite;
    [SerializeField] Image soundButton;
    [SerializeField] List<Sprite> soundSprite;
    [SerializeField] TranslationManager loadScene, endLoadScene;
    [SerializeField] GameObject buttonPanel;
    [SerializeField] GameObject menuButton;
    [SerializeField] List<string> questCollect = new List<string>();
    [SerializeField] List<string> questFinish = new List<string>();
    float timer = 0f;
    GameManager gameManager;
    CameraShake cameraShake;
    #region Синглтон
    public static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }
    #endregion

    private void OnEnable()
    {
        if (!endLoadScene.gameObject.activeSelf)
        {
            endLoadScene.gameObject.SetActive(true);
        }
    }
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
        gameManager = GameManager.Instance;
        cameraShake = CameraShake.Instance;
    }
    private void ShowPanel()
    {
        goalPanel.gameObject.SetActive(true);
        questPanel.gameObject.SetActive(false);
    }
    public void PrepareFruitPanel(int count)
    {
        questPanel.gameObject.SetActive(true);
        questPanel.GetComponentInChildren<TextMeshProUGUI>().text = questCollect[0];
        goalText.text = "Осталось собрать: " + count;
        Invoke("ShowPanel", 2f);
    }
    public void PrepareFruitPanel(string count)
    {
        questPanel.gameObject.SetActive(true);
        goalText.text = "Осталось собрать: " + count;
        Invoke("ShowPanel", 2f);
    }
    public void ChangeCounter(int count)
    {
        goalText.text = "Осталось собрать: " + count;
    }
    public void ChangeCounter(string count)
    {
        goalText.text = "Осталось собрать: " + count;
    }
    public void PrepareTimerPanel()
    {
        questPanel.gameObject.SetActive(true);
        questPanel.GetComponentInChildren<TextMeshProUGUI>().text = questFinish[0];
        Invoke("ShowPanel", 1.5f);
        StartCoroutine(StartTimer());
    }
    IEnumerator StartTimer()
    {
        float shownTime = 0f;
        while (!gameManager.IsWin)
        {
            timer += Time.deltaTime; // Увеличиваем таймер на время прошедшее с последнего кадра
            float roundedTime = Mathf.Round(timer * 100.0f) / 100.0f; // Округляем до сотых

            if (roundedTime != shownTime)
            {
                shownTime = roundedTime;
                goalText.text = "Time: " + shownTime.ToString("F2"); // Выводим значение таймера в формате с двумя десятичными знаками
            }

            yield return null; // Ждем до следующего кадра
        }
    }
    public void ShakeCamera()
    {
        cameraShake.Shake();
    }
    public void OnHomeButtonClick()
    {
        SceneManager.LoadScene(0);
    }
    public void OnResetButtonClick()
    {
        gameManager.RestartLevel();
    }
    public void OnSoundButtonClick()
    {

    }
    public void OnMenuButtonClick()
    {
        if (menuButton.activeSelf)
        {
            StartCoroutine(ShowPanelButton());
            menuButton.SetActive(false);
        }
    }
    IEnumerator ShowPanelButton()
    {
        buttonPanel.SetActive(true);
        Vector3 startVector = new Vector3(0.01f, 0.01f, 0.01f);
        Vector3 endVector = new Vector3(0.6f, 0.6f, 0.6f);
        buttonPanel.transform.localScale = startVector;
        float elapsedTime = 0f;
        while (elapsedTime / 1 < 1)
        {
            buttonPanel.transform.localScale = Vector3.Lerp(startVector, endVector, elapsedTime / 1);
            elapsedTime += 5*Time.deltaTime;
            yield return null;
        }
        buttonPanel.transform.localScale = endVector;
    }
    public void HidePanel()
    {
        StartCoroutine(HidePanelButton());
        
    }
    IEnumerator HidePanelButton()
    {
        Vector3 startVector = new Vector3(0.6f, 0.6f, 0.6f);
        Vector3 endVector = new Vector3(0.01f, 0.01f, 0.01f);
        buttonPanel.transform.localScale = startVector;
        float elapsedTime = 0f;
        while (elapsedTime / 1 < 1)
        {
            buttonPanel.transform.localScale = Vector3.Lerp(startVector, endVector, elapsedTime / 1);
            elapsedTime += 5*Time.deltaTime;
            yield return null;
        }
        buttonPanel.SetActive(false);
        menuButton.SetActive(true);
    }
    public void OnPauseButtonClick()
    {
        gameManager.IsPaused = !gameManager.IsPaused;
        if (gameManager.IsPaused)
        {
            pauseButton.sprite = pauseSprite[0];
        }
        else
        {
            pauseButton.sprite = pauseSprite[1];
        }
    }

    public void PrepareToLoad()
    {
        loadScene.gameObject.SetActive(true);
    }

    #region Аксессоры
    public TranslationManager EndLoadScene
    {
        get { return endLoadScene; }
    }
    public TranslationManager StartLoadScene
    {
        get { return loadScene; }
    }
    #endregion
}
