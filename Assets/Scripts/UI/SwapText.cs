using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwapText : MonoBehaviour
{

    [SerializeField] List<string> text = new List<string>();
    TextMeshProUGUI textMesh;
    SaveManager saveManager;
    private void OnEnable()
    {
        MainMenu.ChangeLocalization += SwapLocalization;
        if (saveManager != null )
        {
            textMesh.text = text[saveManager.GetLocaliztionIndex];
        }
    }
    private void OnDisable()
    {
        MainMenu.ChangeLocalization -= SwapLocalization;
    }
    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        saveManager = SaveManager.Instance;
        textMesh.text = text[saveManager.GetLocaliztionIndex];
    }
    public void SwapLocalization(int index)
    {
        
            textMesh.text = text[index];
        
    }
}
