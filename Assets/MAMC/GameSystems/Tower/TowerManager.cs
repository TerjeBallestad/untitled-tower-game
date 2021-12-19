using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerManager : MonoBehaviour {
    [SerializeField] public int TargetBlockCount { get; private set; } = 10;
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private float _matchTime = 3f;
    [SerializeField] private MergingManager MergingManager;
    private BlockPool _blockPool;
    [SerializeField] private List<Block> _blocks;
    private int _similarSpawns;
    private BlockType _previousSpawn;
    public bool RigidTower = true;
    private int _blockCount;
    private float _lastBlockSpawnTime;
    public List<BlockList> Matches;
    public List<IEnumerator> MatchTimers;
    public List<BlockDetector> BlockDetectors;

    private void Start () {
        _blockPool = GetComponent<BlockPool> ();
        _blocks = new List<Block> ();
        Matches = new List<BlockList> ();
        MatchTimers = new List<IEnumerator> ();
        MergingManager.Tower = this;
        for (int i = 0; i < BlockDetectors.Count; i++) {
            BlockList BL = gameObject.AddComponent<BlockList> ();
            Matches.Add (BL);
        }
    }

    private void Update () {
        if (RigidTower) {
            StiffenTower ();
        }
    }

    public void SetState (TowerState state) {
        StartCoroutine (state.Start ());
    }

    // public void NewCheckForMatches () {
    //     int i = 0;
    //     foreach (var list in Matches) {
    //         list.Clear ();
    //     }
    //     while (BlockDetectors[i].blockInRange != null && i < BlockDetectors.Count) {
    //         BlockType currentType = BlockDetectors[i].blockInRange.Type;
    //         BlockList matchList = Matches[i];
    //         while (i < BlockDetectors.Count) {
    //             if (BlockDetectors[i].blockInRange != null && currentType == BlockDetectors[i].blockInRange.Type) {
    //                 matchList.Add (BlockDetectors[i].blockInRange);
    //                 i++;
    //             } else {
    //                 break;
    //             }
    //         }
    //     }
    // }

    // public void CheckForMatches () {
    //     Matches.Clear ();
    //     _blocks.OrderBy (b => b.transform.position.y);
    //     int i = 0;
    //     while (i < _blocks.Count) {
    //         BlockType currentType = _blocks[i].Type;
    //         var matchList = gameObject.AddComponent<BlockList> ();
    //         while (i < _blocks.Count) {
    //             if (currentType == _blocks[i].Type) {
    //                 matchList.Add (_blocks[i]);
    //                 i++;
    //             } else {
    //                 break;
    //             }
    //         }
    // Matches[i].TryMergeBlocks (3);
    //     }

    // for (int j = 0; j < Matches.Count; j++) {
    // if (MatchTimers[j] == null) {
    // IEnumerator E = MergeBlocks (Matches[j]);
    // }
    // StopCoroutine (MatchTimers[j]);
    // StartCoroutine (MatchTimers[j]);

    // }
    // for (var list in Matches) {
    //     if (list.Blocks.Count > 2) {
    //         MergeBlocks (list);
    //     }
    // }

    // }

    public Block GetRandomBlock () {
        Block block = _blockPool.Get ();
        BlockType randomBlockType = (BlockType) Random.Range (0, 4);
        block.gameObject.name = randomBlockType.ToString ();

        if (randomBlockType == _previousSpawn) {
            _similarSpawns++;
            if (_similarSpawns > 1) {
                if ((int) randomBlockType < 1) {
                    randomBlockType = (BlockType) 3; // set it to the highest enum
                } else {
                    randomBlockType--;
                }
            }
        } else {
            _similarSpawns = 0;
        }
        _previousSpawn = randomBlockType;

        block.Setup (randomBlockType, this);
        return block;
    }

    public void DespawnBlock (Block block) {
        if (block.index >= 0)
            MergingManager._blocks[block.index] = null;
        _blocks.Remove (block);
        _blockPool.ReturnToPool (block);
        _blockCount--;
    }

    public void StiffenTower () {
        foreach (var block in _blocks) {
            block.GetComponent<Rigidbody2D> ().AddForce (new Vector3 (-block.transform.position.x * 10, 0));
        }
    }
    void SpawnBlock () {
        Block block = GetRandomBlock ();
        _blocks.Add (block);
        block.transform.SetPositionAndRotation (transform.position, Quaternion.identity);
        _lastBlockSpawnTime = Time.time;
        _blockCount++;
    }

    bool AbleToSpawnBlock () {
        return _blockCount < TargetBlockCount && Time.time - _lastBlockSpawnTime > _spawnInterval;
    }
    public IEnumerator BlockSpawner () {
        while (true) {
            yield return new WaitUntil (AbleToSpawnBlock);
            SpawnBlock ();
        }
    }
    public void EatBlock (Block block) {
        DespawnBlock (block);
    }

}