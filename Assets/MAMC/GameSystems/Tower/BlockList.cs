using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockList : MonoBehaviour {
    public int Count;
    public float MergeTime = 3f;
    public TowerManager Tower;
    private IEnumerator MergeTimer;
    private BlockMergerPool _mergerPool;
    public List<Block> Blocks;

    public void Setup (TowerManager tower, BlockMergerPool mergerPool) {
        Tower = tower;
        Blocks = new List<Block> ();
        MergeTimer = MergeBlocks ();
        _mergerPool = mergerPool;
    }
    public void Clear () {
        Blocks.Clear ();
    }
    public void Add (Block block) {
        Blocks.Add (block);
    }
    public bool Contains (Block block) {
        return Blocks.Contains (block);
    }

    public bool Remove (Block block) {
        return Blocks.Remove (block);
    }

    public void TryMergeBlocks (float mergeTime) {
        MergeTime = mergeTime;

        if (Count != Blocks.Count && Blocks.Count > 2) {
            StopCoroutine (MergeTimer);
            StartCoroutine (MergeTimer);
            Count = Blocks.Count;
        }
    }

    private IEnumerator MergeBlocks () {
        foreach (var block in Blocks) {
            Rigidbody2D rb = block.GetComponent<Rigidbody2D> ();
            rb.constraints = RigidbodyConstraints2D.FreezePosition;

        }
        yield return new WaitForSeconds (MergeTime);
        foreach (var block in Blocks) {
            Tower.DespawnBlock (block);
            block.MergeList = null;
        }
        Clear ();
        _mergerPool.ReturnToPool (this);
    }
}