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
    
    void Start()
    {
        ClearButton();
        CreateButton();
        scrollbar.value = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnBackButtonClick()
    {
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
            print("Create butt ¹" + i + "with index " + index);
            butt.onClick.AddListener(() => { StartLoadLevel(index); });
            butt.GetComponentInChildren<TextMeshProUGUI>().text = index.ToString();            
        }
    }
    private void StartLoadLevel(int lvl)
    {
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
