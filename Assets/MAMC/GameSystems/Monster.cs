using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
    private int _rage;
    public BlockType Type;
    private TowerManager _tower;

    private void Start () {
        _rage = 0;
        Type = BlockType.green;
        _tower = GameManager.Instance.TowerManager;
    }

    private void OnTriggerEnter2D (Collider2D other) {
        Block block = other.GetComponent<Block> ();
        if (block == null) return;
        if (block.Type != Type) {
            _rage++;
        } else {
            _rage--;
        }
        block.GetEaten (this);
    }

}