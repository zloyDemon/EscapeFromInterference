using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static readonly float TimeForEnd = 1.5f;
    
    private static GameplayManager _instance;
    private bool isEnemyCatch;
    private bool isEnd;
    private Coroutine endGameCor;
    private bool isGamePause;
    private float checkGhostDeviceValue;
    
    public static GameplayManager Instance { get { return _instance; } }
    public Action EndGame = () => { };
    public Action<bool> PauseGame = b => { };

    public bool IsGamePause
    {
        set
        {
            isGamePause = value;
            PauseGame(isGamePause);
            GamePause(isGamePause);
        }

        get { return isGamePause; }
    }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        if(_instance != this)
            Destroy(gameObject);
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

    private void GamePause(bool isPause)
    {
        Time.timeScale = isPause ? 0 : 1;
    }

    private IEnumerator CoTimeForEnd()
    {
        yield return new WaitForSeconds(TimeForEnd);
        Debug.Log("Game end");
        isEnd = true;
        EndGame();
    }
}
