using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergingManager : MonoBehaviour {
    [HideInInspector] public TowerManager Tower;
    public BlockMergerPool MergerPool { get; private set; }
    private List<BlockDetector> _blockDetectors;
    public Block[] _blocks;
    private List<BlockList> Matches;

    void Start () {
        Matches = new List<BlockList> ();
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

    public void MoveBlockToIndex (Block block, int index) {
        // if _blocks contains block, nullify
        // for (int i = 0; i < _blocks.Length; i++) {
        //     if (_blocks[i] == block) {
        //         _blocks[i] = null;
        //     }
        // }
        if (block.index > 0) {
            _blocks[block.index] = null;
        }
        _blocks.SetValue (block, index);
        block.index = index;
        if (block.MergeList == null) {
            MergerPool.Get ().Setup (block, Tower, MergerPool);
        }
        TryAddToNeighbourList (block, index);
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

        List<BlockList> matches = new List<BlockList> ();

        for (int i = 0; i < _blocks.Length; i++) {
            Block currentBlock = _blocks[i];
            if (currentBlock == null) {
                continue;
            }
            if (!matches.Contains (currentBlock.MergeList)) {
                matches.Add (currentBlock.MergeList);
            }

        }
        foreach (var mergeList in matches) {
            mergeList.TryMergeBlocks (3);

        }
        // if (previousBlock == null || previousBlock.Type != currentBlock.Type) {
        //     if (currentBlock.MergeList == null) {
        //         BlockList mergeList = MergerPool.Get ();
        //         mergeList.Setup (currentBlock.Type, Tower, MergerPool);
        //         mergeList.AddUnique (currentBlock);
        //         Matches.Add (mergeList);
        //     }

        // } else {
        //     previousBlock.MergeList.AddUnique (currentBlock);
        // }
        // previousBlock = currentBlock;

        //     Block previous = null;
        //     Block next = null;
        //     int firstIndex = 0;
        //     int matches = 0;
        //     bool lastInSequence = false;

        //     for (int i = 0; i < _blocks.Length; i++) {
        //         // check if current block is not null
        //         Block currentBlock = _blocks[i];
        //         if (currentBlock == null) continue;
        //         // set next block if not last in array
        //         if (i + 1 < _blocks.Length) {
        //             next = _blocks[i + 1];
        //             if (next != null && currentBlock.Type != next.Type) {
        //                 // if next is different means this is the last in sequence
        //                 lastInSequence = true;
        //             }
        //         } else {
        //             next = null;
        //             lastInSequence = true;
        //         }
        //         // if matches are more than 2 create a list and add all the matches
        //         if (lastInSequence && matches > 19) {
        //             BlockList mergeList;
        //             if (_blocks[firstIndex].MergeList != null) {
        //                 mergeList = _blocks[firstIndex].MergeList;
        //             } else {
        //                 mergeList = MergerPool.Get ();
        //                 mergeList.Setup (Tower, MergerPool);
        //                 _blocks[firstIndex].MergeList = mergeList;
        //             }
        //             mergeList.Clear ();
        //             for (int f = firstIndex; f < i; f++) {
        //                 mergeList.Add (_blocks[f]);
        //             }
        //             continue;
        //         }
        //         // Check if previous and current
        //         if (previous != null && previous.Type == currentBlock.Type) {
        //             // increase match counter and go to next iteration
        //             matches++;
        //         } else {
        //             matches = 0;
        //             firstIndex = i;
        //         }
        //         lastInSequence = false;
        //         previous = _blocks[i];
        //     }

        //     while (_blocks != null && i < _blockDetectors.Count) {
        //         Block currentBlock = _blockDetectors[i].blockInRange;
        //         BlockType currentType = _blockDetectors[i].blockInRange.Type;
        //         BlockList mergeList;

        //         if (currentBlock.MergeList != null) {
        //             mergeList = currentBlock.MergeList;
        //         } else {
        //             mergeList = MergerPool.Get ();
        //             currentBlock.MergeList = mergeList;
        //         }
        //         mergeList.Setup (Tower, MergerPool);
        //         while (i < _blockDetectors.Count) {
        //             currentBlock = _blockDetectors[i].blockInRange;
        //             if (currentBlock != null && currentType == currentBlock.Type) {
        //                 mergeList.Add (currentBlock);
        //                 i++;

        //             } else {
        //                 break;
        //             }
        //         }
        //         mergeList.TryMergeBlocks (3);
        //     }
    }

}