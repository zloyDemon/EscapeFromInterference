using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : MonoBehaviour
{
    [SerializeField] 
    public int level;

    [SerializeField]
    public int batteryCount;

    [SerializeField] private bool keyboardControl;
    
    public int Level
    {
        get { return level; }
    }

    public bool KeyboardControl => keyboardControl;

    public static GameConfig Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
            
        DontDestroyOnLoad(gameObject);
        MissionManager.Instance.Init();
    }
}
