using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDetector : MonoBehaviour {

    public Block blockInRange;
    public MergingManager MergeManager;

    private void OnTriggerEnter2D (Collider2D other) {
        Block block = other.GetComponent<Block> ();
        if (!block) return;

        blockInRange = block;
        MergeManager.CheckForMatches ();
    }

    private void OnTriggerExit2D (Collider2D other) {
        Block block = other.GetComponent<Block> ();
        if (block == blockInRange) {
            blockInRange = null;
            MergeManager.CheckForMatches ();
        }
    }
}