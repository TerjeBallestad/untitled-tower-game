using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDetector : MonoBehaviour {

    public MergingManager MergeManager;
    public int index; // set by merging manager

    private void OnTriggerEnter2D (Collider2D other) {
        Block block = other.GetComponent<Block> ();
        if (!block) return;

        MergeManager.MoveBlockToIndex (block, index);
        MergeManager.CheckForMatches ();
    }

    private void OnTriggerExit2D (Collider2D other) {
        Block block = other.GetComponent<Block> ();
        if (!block) return;

        MergeManager.TryNullifyIndex (block, index);
        MergeManager.CheckForMatches ();
    }
}