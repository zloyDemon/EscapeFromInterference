using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public enum GameOverReason
    {
        FlashlightDead,
        EnemyCatch,
        DisableGO,
        None,
    }
    
    public static readonly float TimeForEnd = 1f;

    private MissionInfo currentMissionInfo;
    private static GameplayManager instance;
    private bool isEnemyCatch;
    private bool isEnd;
    private Coroutine corEndGame;
    private bool isGamePause;
    private float checkGhostDeviceValue;
    private GameOverReason currentReason = GameOverReason.None;
    
    public static GameplayManager Instance => instance;
    public Transform Player { get; set; }
    public List<GameObject> GhostOnMap { get; set; }
    
    public event Action EndGame = () => { };
    public event Action<bool> PauseGame = b => { };
    
    public bool IsGamePause
    {
        set
        {
            isGamePause = value;
            GamePause(isGamePause);
        }

        get => isGamePause;
    }

    public MissionInfo CurrentMissionInfo => currentMissionInfo;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(gameObject);

        IsGamePause = false;
        Debug.Log("Awake GameplayManager");
    }

    private void Start()
    {
        DungeonManager.Instance.LevelLoaded += InstanceOnLevelLoaded;
        DungeonManager.Instance.LoadLevel();
    }

    private void InstanceOnLevelLoaded()
    {
        DungeonManager.Instance.LevelLoaded -= InstanceOnLevelLoaded;
        UIManager.Instance.ShowHideLoadingScreen(false);
        UIManager.Instance.BlackScrFadeOut(0.8f, () =>
        {
            currentMissionInfo = DungeonManager.Instance.CurrentMissionInfo;
        });
    }

    private void GamePause(bool isPause)
    {
        Time.timeScale = isPause ? 0 : 1;
        PauseGame(isPause);
    }

    public void MissionComplete()
    {
        UIManager.Instance.BlackScrFadeIn(0.8f, () =>
        { 
            GameItems.Instance.ResetItems();
            SceneManager.LoadScene(0);
        });
    }

    public void GameOver(GameOverReason reason)
    {
        if (currentReason == reason)
            return;

        if (reason == GameOverReason.DisableGO)
        {
            if (corEndGame != null)
            {
                StopCoroutine(corEndGame);
                corEndGame = null;
            }
        }

        if (reason == GameOverReason.EnemyCatch || reason == GameOverReason.FlashlightDead)
        {
            if (corEndGame == null)
                corEndGame = StartCoroutine(CorEndGame());
        }

        currentReason = reason;
    }

    private IEnumerator CorEndGame()
    {
        yield return new WaitForSeconds(TimeForEnd);
        EndGame();
        IsGamePause = true;
    }
}
