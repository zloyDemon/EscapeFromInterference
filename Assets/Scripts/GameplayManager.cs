using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private static readonly float distanceForEnd = 1.2f;
    private static readonly float timeForEnd = 1.5f;
    
    private static GameplayManager _instance;
    private bool isEnemyCatch;
    private bool isEnd;
    private Coroutine endGameCor;
    
    public static GameplayManager Instance { get { return _instance; } }
    public Action EndGame = () => { };
    
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
        if (!isEnemyCatch && dist < distanceForEnd)
        {
            isEnemyCatch = true;
            if (endGameCor == null)
                endGameCor = StartCoroutine(CoTimeForEnd());
        }else if (dist > distanceForEnd && !isEnd)
        {
            if (endGameCor != null)
            {
                StopCoroutine(endGameCor);
                endGameCor = null;
            }

            isEnemyCatch = false;
        }           
    }

    private IEnumerator CoTimeForEnd()
    {
        yield return new WaitForSeconds(timeForEnd);
        Debug.Log("Game end");
        isEnd = true;
        EndGame();
    }
}
