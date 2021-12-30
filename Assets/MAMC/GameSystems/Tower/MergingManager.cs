using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergingManager : MonoBehaviour {
    [HideInInspector] public TowerManager Tower;
    public BlockMergerPool MergerPool { get; private set; }
    private List<BlockDetector> _blockDetectors;
    public Block[] _blocks;

    void Start () {
        MergerPool = GetComponent<BlockMergerPool> ();
        _blockDetectors = new List<BlockDetector> ();
        _blockDetectors.AddRange (GetComponentsInChildren<BlockDetector> ());
        _blocks = new Block[_blockDetectors.Count];
        int i = 0;
        foreach (var detector in _blockDetectors) {
            detector.MergeManager = this;
            detector.index = i;
            i++;
        }
    }

    private void Update () {
        CheckForMatches ();
    }

    public void MoveBlockToIndex (Block block, int index) {
        if (block.index > 0) {
            _blocks[block.index] = null;
        }
        _blocks.SetValue (block, index);
        block.index = index;
        block.gameObject.name = index.ToString () + " - " + block.Type.ToString ();
    }

    private bool TryAddToNeighbourList (Block block, int index) {
        int above = index + 1;
        int below = index - 1;
        if (above < _blocks.Length && _blocks[above] != null && _blocks[above].MergeList != null && _blocks[above].MergeList.Type == block.Type) {
            _blocks[above].MergeList.AddUniqueRange (block.MergeList);
            Debug.Log ("merging list: " + block.name + " With something above");
            return true;
        } else if (below >= 0 && _blocks[below] != null && _blocks[below].MergeList != null && _blocks[below].MergeList.Type == block.Type) {
            _blocks[below].MergeList.AddUniqueRange (block.MergeList);
            Debug.Log ("merging list: " + block.name + " With something below");
            return true;
        }
        return false;
    }

    public bool TryNullifyBlockAt (Block block, int index) {
        if (_blocks[index] == block && block.gameObject.activeInHierarchy) {
            _blocks[index] = null;
            block.MergeList.Remove (block);
            block.index = -1;
            return true;
        } else return false;
    }

    public void CheckForMatches () {

        foreach (var detector in _blockDetectors) {
            detector.UpdateClosestBlock ();
        }
        List<BlockList> matches = new List<BlockList> ();

        int i = 0;

        while (i < _blocks.Length) {
            Block currentBlock = _blocks[i];
            if (currentBlock == null) {
                i++;
                continue;
            }
            BlockList mergeList = null;
            if (currentBlock.MergeList == null) {
                mergeList = MergerPool.Get ();
                mergeList.Setup (currentBlock, Tower, MergerPool);
                matches.Add (mergeList);
            } else mergeList = currentBlock.MergeList;
            i++;
            int j = i;
            while (j < _blocks.Length) {
                // if the block is a special block (>3) && (better block) it is ignored and we skip to the next iteration
                // so that special blocks don't clog up to tower
                if ((int?) _blocks[j]?.Type > 3 && ((int?) _blocks[j]?.Type > (int?) currentBlock.Type)) {
                    j++;
                    continue;
                }
                if (currentBlock.Type == _blocks[j]?.Type) {
                    mergeList.AddUnique (_blocks[j]);
                    j++;
                } else break;
            }
        }

        foreach (var mergeList in matches) {
            mergeList.TryMergeBlocks (3);

        }
    }

}