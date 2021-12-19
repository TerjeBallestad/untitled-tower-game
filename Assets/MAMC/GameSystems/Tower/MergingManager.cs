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

    public void MoveBlockToIndex (Block block, int index) {

        for (int i = 0; i < _blocks.Length; i++) {
            if (_blocks[i] == block) {
                _blocks[i] = null;
            }
        }
        block.gameObject.name = index.ToString () + " - " + block.Type.ToString ();
        _blocks.SetValue (block, index);
    }

    public bool TryNullifyIndex (Block block, int index) {
        if (_blocks[index] == block) {
            _blocks[index] = null;
            return true;
        } else return false;
    }

    public void CheckForMatches () {

        Block previousBlock = null;
        List<BlockList> matches = new List<BlockList> ();

        for (int i = 0; i < _blocks.Length; i++) {
            Block currentBlock = _blocks[i];
            if (currentBlock == null) {
                previousBlock = null;
                continue;
            }
            if (previousBlock == null || previousBlock.Type != currentBlock.Type) {
                if (currentBlock.MergeList == null) {
                    BlockList mergeList = MergerPool.Get ();
                    mergeList.Setup (Tower, MergerPool);
                    mergeList.AddUnique (currentBlock);
                    matches.Add (mergeList);
                }

            } else {
                previousBlock.MergeList.AddUnique (currentBlock);
            }
            previousBlock = currentBlock;
        }
        foreach (var mergeList in matches) {
            mergeList.TryMergeBlocks (3);
        }

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