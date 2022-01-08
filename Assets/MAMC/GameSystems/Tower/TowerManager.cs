using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerManager : MonoBehaviour {
    [SerializeField] private int _targetBlockCount = 10;
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private float _bronzePowerTime = 5, _silverPowerTime = 10, _goldPowerTime = 15, _diamondPowerTime = 20;
    [SerializeField] private MergingManager _mergingManager;
    [HideInInspector] public MergingManager MergingManager { get { return _mergingManager; } private set { _mergingManager = value; } }
    private MonsterManager _monsterManager;
    public ProgressBar progressBar;
    private List<Block> _blocks;
    public List<Block> Blocks { get { return _blocks; } private set { _blocks = value; } }
    private BlockPool _blockPool;
    private int _similarSpawns;
    private BlockType _previousSpawn;
    private int _blockCount;
    private float _lastBlockSpawnTime;
    private PowerUp _powerUp;
    public PowerUp PowerUp { get { return _powerUp; } private set { _powerUp = value; } }

    private void Start () {
        _blockPool = GetComponent<BlockPool> ();
        _monsterManager = GetComponent<MonsterManager> ();
        _blocks = new List<Block> ();
        _mergingManager.Tower = this;
        _monsterManager.SetTwoRandomMonsters ();
        SetPowerUp (new NormalPower (this));
    }

    private void Update () {
        _powerUp.UpdateState ();
    }

    public void SetState (TowerState state) {
        StartCoroutine (state.Start ());
    }

    public void SetPowerUp (PowerUp powerUp) {
        if (_powerUp != null) {
            StartCoroutine (_powerUp.EndState ());
        }
        _powerUp = powerUp;
        Debug.Log ("new power up is " + powerUp);
        StartCoroutine (_powerUp.InitializeState ());
    }

    public Block GetRandomBlock () {
        Block block = _blockPool.Get ();
        BlockType randomBlockType = (BlockType) UnityEngine.Random.Range (0, 4);
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
        if (block.Index >= 0)
            _mergingManager._blocks[block.Index] = null;
        _blocks.Remove (block);
        _blockPool.ReturnToPool (block);
        _blockCount--;
    }

    public IEnumerator CenterBlocksSlightly () {
        float end = Time.time + 0.4f;
        var instruction = new WaitForEndOfFrame ();
        while (end > Time.time) {

            foreach (var block in _blocks) {
                if (block.Index < 0) continue;
                Vector3 target = block.transform.position;
                target.x = 0;
                // Vector3 target = _mergingManager.BlockDetectors[block.index].transform.position;
                // Vector3 heading = (_mergingManager.BlockDetectors[block.index].transform.position - block.transform.position);
                // Vector3 direction = heading / heading.magnitude;
                // Debug.Log (direction);
                block.transform.rotation = Quaternion.identity;
                block.transform.position = Vector3.MoveTowards (block.transform.position, target, 0.004f);
            }
            yield return instruction;
        }
    }
    void SpawnRandomBlockAtTop () {
        Block block = GetRandomBlock ();
        _blocks.Add (block);
        block.transform.SetPositionAndRotation (transform.position, transform.rotation);
        _lastBlockSpawnTime = Time.time;
        _blockCount++;
    }

    public void SpawnBlockTypeAtLocation (BlockType type, Vector3 position, int amount) {
        for (int i = 0; i < amount; i++) {
            Block block = _blockPool.Get ();
            block.Setup (type, this);
            block.Index = 0; // or else it triggers game over if spawning on bottom
            block.transform.position = new Vector3 (position.x, position.y + (i * block.transform.localScale.y), position.z);
            _blocks.Add (block);
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
                SetPowerUp (new SolidTower (this));
                break;
            case BlockType.red:
                SetPowerUp (new SlowMo (this));
                break;
            case BlockType.blue:
                SetPowerUp (new MorePoints (this));
                break;
            default:
                break;
        }
        if (block.Type == BlockType.bronze) StartCoroutine (PowerUpTimer (_bronzePowerTime));
        else if (block.Type == BlockType.silver) StartCoroutine (PowerUpTimer (_silverPowerTime));
        else if (block.Type == BlockType.gold) StartCoroutine (PowerUpTimer (_goldPowerTime));
        else if (block.Type == BlockType.diamond) StartCoroutine (PowerUpTimer (_diamondPowerTime));

    }

    private IEnumerator PowerUpTimer (float seconds) {
        progressBar.gameObject.SetActive (true);
        _powerUp.SetTimers (seconds);
        yield return new WaitForSeconds (seconds);
        SetPowerUp (new NormalPower (this));
    }

    public void DespawnAllBlocks () {
        if (_blocks == null || _blocks.Count < 1) return;
        foreach (var block in _blocks) {
            if (block.Index >= 0)
                _mergingManager._blocks[block.Index] = null;
            _blockPool.ReturnToPool (block);
            _blockCount--;
        }
        _blocks.Clear ();
    }
}