using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static readonly float TimeForEnd = 1.5f;
    
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
    

    private void Awake()
    {
        if (instance == null)
            instance = this;
        if(instance != this)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {

    }

    public void EnemyChecked(Transform enemyTransform, Transform playerTransform)
    {
        var dist = Vector3.Distance(enemyTransform.position ,playerTransform.position);
        GameUI.Instance.SetText(dist.ToString());
        if (!isEnemyCatch && dist < EFIUtils.DistanceForEnd)
        {
            isEnemyCatch = true;
            if (endGameCor == null)
                endGameCor = StartCoroutine(CoTimeForEnd());
        }else if (dist > EFIUtils.DistanceForEnd && !isEnd)
        {
            if (endGameCor != null)
            {
                StopCoroutine(endGameCor);
                endGameCor = null;
            }

            isEnemyCatch = false;
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
