using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour {
    [SerializeField] private Monster _rightMonster, _lefMonster;

    void Start () {
        GameManager.Instance.OnGameBegin += HandleNewGame;
        GameManager.Instance.OnGameOver += HandleGameOver;
        GameManager.Instance.TowerManager.MergingManager.OnMerge += HandleMerge;
    }

    public void HandleNewGame () {
        SetTwoRandomMonsters ();
        UnPacifyMonsters ();
    }
    public void HandleGameOver () {
        PacifyMonsters ();
    }
    public void SetTwoRandomMonsters () {
        BlockType monster1 = (BlockType) Random.Range (0, 4);
        BlockType monster2 = Block.ExclusiveRandomBlockType (monster1);
        _lefMonster.Setup (monster1);
        _rightMonster.Setup (monster2);
    }

    public void HandleMonsterSwitching (BlockType newType) {
        if (_lefMonster.Type == newType) {
            _lefMonster.Setup (Block.ExclusiveRandomBlockType (newType, _rightMonster.Type));
        } else if (_rightMonster.Type == newType) {
            _rightMonster.Setup (Block.ExclusiveRandomBlockType (newType, _lefMonster.Type));
        }
    }

    public void HandleMerge (BlockType type) {
        _rightMonster.AddRage (-20);
        _lefMonster.AddRage (-20);
    }

    public void PacifyMonsters () {
        _lefMonster.Pacify ();
        _rightMonster.Pacify ();
    }
    public void UnPacifyMonsters () {
        _lefMonster.UnPacify ();
        _rightMonster.UnPacify ();

    }
}