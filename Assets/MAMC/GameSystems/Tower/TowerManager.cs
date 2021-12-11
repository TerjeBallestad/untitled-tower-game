using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerManager : MonoBehaviour {
    [SerializeField] public int TargetBlockCount { get; private set; } = 10;
    [SerializeField] private float SpawnInterval = 2f;
    private BlockPool blockPool;
    [SerializeField] private List<Block> blocks;
    private int similarSpawns;
    private BlockType previousSpawn;
    public bool stiffTower = true;
    private int blockCount;
    private float lastBlockSpawnTime;
    public List<BlockList> Matches;

    private void Start () {
        blockPool = GetComponent<BlockPool> ();
        blocks = new List<Block> ();
        Matches = new List<BlockList> ();
    }

    private void Update () {
        if (stiffTower) {
            StiffenTower ();
        }
    }

    public void SetState (TowerState state) {
        StartCoroutine (state.Start ());
    }

    public void CheckForMatches () {
        Matches.Clear ();
        blocks.OrderBy (b => b.transform.position.y);
        int i = 0;
        while (i < blocks.Count) {
            BlockType currentType = blocks[i].Type;
            var matchList = new BlockList ();

            while (i < blocks.Count) {
                if (currentType == blocks[i].Type) {
                    matchList.Blocks.Add (blocks[i]);
                    i++;
                } else {
                    break;
                }
            }
            if (matchList.Blocks.Count > 2) {

                MergeBlocks (matchList);
            }
            Matches.Add (matchList);
        }
    }

    private void MergeBlocks (BlockList blockList) {
        SetState (new Matching (this, blockList));
    }

    public Block GetRandomBlock () {
        Block block = blockPool.Get ();
        BlockType randomBlockType = (BlockType) Random.Range (0, 4);

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
        blocks.Add (block);
        return block;
    }

    public void DespawnBlock (Block block) {
        blocks.Remove (block);
        blockPool.ReturnToPool (block);
        blockCount--;
    }

    public void StiffenTower () {
        foreach (var block in blocks) {
            block.GetComponent<Rigidbody2D> ().AddForce (new Vector3 (-block.transform.position.x * 10, 0));
        }
    }
    void SpawnBlock () {
        Block block = GetRandomBlock ();
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

[System.Serializable]
public class BlockList {
    [SerializeField] public List<Block> Blocks;

    public BlockList () {
        Blocks = new List<Block> ();
    }
}