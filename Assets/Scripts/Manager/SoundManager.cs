using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager: MonoBehaviour
{
    #region Синглтон
    public static SoundManager _instance;
    public static SoundManager Instance { get { return _instance; } }
    #endregion
    private void Awake()
    {
        #region Синглтон
        if (Instance != null && Instance != this)
        {

            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        #endregion
    }
}
