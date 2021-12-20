using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDetector : MonoBehaviour {

    public MergingManager MergeManager;
    public int index; // set by merging manager

    public void UpdateClosestBlock () {
        Collider2D collider = GetComponent<Collider2D> ();
        List<Collider2D> result = new List<Collider2D> ();
        ContactFilter2D filter = new ContactFilter2D ();
        filter.useTriggers = true;
        collider.OverlapCollider (filter, result);
        Collider2D closest = null;
        if (result.Count > 1) {
            float shortestDistance = Mathf.Infinity;
            foreach (var c in result) {
                float distance = Vector2.Distance (c.transform.position, collider.transform.position);
                if (distance < shortestDistance) {
                    shortestDistance = distance;
                    closest = c;
                    Debug.Log (c.gameObject.name + " distance " + distance);
                }
            }
        } else if (result.Count > 0) {
            closest = result[0];
        }
        if (closest != null) {
            Block block = closest.GetComponent<Block> ();
            MergeManager.MoveBlockToIndex (block, index);
        }
    }
    // private void OnTriggerEnter2D (Collider2D other) {
    //     Block block = other.GetComponent<Block> ();
    //     if (!block) return;

    //     MergeManager.MoveBlockToIndex (block, index);
    //     MergeManager.CheckForMatches ();
    // }

    // private void OnTriggerExit2D (Collider2D other) {
    //     Block block = other.GetComponent<Block> ();
    //     if (!block) return;

    //     MergeManager.TryNullifyBlockAt (block, index);
    //     MergeManager.CheckForMatches ();
    // }
}