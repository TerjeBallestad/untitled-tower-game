using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockList : MonoBehaviour {
    public int Count;
    public float MergeTime = 3f;
    public BlockType Type;
    public TowerManager Tower;
    private Coroutine MergeTimer;
    private BlockMergerPool _mergerPool;
    public List<Block> Blocks;
    public Block Origin;

    public void Setup (Block origin, TowerManager tower, BlockMergerPool mergerPool) {
        Tower = tower;
        Origin = origin;
        Type = origin.Type;
        Blocks = new List<Block> ();
        _mergerPool = mergerPool;
        AddUnique (origin);
        Count = Blocks.Count;
        gameObject.name = origin.name + " Merger";
    }
    public void Clear () {
        foreach (var block in Blocks) {
            if (block.MergeList == this) {
                block.MergeList = null;
            }
        }
        Count = 0;
        Blocks.Clear ();
        _mergerPool.ReturnToPool (this);
    }
    public void Add (Block block) {

        Blocks.Add (block);
        block.MergeList = this;
        if (block.transform.position.y < Origin.transform.position.y) {
            Origin = block;
            gameObject.name = Origin.name + " Merger";
        }
    }
    public bool AddUnique (Block block) {
        if (!Contains (block)) {
            Add (block);
            return true;
        }
        return false;
    }

    public void AddUniqueRange (BlockList list) {

        foreach (var block in list.Blocks) {
            Add (block);
        }
        list.Clear ();
        // _mergerPool.ReturnToPool (list);
    }
    public bool Contains (Block block) {
        return Blocks.Contains (block);
    }

    public bool Remove (Block block) {
        if (block.MergeList == this) {
            block.MergeList = null;
        }
        bool returnvalue = Blocks.Remove (block);
        if (Blocks.Count < 1) {
            _mergerPool.ReturnToPool (this);
        }
        return returnvalue;
    }

    public void TryMergeBlocks (float mergeTime) {
        if (Count < Blocks.Count && Blocks.Count >= 3) {
            Debug.Log (gameObject.name);
            MergeTime = Time.time + mergeTime;
            if (MergeTimer != null) {
                StopCoroutine (MergeTimer);
            }
            MergeTimer = StartCoroutine (MergeBlocks ());
            Count = Blocks.Count;
        } else if (Blocks.Count < 3) {
            StopAllCoroutines ();
            // Clear ();
            // _mergerPool.ReturnToPool (this);
        }
    }

    private bool TimeToMerge () {
        return MergeTime < Time.time;
    }

    private IEnumerator MergeBlocks () {
        var instruction = new WaitForEndOfFrame ();
        while (!TimeToMerge ()) {
            foreach (var block in Blocks) {
                Rigidbody2D rb = block.GetComponent<Rigidbody2D> ();
                rb.constraints = RigidbodyConstraints2D.FreezePosition;

            }
            Debug.Log ((MergeTime - Time.time));
            yield return instruction;
        }
        Debug.Log ("timer ended");
        foreach (var block in Blocks) {

            Tower.DespawnBlock (block);

        }
        Clear ();
        // _mergerPool.ReturnToPool (this);
    }
}