using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : MonoBehaviour
{
    [SerializeField] public int level;

    [SerializeField]
    public int batteryCount;
    
    public MissionInfo DebugMissionInfo { get{return new MissionInfo(1,1,1,1);}}
    
    public int Level
    {
        get { return level; }
    }

    public static GameConfig Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        if (Instance != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }
}
