using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
    [Range (1, 100)] public int Rage;
    private BlockType _blockType;

    private void Start () {
        _blockType = BlockType.green;
    }

    private void OnTriggerEnter2D (Collider2D other) {
        other.GetComponent<IEatable> ().GetEaten ();
    }
}