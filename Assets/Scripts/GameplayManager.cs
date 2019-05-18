using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public static readonly float TimeForEnd = 1.5f;

    private MissionInfo currentMissionInfo;
    private static GameplayManager instance;
    private bool isEnemyCatch;
    private bool isEnd;
    private Coroutine endGameCor;
    private bool isGamePause;
    private float checkGhostDeviceValue;
    
    public static GameplayManager Instance { get { return instance; } }
    
    public Action EndGame = () => { };
    public Action<bool> PauseGame = b => { };
    
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
        currentMissionInfo = new MissionInfo(2, 3,3);
    }

    private void Start()
    {
        DungeonManager.Instance.LevelLoaded += InstanceOnLevelLoaded;
        DungeonManager.Instance.LoadLevel();
    }

    private void OnDestroy()
    {
        DungeonManager.Instance.LevelLoaded -= InstanceOnLevelLoaded;
    }
    
    private void InstanceOnLevelLoaded()
    {
        UIManager.Instance.FadeOut(0.8f, () =>
        {
            Debug.Log("Start game");
        });
    }

    public void EnemyChecked(Transform enemyTransform, Transform playerTransform)
    {
        var dist = Vector3.Distance(enemyTransform.position ,playerTransform.position);
        if (!isEnemyCatch && dist < EFIUtils.DistanceForEnd)
        {
            isEnemyCatch = true;
            EnableGameEnd(true);
        }else if (dist > EFIUtils.DistanceForEnd && !isEnd)
        {
            EnableGameEnd(false);
            isEnemyCatch = false;
        }           
    }

    private void EnableGameEnd(bool flag)
    {
        if (flag)
        {
            if (endGameCor == null)
                endGameCor = StartCoroutine(CoTimeForEnd());    
        }
        
        if (!flag)
        {
            if (endGameCor != null)
            {
                StopCoroutine(endGameCor);
                endGameCor = null;
            }    
        }
        
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

    //Not use yet
    private void GameEnd(bool isEnd)
    {
        if (isEnd && endGameCor == null)
        {
            endGameCor = StartCoroutine(CoTimeForEnd());
        }
    }

    private IEnumerator CoTimeForEnd()
    {
        yield return new WaitForSeconds(TimeForEnd);
        Debug.Log("Game end");
        isEnd = true;
        EndGame();
    }
}
