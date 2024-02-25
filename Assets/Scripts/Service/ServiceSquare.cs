using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ServiceSquare: MonoBehaviour
{
    [SerializeField] TextMeshPro textMesh;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool theWay = false;


    public void UpdateText()
    {
        textMesh.text = (2 * transform.position.x).ToString() + ": " + (2 * transform.position.y).ToString();
    }

    public void ThisIsTheWay()
    {
        //spriteRenderer.color = Color.green;
        theWay = true;
    }
    public bool TheWay {  get { return theWay; } }
}
