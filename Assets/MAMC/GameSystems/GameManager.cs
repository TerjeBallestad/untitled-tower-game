using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] private GameObject CondoPrefab;
    private static GameManager instance;
    private SelectionManager SelectionManager;
    public TowerManager TowerManager;

    // Singleton instantiation
    public static GameManager Instance {
        get {
            if (instance == null) instance = GameObject.FindObjectOfType<GameManager> ();
            return instance;
        }
    }

    void Start () {
        SelectionManager = GetComponent<SelectionManager> ();
        TowerManager = GetComponent<TowerManager> ();
        StartCoroutine (TowerManager.BlockSpawner ());
    }

}