using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinderService : MonoBehaviour
{
    [SerializeField] Vector2Int startWorldPoint, endWorldPoint;
    [SerializeField] Tilemap tilemap;
    [SerializeField] GameObject squarePrefab;
    Vector2Int[] directions = new Vector2Int[8];
    Dictionary<Vector2Int, ServiceSquare> pathBase = new Dictionary<Vector2Int, ServiceSquare>();

    private void Awake()
    {
        directions = new Vector2Int[] {Vector2Int.left, new Vector2Int(-1,1), Vector2Int.up, new Vector2Int(1, 1), Vector2Int.right, new Vector2Int(1, -1), Vector2Int.down,new Vector2Int(-1, -1)};
    }
    private void Start()
    {
        //!Это первая версия
        StartCoroutine(PlaceSquare());
    }
    #region 1 версия
    IEnumerator PlaceSquare()
    {
        for (int i = startWorldPoint.x; i <= endWorldPoint.x; ++i)
        {
            for (int j = startWorldPoint.y; j <= endWorldPoint.y; ++j)
            {
                TileBase tile = tilemap.GetTile(new Vector3Int(i, j));
                if (tile == null)
                {
                    ServiceSquare square = Instantiate(squarePrefab, new Vector3((float)i / 2, (float)j / 2), Quaternion.identity, transform).GetComponent<ServiceSquare>();
                    square.UpdateText();
                    pathBase.Add(new Vector2Int(i, j), square);
                }
            }
            
        }
        yield return null;
        StartCoroutine(CheckPair());
    }

    IEnumerator CheckPair()
    {
        foreach (var key in pathBase.Keys)
        {
            CheckNearestSquare(key);
            
        }
        yield return null;
    }
    private void CheckNearestSquare(Vector2Int baseKey)
    {
        bool flag = false;
        for (int i =0; i< directions.Length; ++i)
        {
            Vector2Int newKey = baseKey + directions[i];
            if (!pathBase.ContainsKey(newKey))
            {
                pathBase[baseKey].ThisIsTheWay();
                flag = true;
            }
        }        
    }
    #endregion



    public enum Directions
    {
        left,
        up,
        right,
        down
    }
}
