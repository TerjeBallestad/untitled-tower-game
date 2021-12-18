using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergingManager : MonoBehaviour {
    public BlockMergerPool MergerPool { get; private set; }
    private List<BlockDetector> _blockDetectors;
    [HideInInspector] public TowerManager Tower;
    void Start () {
        MergerPool = GetComponent<BlockMergerPool> ();
        _blockDetectors = new List<BlockDetector> ();
        _blockDetectors.AddRange (GetComponentsInChildren<BlockDetector> ());
        foreach (var detector in _blockDetectors) {

            detector.MergeManager = this;
        }
    }

    public void CheckForMatches () {
        int i = 0;
        while (_blockDetectors[i].blockInRange != null && i < _blockDetectors.Count) {
            Block currentBlock = _blockDetectors[i].blockInRange;
            BlockType currentType = _blockDetectors[i].blockInRange.Type;
            BlockList mergeList;

            if (currentBlock.MergeList != null) {
                mergeList = currentBlock.MergeList;
            } else {
                mergeList = MergerPool.Get ();
                currentBlock.MergeList = mergeList;
            }
            mergeList.Setup (Tower, MergerPool);
            while (i < _blockDetectors.Count) {
                currentBlock = _blockDetectors[i].blockInRange;
                if (currentBlock != null && currentType == currentBlock.Type) {
                    mergeList.Add (currentBlock);
                    i++;

                } else {
                    break;
                }
            }
            mergeList.TryMergeBlocks (3);
        }
    }

}