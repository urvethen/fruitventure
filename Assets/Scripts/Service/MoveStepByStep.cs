using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class MoveStepByStep: MonoBehaviour
{
    [SerializeField] bool isStatic, isLinear;
    [SerializeField] float stepTime = 1f;
    [SerializeField] Transform gfx;
    [SerializeField] Transform startPoint, endPoint;
    [SerializeField] Chain chain;
    CirclePathfinderModule pathfinderModule;

    private void Awake()
    {
        pathfinderModule = GetComponent<CirclePathfinderModule>();
        gfx = transform.GetChild(0);
        chain = GetComponentInChildren<Chain>();
    }
    void Start()
    {
        if (!isStatic && !isLinear)
        {
            StartCoroutine(StepMovement());
            if (pathfinderModule.IsClockwise)
            {
                gfx.localScale = new Vector3(-1, 1, 1);
            }
        }
        else if (!isStatic && isLinear)
        {
            chain.CreateChain(startPoint, endPoint);
            StartCoroutine(MoveToEndPoint());
        }

    }


    IEnumerator StepMovement()
    {
        while (true)
        {
            yield return new WaitForSeconds(stepTime);
            StartCoroutine(GlobalMove(pathfinderModule.CheckNextStepV2()));
            //    gfx.localPosition = CorrectPosition(pathfinderModule.CurrentDirectional);
        }

    }
    public void CorrectPosition(CirclePathfinderModule.MoveDirectional direct)
    {
        if (!pathfinderModule.IsClockwise)
        {
            switch (direct)
            {
                case CirclePathfinderModule.MoveDirectional.Left:
                    StartCoroutine(LocalMove(new Vector3(0, 0, 0)));
                    break;
                case CirclePathfinderModule.MoveDirectional.Down:
                    StartCoroutine(LocalMove(new Vector3(0.5f, 0, 0)));
                    break;
                case CirclePathfinderModule.MoveDirectional.Right:
                    StartCoroutine(LocalMove(new Vector3(0.5f, 0.5f, 0)));
                    break; ;
                case CirclePathfinderModule.MoveDirectional.Up:
                    StartCoroutine(LocalMove(new Vector3(0, 0.5f, 0)));
                    break;
            }
        }
        else
        {
            switch (direct)
            {
                case CirclePathfinderModule.MoveDirectional.Left:
                    StartCoroutine(LocalMove(new Vector3(0, 0.5f, 0)));
                    break;
                case CirclePathfinderModule.MoveDirectional.Down:
                    StartCoroutine(LocalMove(new Vector3(0, 0, 0)));
                    break;
                case CirclePathfinderModule.MoveDirectional.Right:
                    StartCoroutine(LocalMove(new Vector3(0.5f, 0, 0)));
                    break; ;
                case CirclePathfinderModule.MoveDirectional.Up:
                    StartCoroutine(LocalMove(new Vector3(0.5f, 0.5f, 0)));
                    break;
            }
        }
    }
    IEnumerator LocalMove(Vector3 destination)
    {
        float elapsedTime = 0f;
        Vector3 current = gfx.localPosition;
        while (elapsedTime < 1f)
        {
            gfx.localPosition = Vector3.Lerp(current, destination, elapsedTime);
            elapsedTime += Time.deltaTime / stepTime;
            yield return null;
        }
    }
    IEnumerator GlobalMove(Vector3 destination)
    {
        float elapsedTime = 0f;
        Vector3 current = transform.position;
        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(current, destination, elapsedTime);
            elapsedTime += Time.deltaTime / stepTime;
            yield return null;
        }
    }

    IEnumerator MoveToEndPoint()
    {
        float distance = Vector3.Distance(gfx.position, endPoint.position);
        while (distance > 0.1f)
        {
            CheckDirection(endPoint.position);
            gfx.position = Vector3.MoveTowards(gfx.position, endPoint.position, Time.deltaTime / stepTime);
            distance = Vector3.Distance(gfx.position, endPoint.position);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(MoveToStartPoint());
    }
    IEnumerator MoveToStartPoint()
    {

        float distance = Vector3.Distance(gfx.position, startPoint.position);
        while (distance > 0.1f)
        {
            CheckDirection(startPoint.position);
            gfx.position = Vector3.MoveTowards(gfx.position, startPoint.position, Time.deltaTime / stepTime);
            distance = Vector3.Distance(gfx.position, startPoint.position);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(MoveToEndPoint());
    }
    private void CheckDirection(Vector3 position)
    {
        if ( position.x - gfx.position.x < 0 )
        {
            gfx.localScale = Vector3.one;
        }
        else
        {
            
            gfx.localScale = new Vector3(-1, 1, 1);
        }
    }
}
