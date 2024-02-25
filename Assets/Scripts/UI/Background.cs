using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] SpriteRenderer bg1, bg2;
    [SerializeField] List<Sprite> sprites = new List<Sprite>();
    [SerializeField] Vector2Int direction = Vector2Int.down;
    private void Awake()
    {
        int num = Random.Range(0,sprites.Count);
        bg1.sprite = sprites[num];
        bg2.sprite = sprites[num];
    }

}
