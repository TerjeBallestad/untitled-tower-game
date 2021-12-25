using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerManager : MonoBehaviour {
    [SerializeField] private int _targetBlockCount = 10;
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
    private PowerUp _powerUp;
    public PowerUp PowerUp { get { return _powerUp; } private set { _powerUp = value; } }

    private void Start () {
        _blockPool = GetComponent<BlockPool> ();
        _blocks = new List<Block> ();
        Matches = new List<BlockList> ();
        MatchTimers = new List<IEnumerator> ();
        MergingManager.Tower = this;
        SetPowerUp (new NormalPower (this));
    }

    private void Update () {
        if (RigidTower) {
            StiffenTower ();
        }
        _powerUp.UpdateState ();
    }

    public void SetState (TowerState state) {
        StartCoroutine (state.Start ());
    }

    public void SetPowerUp (PowerUp powerUp) {
        StartCoroutine (powerUp.EndState ());
        _powerUp = powerUp;
        StartCoroutine (powerUp.InitializeState ());
    }

    public Block GetRandomBlock () {
        Block block = _blockPool.Get ();
        BlockType randomBlockType = (BlockType) Random.Range (0, 4);
        block.gameObject.name = randomBlockType.ToString ();

        if (randomBlockType == _previousSpawn) {
            _similarSpawns++;
            if (_similarSpawns > 1) {
                if ((int) randomBlockType < 1) {
                    randomBlockType = (BlockType) 3; // set it to the highest "normal" enum
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
    void SpawnRandomBlockAtTop () {
        Block block = GetRandomBlock ();
        _blocks.Add (block);
        block.transform.SetPositionAndRotation (transform.position, Quaternion.identity);
        _lastBlockSpawnTime = Time.time;
        _blockCount++;
    }

    public void SpawnBlockTypeAtLocation (BlockType type, Vector3 position, int amount) {
        for (int i = 0; i < amount; i++) {
            Block block = _blockPool.Get ();
            block.Setup (type, this);
            block.transform.position = new Vector3 (position.x, position.y + (i * block.transform.localScale.y), position.z);
            _blockCount++;
        }
    }

    bool AbleToSpawnBlock () {
        return _blockCount < _targetBlockCount && Time.time - _lastBlockSpawnTime > _spawnInterval;
    }
    public IEnumerator BlockSpawner () {
        while (true) {
            yield return new WaitUntil (AbleToSpawnBlock);
            SpawnRandomBlockAtTop ();
        }
    }
    public void EatBlock (Monster monster, Block block) {
        // If special block
        if ((int) block.Type > 3) {
            TriggerPowerUp (monster, block);
        }
        DespawnBlock (block);
    }

    private void TriggerPowerUp (Monster monster, Block block) {
        switch (monster.Type) {
            case BlockType.green:
                SetPowerUp (new DoubleBlock (this));
                break;
            case BlockType.purple:
                SetPowerUp (new DoubleBlock (this));
                break;
            case BlockType.red:
                SetPowerUp (new DoubleBlock (this));
                break;
            case BlockType.blue:
                SetPowerUp (new DoubleBlock (this));
                break;
            default:
                break;
        }
        if (block.Type == BlockType.bronze) PowerUpTimer (3);
        else if (block.Type == BlockType.silver) PowerUpTimer (4);
        else if (block.Type == BlockType.gold) PowerUpTimer (5);
        else if (block.Type == BlockType.diamond) PowerUpTimer (6);

    }

    private IEnumerator PowerUpTimer (float seconds) {
        yield return new WaitForSeconds (seconds);
        SetPowerUp (new NormalPower (this));
    }

}