using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerManager : MonoBehaviour {
    [SerializeField] public int TargetBlockCount { get; private set; } = 10;
    [SerializeField] private float SpawnInterval = 2f;
    [SerializeField] private float MatchTime = 3f;
    private BlockPool BlockPool;
    [SerializeField] private List<Block> Blocks;
    private int similarSpawns;
    private BlockType previousSpawn;
    public bool rigidTower = true;
    private int blockCount;
    private float lastBlockSpawnTime;
    public List<BlockList> Matches;
    public List<IEnumerator> MatchTimers;

    private void Start () {
        BlockPool = GetComponent<BlockPool> ();
        Blocks = new List<Block> ();
        Matches = new List<BlockList> ();
        MatchTimers = new List<IEnumerator> ();
        for (int i = 0; i < TargetBlockCount; i++) {
            Matches.Add (gameObject.AddComponent<BlockList> ());
        }
    }

    private void Update () {
        if (rigidTower) {
            StiffenTower ();
        }
    }

    public void SetState (TowerState state) {
        StartCoroutine (state.Start ());
    }

    public void CheckForMatches () {
        Matches.Clear ();
        Blocks.OrderBy (b => b.transform.position.y);
        int i = 0;
        while (i < Blocks.Count) {
            BlockType currentType = Blocks[i].Type;

            while (i < Blocks.Count) {
                if (currentType == Blocks[i].Type) {
                    Matches[i].Setup (this);
                    Matches[i].Add (Blocks[i]);
                    i++;
                } else {
                    break;
                }
            }
            Matches[i].TryMergeBlocks (3);
        }
        for (int j = 0; j < Matches.Count; j++) {
            if (MatchTimers[j] == null) {
                // IEnumerator E = MergeBlocks (Matches[j]);
            }
            // StopCoroutine (MatchTimers[j]);
            // StartCoroutine (MatchTimers[j]);

            // }
            // for (var list in Matches) {
            //     if (list.Blocks.Count > 2) {
            //         MergeBlocks (list);
            //     }
            // }
        }
    }

    public Block GetRandomBlock () {
        Block block = BlockPool.Get ();
        BlockType randomBlockType = (BlockType) Random.Range (0, 4);
        block.gameObject.name = randomBlockType.ToString ();

        if (randomBlockType == previousSpawn) {
            similarSpawns++;
            if (similarSpawns > 1) {
                if ((int) randomBlockType < 1) {
                    randomBlockType = (BlockType) 3; // set it to the highest enum
                } else {
                    randomBlockType--;
                }
            }
        } else {
            similarSpawns = 0;
        }
        previousSpawn = randomBlockType;

        block.Setup (randomBlockType, this);
        return block;
    }

    public void DespawnBlock (Block block) {
        Blocks.Remove (block);
        BlockPool.ReturnToPool (block);
        blockCount--;
    }

    public void StiffenTower () {
        foreach (var block in Blocks) {
            block.GetComponent<Rigidbody2D> ().AddForce (new Vector3 (-block.transform.position.x * 10, 0));
        }
    }
    void SpawnBlock () {
        Block block = GetRandomBlock ();
        Blocks.Add (block);
        block.transform.SetPositionAndRotation (transform.position, Quaternion.identity);
        lastBlockSpawnTime = Time.time;
        blockCount++;
    }

    bool AbleToSpawnBlock () {
        return blockCount < TargetBlockCount && Time.time - lastBlockSpawnTime > SpawnInterval;
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