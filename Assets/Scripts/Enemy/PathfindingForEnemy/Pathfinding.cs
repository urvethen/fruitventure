using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding: MonoBehaviour
{
    //!Базовый файл для поиска пути в ширину на объекте
    [Header("Создание путевых точек")]
    [SerializeField] Vector2Int worldSize;
    [SerializeField] GameObject waypointsPrefab;
    [SerializeField] Transform waypoints;
    [SerializeField] List<Tilemap> tilemaps = new List<Tilemap>();
    
    [Header("Параметры работы")]
    [SerializeField] bool isRunning = false;
    [SerializeField] Transform start, end;
    Dictionary<Vector2Int, Waypoint> levelMaps = new Dictionary<Vector2Int, Waypoint>();
    Vector2Int startKey, endKey, searchingKey;
    Queue<Waypoint> waypointsQueue = new Queue<Waypoint>();
    List<Waypoint> waypointsRoad = new List<Waypoint>();
    Vector2Int[] directions =
    {
        Vector2Int.left,
        
        Vector2Int.up,
        
        Vector2Int.right,
        
        Vector2Int.down,
        new Vector2Int(-1, 1),
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, -1)
    };
    private void Awake()
    {
        CreateWaypoints();
    }

    void Start()
    {
       // CreateRoute(start.position, end.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public List<Vector3> CreateRoute(Vector3 startPoint, Vector3 endPoint)
    {
        List<Vector3> route = new List<Vector3>();
        startKey = (Vector2Int)tilemaps[0].WorldToCell(startPoint);
        endKey = (Vector2Int)tilemaps[0].WorldToCell(endPoint);        
        if (levelMaps.ContainsKey(startKey) && levelMaps.ContainsKey(endKey))
        {
            
            isRunning = true;
            StartPathfinding();
            CreateRoad();
            route = CreateRoute();
            ClearWaypoints();
        }
        else
        {
            Debug.LogWarning("По точкам маршрута не найдены клетки маршрута, укажите другие точки");
        }
        return route;
    }    
    private void StartPathfinding()
    {
        waypointsQueue.Clear();
        //ExploreNearPoints(startKey);
        waypointsQueue.Enqueue(levelMaps[startKey]);
        while (waypointsQueue.Count > 0 && isRunning == true)
        {
            Waypoint searching = waypointsQueue.Dequeue();
            searching.isExplored = true;            
            CheckForTheEnd(searching);
            ExploreNearPoints(searching.GridPosition);
        }
    }
    private void CheckForTheEnd(Waypoint searching)
    {
        if (searching.GridPosition == endKey)
        {
            isRunning = false;
        }
    }
    private void ExploreNearPoints(Vector2Int key)
    {
        if (!isRunning)
        {
            return;
        }
        foreach (Vector2Int direction in directions)
        {
            Vector2Int nearPoints = key + direction;
            if (levelMaps.ContainsKey(nearPoints) && !waypointsQueue.Contains(levelMaps[nearPoints]) && !levelMaps[nearPoints].isExplored)
            {
                waypointsQueue.Enqueue(levelMaps[nearPoints]);
                levelMaps[nearPoints].eploredFrom = levelMaps[key];               
            }
        }
    }
    private void CreateRoad()
    {
        waypointsRoad.Add(levelMaps[endKey]);

        while (true)
        {
            Waypoint previous = waypointsRoad[waypointsRoad.Count - 1].eploredFrom;
            waypointsRoad.Add(previous);
            if (previous == levelMaps[startKey])
            {
                break;
            }
            else
            {
               
            }
        }
        waypointsRoad.Reverse();
    }
    private List<Vector3> CreateRoute()
    {
        List<Vector3> route = new List<Vector3>();
        for (int i = 0; i < waypointsRoad.Count; i++)
        {
            route.Add(waypointsRoad[i].RealPosition);
        }
        return route;
    }
    private void ClearWaypoints()
    {
        foreach (var waypoint in levelMaps.Values)
        {
            if (waypoint.isExplored)
            {
                waypoint.eploredFrom = null;
                waypoint.isExplored = false;
            }
        }
    }

    #region Создание поля точек
    private void CreateWaypoints()
    {
        for (int i = -worldSize.x; i < worldSize.x; i++)
        {
            for (int j = -worldSize.y; j < worldSize.y; j++)
            {
                Vector2Int key = new Vector2Int(i, j);
                if (CheckTileForEmpty(key))
                {
                    Waypoint newPoint = Instantiate(waypointsPrefab, new Vector3(i / 2f, j / 2f), Quaternion.identity, waypoints).GetComponent<Waypoint>();
                    levelMaps.Add(key, newPoint);
                }

            }
        }
    }

    private bool CheckTileForEmpty(Vector2Int pos)
    {
        Vector3Int posInGrid = new Vector3Int(pos.x, pos.y);
        bool isEmpty = true;
        for (int i = 0; i < tilemaps.Count; i++)
        {
            TileBase tile = tilemaps[i].GetTile(posInGrid);
            if (tile != null)
            {
                isEmpty = false;
                break;
            }
        }

        return isEmpty;
    }
    #endregion
}
