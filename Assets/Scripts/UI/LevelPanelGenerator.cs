using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelPanelGenerator: MonoBehaviour
{
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Transform container;
    [SerializeField] Transform panelMain, panelLvl;
    [SerializeField] TranslationManager translationManager;
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] Sprite unenableButtSprite;
    SoundManager soundManager;
    string levels;
    SaveManager saveManager;
    void Start()
    {
        soundManager = SoundManager.Instance;
        saveManager = SaveManager.Instance;
        levels = saveManager.GetLevels;
        ClearButton();
        CreateButton();
        scrollbar.value = 1.0f;
    }

    
    public void OnBackButtonClick()
    {
        soundManager.PlayCLick();
        panelMain.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    void ClearButton()
    {
        if (container.childCount > 0)
        {
            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }
        }
    }
    void CreateButton()
    {
        
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            Button butt = Instantiate(buttonPrefab, container).GetComponent<Button>();
            int index = i;
            print(levels.Length);
            if (levels[i-1] == 'A')
            {
                butt.onClick.AddListener(() => { StartLoadLevel(index); });
                butt.GetComponentInChildren<TextMeshProUGUI>().text = index.ToString();
            }
            else
            {
                butt.enabled = false;
                butt.GetComponentInChildren<TextMeshProUGUI>().text = index.ToString();
                butt.transform.GetComponent<Image>().sprite = unenableButtSprite;
            }
           // print("Create butt ¹" + i + "with index " + index);
                      
        }
    }
    private void StartLoadLevel(int lvl)
    {
        soundManager.PlayCLick();
        StartCoroutine(LoadScene(lvl));

        SaveManager manager = SaveManager.Instance;
        manager.SaveLastNumber(lvl);
    }
    IEnumerator LoadScene(int id)
    {
        print("Load scene ¹" + id);
        translationManager.gameObject.SetActive(true);
        while (!translationManager.Ready)
        {
            yield return null;
        }
        SceneManager.LoadScene(id);
    }
}
