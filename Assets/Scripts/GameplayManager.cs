using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public static readonly float TimeForEnd = 1f;

    private MissionInfo currentMissionInfo;
    private static GameplayManager instance;
    private bool isEnemyCatch;
    private bool isEnd;
    private Coroutine corEndGame;
    private bool isGamePause;
    private float checkGhostDeviceValue;
    
    public static GameplayManager Instance => instance;

    public Transform Player { get; set; }

    public List<GameObject> GhostOnMap { get; set; }
    
    public event Action EndGame = () => { };
    public event Action<bool> PauseGame = b => { };
    public event Action<bool> StartFallGame = b => { };
    
    public bool IsGamePause
    {
        set
        {
            isGamePause = value;
            GamePause(isGamePause);
        }

        get { return isGamePause; }
    }

    public MissionInfo CurrentMissionInfo { get { return currentMissionInfo; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(gameObject);
        
        Debug.Log("Awake GameplayManager");
    }

    private void Start()
    {
        DungeonManager.Instance.LevelLoaded += InstanceOnLevelLoaded;
        DungeonManager.Instance.LoadLevel();
    }

    private bool isPause;
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            GamePause(isPause);
            isPause = !isPause;
        }
            
    }

    private void OnDestroy()
    {
        DungeonManager.Instance.LevelLoaded -= InstanceOnLevelLoaded;
    }
    
    private void InstanceOnLevelLoaded()
    {
        UIManager.Instance.FadeOut(0.8f, () =>
        {
            currentMissionInfo = DungeonManager.Instance.CurrentMissionInfo;
        });
    }


    public void FlashlighDead()
    {
        Debug.Log("GameplayManager: Flashlight dead");
    }

    private void GamePause(bool isPause)
    {
        Time.timeScale = isPause ? 0 : 1;
      
        PauseGame(isPause);
    }

    public void MissionComplete()
    {
        UIManager.Instance.FadeIn(0.8f, () =>
        {
            Debug.Log("GameplayManager: MissionComplete. FadeIn");    
            GameItems.Instance.ResetItems();
            SceneManager.LoadScene(0);
        });
    }

    public void FallingLevel(bool isFalling)
    {
        StartFallGame(isFalling);
    }

    public void StartEndGame()
    {
        StartCoroutine(FallingGame());
    }

    private IEnumerator FallingGame()
    {
        yield return new WaitForSeconds(TimeForEnd);
        EndGame();
    }
    
    
}
