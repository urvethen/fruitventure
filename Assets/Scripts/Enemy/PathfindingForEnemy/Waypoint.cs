using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint: MonoBehaviour
{
    public Waypoint eploredFrom;
    public bool isExplored;
    [SerializeField] SpriteRenderer spriteRenderer;
    public Vector3 RealPosition
    {
        get
        {
            return new Vector3(transform.position.x + 0.25f, transform.position.y + 0.25f, transform.position.z);
        }
    }
    public Vector2Int GridPosition
    {
        get
        {
            return new Vector2Int(Mathf.RoundToInt(transform.position.x * 2), Mathf.RoundToInt(transform.position.y * 2));
        }
    }
    void Start()
    {
        //  spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a/2);
    }


}
