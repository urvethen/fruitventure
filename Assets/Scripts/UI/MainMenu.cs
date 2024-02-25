using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TranslationManager translationManager;
    [SerializeField] Transform levelPanel;
    public void OnButtonPlayClick()
    {
        StartCoroutine(LoadScene(1));
    }

    public void OnButtonContinueClick()
    {
        int id = SaveManager.Instance.GetLastLvl;
        StartCoroutine(LoadScene(id));
    }
    public void OnButtonLvlClick(int lvl)
    {
        StartCoroutine(LoadScene(lvl));
    }
    public void OnButtonLevelCLick()
    {
        levelPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    public void OnButtonSoundClick()
    {
        
    }
    public void OnButtonLocalizationClick()
    {

    }
    public void OnButtonLeadershipClick()
    {

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
