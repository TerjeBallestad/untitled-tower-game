using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] private GameObject _gameOverMenu;
    private static GameManager instance;
    private TouchManager TouchManager;
    [SerializeField] private float _gameDuration = 120;
    private Timer _timer;
    [HideInInspector] public TowerManager TowerManager;
    [HideInInspector] public event Action OnGameBegin;
    [HideInInspector] public event Action OnGameOver;

    // Singleton instantiation
    public static GameManager Instance {
        get {
            if (instance == null) instance = GameObject.FindObjectOfType<GameManager> ();
            return instance;
        }
    }

    void Awake () {
        TouchManager = GetComponent<TouchManager> ();
        TowerManager = GetComponent<TowerManager> ();
        _timer = GetComponent<Timer> ();
        // StartCoroutine (TowerManager.BlockSpawner ());
    }
    private void Start () {
        StartNewGame ();
    }

    public void EndGame () {
        _gameOverMenu.SetActive (true);
        TowerManager.StopAllCoroutines ();
        OnGameOver?.Invoke ();
    }

    public void StartNewGame () {
        _timer.SetGameDuration (_gameDuration);
        TowerManager.DespawnAllBlocks ();
        StartCoroutine (TowerManager.BlockSpawner ());
        _gameOverMenu.SetActive (false);
        OnGameBegin?.Invoke ();
    }

}