using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDetector : MonoBehaviour {
    public TowerManager Tower;
    public Block blockInRange;

    private void OnTriggerEnter2D (Collider2D other) {
        Block block = other.GetComponent<Block> ();
        if (!block) return;

        blockInRange = block;

        Tower.NewCheckForMatches ();
    }

    private void OnTriggerExit2D (Collider2D other) {
        Block block = other.GetComponent<Block> ();
        if (block == blockInRange) {
            blockInRange = null;

        }
    }
}