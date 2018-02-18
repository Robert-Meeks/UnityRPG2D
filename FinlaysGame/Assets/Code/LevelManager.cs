using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public Player Player { get; private set; }
    public CameraController Camera { get; private set; }
    public TimeSpan RunningTime { get { return DateTime.UtcNow - _started; } }
    public int CurrentTimeBonus
    {
        get
        {
            var secondDifference = (int)(BonusCutOffSeconds - RunningTime.TotalSeconds);
            return Mathf.Max(0, secondDifference) * BonusSecondmultiplier;
        }
    }

    private List<Checkpoint> _checkpoints;
    private int _currentCheckpointIndex;
    private DateTime _started;
    private int _savedPoints;

    public Checkpoint DebugSpawn; // this is for testing hence "Debug"/ it will give the ability to set a spawn point that will not be in the final game
    public int BonusCutOffSeconds;
    public int BonusSecondmultiplier;

    public void Awake()
    {
        _savedPoints = GameManager.Instance.Points;
        Instance = this;
    }

    public void Start()
    {
        _checkpoints = FindObjectsOfType<Checkpoint>().OrderBy(t => t.transform.position.x).ToList(); // creates ordered list of checkpoints out of the checkpoints that exist in the level ins smallest x position to largest x possition
        _currentCheckpointIndex = _checkpoints.Count > 0 ? 0 : -1; // so this is to check to see if there are check points in the level (for EX a boss may not have checkpoints) if there isnt a check point then -1 is a special condition that wil tell the code not to do checkpoint stuff as its not needed

        Player = FindObjectOfType<Player>();
        Camera = FindObjectOfType<CameraController>();

        _started = DateTime.UtcNow;

        /* the following he admits is inefficient but is simple so for now a good one for learning - it is inefficient for items that require a lot of processing 
           the point stars dont at all (could have a 1000) and will have zero negative effect */
        var listeners = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerRespawnListener>();
        foreach (var listener in listeners)
        {
            for (var i = _checkpoints.Count - 1; i >= 0; i--)
            {
                var distance = ((MonoBehaviour)listener).transform.position.x - _checkpoints[i].transform.position.x;
                if (distance < 0)
                {
                    continue;
                }
                _checkpoints[i].AssignObjectToCheckpoint(listener);

                break;
            }
        }

        #if UNITY_EDITOR  // This is a compile time IF. it works by: IF we are in the UNITY EDITOR then ___ ELSE ___ . 
                if (DebugSpawn != null) // this if allows us to have a debug spawn for debugging as well as defined game checkpoints (inc Start point), obviously this will not be used in the final product hence the hashtagIF
                    DebugSpawn.SpawnPlayer(Player);
                else if (_currentCheckpointIndex != -1)
                    _checkpoints[_currentCheckpointIndex].SpawnPlayer(Player);

        #else
            if(_currentCheckpointIndex != -1) // if there are checkpoints in the level check to see which one to spawn at
                _checkpoints[_currentCheckpointIndex].SpawnPlayer(Player);
    
        #endif
    }

    public void Update()
    {
        var isAtLastCheckpoint = _currentCheckpointIndex + 1 >= _checkpoints.Count;
        if (isAtLastCheckpoint)
            return;

        var distanceToNextCheckpoint = _checkpoints[_currentCheckpointIndex + 1].transform.position.x - Player.transform.position.x;
        if (distanceToNextCheckpoint >= 0)
            return;

        // if get to this point then none of the above IFs are were true, this means that we have left/passed a check point and need to log that we have 
        _checkpoints[_currentCheckpointIndex].PlayerLeftCheckpoint();
        _currentCheckpointIndex++;
        _checkpoints[_currentCheckpointIndex].PlayerHitCheckPoint();

        GameManager.Instance.AddPoints(CurrentTimeBonus);
        _savedPoints = GameManager.Instance.Points;
        _started = DateTime.UtcNow;

 
    }

    public void GoToNextLevel(string levelName)
    {
        StartCoroutine(GoToNextLevelCo(levelName));
    }

    private IEnumerator GoToNextLevelCo(string levelName)
    {
        Player.FinishLevel();
        GameManager.Instance.AddPoints(CurrentTimeBonus);

        FloatingText.Show("Level Complete!!", "checkpointText", new CenteredTextPositioner(.2f));
        yield return new WaitForSeconds(1f);

        FloatingText.Show(string.Format("{0} Points!!", GameManager.Instance.Points), "CheckpointText", new CenteredTextPositioner(.1f));
        yield return new WaitForSeconds(5f);

        if (string.IsNullOrEmpty(levelName))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("StartScreen");
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
        }
    }

    public void KillPlayer()
    {
        StartCoroutine(KillPlayerCo()); // StartCoroutine() is a unity method that Allows a method to take longer than a unity frame ie if frame rate is 60 then method can take more than 1/60 sec to run
    }

    private IEnumerator KillPlayerCo()
    {
        Player.Kill();
        Camera.IsFollowing = false;
        yield return new WaitForSeconds(2f);

        Camera.IsFollowing = true;

        if (_currentCheckpointIndex != -1)
            _checkpoints[_currentCheckpointIndex].SpawnPlayer(Player);

        _started = DateTime.UtcNow;
        GameManager.Instance.ResetPoints(_savedPoints);
    }
}

