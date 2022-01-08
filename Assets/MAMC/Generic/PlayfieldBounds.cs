using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldBounds : MonoBehaviour {
    private void OnCollisionExit2D (Collision2D other) {
        Block block = other.gameObject.GetComponent<Block> ();
        if (block == null) return;
        if (block.Index != 0) {
            GameManager.Instance.EndGame ();
        }
    }
}