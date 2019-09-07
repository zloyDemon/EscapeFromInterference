using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public static readonly float TimeForEnd = 1f;
    private const float FadeDuration = 0.8f;
    
    private static GameplayManager instance;
    private bool isEnemyCatch;
    private bool isEnd;
    private Coroutine corEndGame;
    private bool isGamePause;
    private float checkGhostDeviceValue;
    private EFIEnums.GameOverReason currentReason = EFIEnums.GameOverReason.None;
    
    public static GameplayManager Instance => instance;
    
    public Transform Player { get; set; }
    public List<GameObject> GhostOnMap { get; set; }
    
    public event Action EndGame = () => { };
    public event Action<bool> PauseGame = b => { };
    
    public bool IsGamePause
    {
        set => GamePause(value);
        get => isGamePause;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(gameObject);

        IsGamePause = false;
        MissionManager.Instance.MissionCompleted += MissionCompleted;
        Debug.Log("Awake GameplayManager");
    }

    private void Start()
    {
        DungeonManager.Instance.LevelLoaded += LevelLoaded;
        DungeonManager.Instance.LoadLevel();
    }

    private void OnDestroy()
    {
        MissionManager.Instance.MissionCompleted -= MissionCompleted;
    }

    private void LevelLoaded()
    {
        DungeonManager.Instance.LevelLoaded -= LevelLoaded;
        UIManager.Instance.BlackScrFadeInOut(FadeDuration, type =>
        {
            if(type == EFIEnums.FadeType.FadeIn)
                UIManager.Instance.ShowHideLoadingScreen(false);
        });
    }

    private void GamePause(bool isPause)
    {
        Time.timeScale = isPause ? 0 : 1;
        isGamePause = isPause;
        PauseGame(isPause);
    }

    public void MissionCompleted(MissionInfo missionInfo)
    {
        UIManager.Instance.BlackScrFadeIn(FadeDuration, () =>
        { 
            UIManager.Instance.ShowEndLevelScreen(true);
            UIManager.Instance.BlackScrFadeOut(FadeDuration, null);
        });
    }

    public void GameOver(EFIEnums.GameOverReason reason)
    {
        if (currentReason == reason)
            return;

        if (reason == EFIEnums.GameOverReason.DisableGO)
        {
            if (corEndGame != null)
            {
                StopCoroutine(corEndGame);
                corEndGame = null;
            }
        }

        if (reason == EFIEnums.GameOverReason.EnemyCatch || reason == EFIEnums.GameOverReason.FlashlightDead)
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
