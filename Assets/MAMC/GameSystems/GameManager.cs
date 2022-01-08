using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] private GameObject _gameOverMenu;
    private static GameManager instance;
    private TouchManager TouchManager;
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
        StartCoroutine (TowerManager.BlockSpawner ());
    }

    public void EndGame () {
        _gameOverMenu.SetActive (true);
        TowerManager.StopAllCoroutines ();
        OnGameOver?.Invoke ();
    }

    public void StartNewGame () {
        TowerManager.DespawnAllBlocks ();
        StartCoroutine (TowerManager.BlockSpawner ());
        _gameOverMenu.SetActive (false);
        OnGameBegin?.Invoke ();
    }

}