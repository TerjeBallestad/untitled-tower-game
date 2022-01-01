using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] private GameObject _gameOverMenu;
    private static GameManager instance;
    private TouchManager TouchManager;
    [HideInInspector] public TowerManager TowerManager;

    // Singleton instantiation
    public static GameManager Instance {
        get {
            if (instance == null) instance = GameObject.FindObjectOfType<GameManager> ();
            return instance;
        }
    }

    void Start () {
        TouchManager = GetComponent<TouchManager> ();
        TowerManager = GetComponent<TowerManager> ();
        StartCoroutine (TowerManager.BlockSpawner ());
    }

    public void EndGame () {
        _gameOverMenu.SetActive (true);
    }

}