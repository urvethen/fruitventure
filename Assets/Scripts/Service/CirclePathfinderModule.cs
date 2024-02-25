using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CirclePathfinderModule: MonoBehaviour
{
    /*! Порядок работы алгоритма
     *Выбираем направление
     *Ставим метку на следующую клетку
     *Смотрим, есть ли внизу граница?
     * Есть - переходим дальше влево
     * Нет? идем вниз на клетку и сканируем справа
     */
    [SerializeField] MoveDirectional currentDirectional;
    [SerializeField] bool isClockwise;
    Queue<Vector2Int> directQueue = new Queue<Vector2Int>();
    [SerializeField] List<Tilemap> level = new List<Tilemap>();
    [SerializeField] Vector2 targetStep;
    MoveStepByStep step;
    //List<MoveDirectional> moveDirectionals = new List<MoveDirectional>();

    private void Awake()
    {
        step = GetComponent<MoveStepByStep>();
        CreateQueue();
    }
    private void Start()
    {
        step.CorrectPosition(currentDirectional);
    }
    public Vector2 CheckNextStepV2()
    {
        Vector2Int current = new Vector2Int(Mathf.RoundToInt(2 * transform.position.x), Mathf.RoundToInt(2 * transform.position.y));
        Vector2Int firstDirection = directQueue.Dequeue();
        Vector2Int next = current + firstDirection;
        TileBase firstCheck = GetTile(new Vector3Int(next.x, next.y));
        if (firstCheck == null)
        {
            
            Vector2Int next2 = next + directQueue.Dequeue();
            TileBase secondCheck = GetTile(new Vector3Int(next2.x, next2.y));
            if (secondCheck != null)
            {
                CreateQueue();
                targetStep = new Vector2((float)next.x / 2, (float)next.y / 2);
            }
            else
            {
                RotateDirectional(true);
                targetStep = new Vector2((float)next2.x / 2, (float)next2.y / 2);
            }
        }
        else
        {
            RotateDirectional(false);
            targetStep = new Vector2((float)current.x / 2, (float)current.y / 2);
        }


            return targetStep;
    }
   private TileBase GetTile(Vector3Int pos)
    {
        for (int i = 0; i < level.Count; i++)
        {
            TileBase tile = level[i].GetTile(pos);
            if(tile != null)
            {
                return tile;
            }
        }
        return null;
    }
    private void CreateQueue()
    {
        directQueue.Clear();
        if (!isClockwise)
        {
            switch (currentDirectional)
            {
                case MoveDirectional.Left:
                    directQueue.Enqueue(Vector2Int.left);
                    directQueue.Enqueue(Vector2Int.down);
                    directQueue.Enqueue(Vector2Int.up);
                    break;
                case MoveDirectional.Down:
                    directQueue.Enqueue(Vector2Int.down);
                    directQueue.Enqueue(Vector2Int.right);
                    directQueue.Enqueue(Vector2Int.left);
                    break;
                case MoveDirectional.Right:
                    directQueue.Enqueue(Vector2Int.right);
                    directQueue.Enqueue(Vector2Int.up);
                    directQueue.Enqueue(Vector2Int.down);
                    break;
                case MoveDirectional.Up:
                    directQueue.Enqueue(Vector2Int.up);
                    directQueue.Enqueue(Vector2Int.left);
                    directQueue.Enqueue(Vector2Int.right);
                    break;
            }
        }
        else
        {
            switch (currentDirectional)
            {
                case MoveDirectional.Left:
                    directQueue.Enqueue(Vector2Int.left);
                    directQueue.Enqueue(Vector2Int.up);
                    directQueue.Enqueue(Vector2Int.down);
                    break;
                case MoveDirectional.Down:
                    directQueue.Enqueue(Vector2Int.down);
                    directQueue.Enqueue(Vector2Int.left);
                    directQueue.Enqueue(Vector2Int.right);
                    break;
                case MoveDirectional.Right:
                    directQueue.Enqueue(Vector2Int.right);
                    directQueue.Enqueue(Vector2Int.down);
                    directQueue.Enqueue(Vector2Int.up);
                    break;
                case MoveDirectional.Up:
                    directQueue.Enqueue(Vector2Int.up);
                    directQueue.Enqueue(Vector2Int.right);
                    directQueue.Enqueue(Vector2Int.left);
                    break;
            }
        }
    }    
    private void RotateDirectional(bool flag)
    {
        if (!isClockwise)
        {
            if (flag)
            {
                switch (currentDirectional)
                {
                    case MoveDirectional.Left:
                        CurrentDirectional = MoveDirectional.Down;
                        break;
                    case MoveDirectional.Right:
                        CurrentDirectional = MoveDirectional.Up;
                        break;
                    case MoveDirectional.Up:
                        CurrentDirectional = MoveDirectional.Left;
                        break;
                    case MoveDirectional.Down:
                        CurrentDirectional = MoveDirectional.Right;
                        break;
                }
            }
            else
            {
                switch (currentDirectional)
                {
                    case MoveDirectional.Left:
                        CurrentDirectional = MoveDirectional.Up;
                        break;
                    case MoveDirectional.Right:
                        CurrentDirectional = MoveDirectional.Down;
                        break;
                    case MoveDirectional.Up:
                        CurrentDirectional = MoveDirectional.Right;
                        break;
                    case MoveDirectional.Down:
                        CurrentDirectional = MoveDirectional.Left;
                        break;
                }
            }
        }
        else
        {
            if (flag)
            {
                switch (currentDirectional)
                {
                    case MoveDirectional.Left:
                        CurrentDirectional = MoveDirectional.Up;
                        break;
                    case MoveDirectional.Right:
                        CurrentDirectional = MoveDirectional.Down;
                        break;
                    case MoveDirectional.Up:
                        CurrentDirectional = MoveDirectional.Right;
                        break;
                    case MoveDirectional.Down:
                        CurrentDirectional = MoveDirectional.Left;
                        break;
                }
            }
            else
            {
                switch (currentDirectional)
                {
                    case MoveDirectional.Left:
                        CurrentDirectional = MoveDirectional.Down;
                        break;
                    case MoveDirectional.Right:
                        CurrentDirectional = MoveDirectional.Up;
                        break;
                    case MoveDirectional.Up:
                        CurrentDirectional = MoveDirectional.Left;
                        break;
                    case MoveDirectional.Down:
                        CurrentDirectional = MoveDirectional.Right;
                        break;
                }
            }
        }
        CreateQueue();
    }
    [Serializable]
    public enum MoveDirectional
    {
        Left,
        Down,
        Right,
        Up
    }
    public bool IsClockwise
    {
        get { return isClockwise; }
    }
    public MoveDirectional CurrentDirectional 
    { get { return currentDirectional; }
        private set 
        { 
        if(currentDirectional != value)
            {
                currentDirectional = value;
                step.CorrectPosition(currentDirectional);
            }
        }
    }
}
