using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour {
    private void OnCollisionEnter2D (Collision2D other) {
        Debug.Log ("collided with ground");
        Block block = other.gameObject.GetComponent<Block> ();
        if (block == null || block.Index == 0 || block.BeingTouched) return;
        GameManager.Instance.EndGame ();
    }
}