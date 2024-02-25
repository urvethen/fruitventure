using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;

public class WobbleText: MonoBehaviour
{
    [SerializeField] bool needWobble = true;
    [SerializeField] bool vertex, characters, word;
    [SerializeField] Vector2 woobleSpeed = new Vector2(2f, 1.5f);
    [SerializeField] float woobleAmplitude = 5f;
    [SerializeField] bool needRainbow;
    [SerializeField] Gradient rainbow;
    [SerializeField] float colorChangeSpeed = 1f, colorChangeModifier = 1f;

    TMP_Text text;
    List<int> wordIndexes = new List<int>();
    List<int> wordLengths = new List<int>();

    Mesh mesh;
    Vector3[] vertices;
    Color[] colors;
    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }
    // Start is called before the first frame update
    void Start()
    {
        SplitStringsOnWord();
    }

    // Update is called once per frame
    void Update()
    {
        text.ForceMeshUpdate();
        mesh = text.mesh;
        vertices = mesh.vertices;
        colors = mesh.colors;
        if (needWobble)
        {

            if (vertex)
            {
                WobbleVertex();
            }
            if (characters)
            {
                WobbleCharacters();
            }
            if (word)
            {
                WobbleWord();
            }

        }
        if (needRainbow)
        {
            CreateRainbow();
        }
        mesh.colors = colors;
        mesh.vertices = vertices;
        text.canvasRenderer.SetMesh(mesh);
    }


    private void CreateRainbow()
    {

        for (int i = 0; i < vertices.Length - 4; i++)
        {
            colors[i] = rainbow.Evaluate(Mathf.Repeat(Time.time * colorChangeSpeed + vertices[i].x * 0.001f * colorChangeModifier, 1f));
        }


    }
    private void WobbleVertex()
    {
        for (int i = 0; i < vertices.Length - 4; i++)
        {
            Vector3 offset = woobleAmplitude * Wobble(Time.time + i);
            vertices[i] = vertices[i] + offset;
            // colors[i] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[i].x * 0.001f, 1f));

        }
    }

    private void WobbleCharacters()
    {
        for (int i = 0; i < text.textInfo.characterCount; i++)
        {
            TMP_CharacterInfo c = text.textInfo.characterInfo[i];
            int index = c.vertexIndex;
            Vector3 offset = woobleAmplitude * Wobble(Time.time + i);
            vertices[index] = vertices[index] + offset;
            vertices[index + 1] = vertices[index + 1] + offset;
            vertices[index + 2] = vertices[index + 2] + offset;
            vertices[index + 3] = vertices[index + 3] + offset;
            /*
            colors[index] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index].x * 0.001f, 1f));
            colors[index + 1] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index + 1].x * 0.001f, 1f));
            colors[index + 2] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index + 2].x * 0.001f, 1f));
            colors[index + 3] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index + 3].x * 0.001f, 1f));
            */
        }
    }

    private void SplitStringsOnWord()
    {
        wordIndexes = new List<int>() { 0 };
        wordLengths = new List<int>();
        string s = text.text;
        for (int index = s.IndexOf(' '); index > -1; index = s.IndexOf(' ', index + 1))
        {
            wordLengths.Add(index - wordIndexes[wordIndexes.Count - 1]);
            wordIndexes.Add(index + 1);
        }
        wordLengths.Add(s.Length - wordIndexes[wordIndexes.Count - 1]);
    }

    private void WobbleWord()
    {
        for (int w = 0; w < wordIndexes.Count; w++)
        {
            int wordIndex = wordIndexes[w];
            Vector3 offset = woobleAmplitude * Wobble(Time.time + w);

            for (int i = 0; i < wordLengths[w]; i++)
            {
                TMP_CharacterInfo c = text.textInfo.characterInfo[wordIndex + i];

                int index = c.vertexIndex;

                vertices[index] += offset;
                vertices[index + 1] += offset;
                vertices[index + 2] += offset;
                vertices[index + 3] += offset;
                /*
                colors[index] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index].x * 0.001f, 1f));
                colors[index + 1] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index + 1].x * 0.001f, 1f));
                colors[index + 2] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index + 2].x * 0.001f, 1f));
                colors[index + 3] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index + 3].x * 0.001f, 1f));
                */
            }
        }

    }
    Vector2 Wobble(float time)
    {
        return new Vector2(Mathf.Sin(time * woobleSpeed.x), Mathf.Cos(time * woobleSpeed.y));
    }
}
