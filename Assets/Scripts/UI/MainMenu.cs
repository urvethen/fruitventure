using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TranslationManager translationManager;
    [SerializeField] Transform levelPanel;
    [SerializeField] List<string> fruitCounterText = new List<string>();
    [SerializeField] TextMeshProUGUI fruitCounter;
    [SerializeField] List<Sprite> soundSprite = new List<Sprite>();
    [SerializeField] Image soundButton;
    public static UnityAction<int> ChangeLocalization;
    SaveManager saveManager;
    SoundManager soundManager;
    
    public void OnButtonPlayClick()
    {
        soundManager.PlayCLick();
        StartCoroutine(LoadScene(1));
    }

    public void OnButtonContinueClick()
    {
        soundManager.PlayCLick();
        int id = saveManager.GetLastLvl;
        StartCoroutine(LoadScene(id));
    }
    public void OnButtonLvlClick(int lvl)
    {
        soundManager.PlayCLick();
        StartCoroutine(LoadScene(lvl));
    }
    public void OnButtonLevelCLick()
    {
        soundManager.PlayCLick();
        levelPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    public void OnButtonSoundClick()
    {
        soundManager.PlayCLick();
        saveManager.SoundOn = !saveManager.SoundOn;
        if(saveManager.SoundOn)
        {
            soundManager.PlayMainMenuTheme();
            soundButton.sprite = soundSprite[0];
        }
        else
        {
            soundButton.sprite = soundSprite[1];
        }
    }
    
    public void OnButtonLocalizationClick()
    {
        soundManager.PlayCLick();
        if (saveManager.GetLocaliztionIndex == 0)
        {
            saveManager.ChangeLocalization(1);
        }
        else
        {
            saveManager.ChangeLocalization(0);
        }
        ChangeLocalization?.Invoke(saveManager.GetLocaliztionIndex);
    }
    public void OnButtonLeadershipClick()
    {

    }
    private void Start()
    {
        
        saveManager = SaveManager.Instance;
        soundManager = SoundManager.Instance;
        if (saveManager == null && saveManager.SoundOn)
        {
            soundManager.PlayMainMenuTheme();
        }        
        SetFruitCounter();
    }
    private void SetFruitCounter()
    {
        fruitCounter.text = fruitCounterText[saveManager.GetLocaliztionIndex] + saveManager.GetFruitCounter.ToString();
    }
    IEnumerator LoadScene(int id)
    {
        translationManager.gameObject.SetActive(true);
        while (!translationManager.Ready)
        {
            yield return null;
        }
        SceneManager.LoadScene(id);
    }
}
