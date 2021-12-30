using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerManager : MonoBehaviour {
    [SerializeField] private int _targetBlockCount = 10;
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private float _matchTime = 3f;
    [SerializeField] private float _bronzePowerTime = 5, _silverPowerTime = 10, _goldPowerTime = 15, _diamondPowerTime = 20;
    [SerializeField] private MergingManager _mergingManager;
    [SerializeField] private Monster _rightMonster, _lefMonster;
    public ProgressBar progressBar;
    public List<BlockDetector> BlockDetectors;
    private List<Block> _blocks;
    private BlockPool _blockPool;
    private int _similarSpawns;
    private BlockType _previousSpawn;
    public bool RigidTower;
    private int _blockCount;
    private float _lastBlockSpawnTime;
    private PowerUp _powerUp;
    public PowerUp PowerUp { get { return _powerUp; } private set { _powerUp = value; } }

    private void Start () {
        _blockPool = GetComponent<BlockPool> ();
        _blocks = new List<Block> ();
        _mergingManager.Tower = this;
        SpawnTwoRandomMonsters ();
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
        if (_powerUp != null) {
            StartCoroutine (_powerUp.EndState ());
        }
        _powerUp = powerUp;
        Debug.Log ("new power up is " + powerUp);
        StartCoroutine (_powerUp.InitializeState ());
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
            _mergingManager._blocks[block.index] = null;
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
        block.transform.SetPositionAndRotation (transform.position, transform.rotation);
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

    private void SpawnTwoRandomMonsters () {
        BlockType monster1 = (BlockType) Random.Range (0, 4);
        BlockType monster2 = ExclusiveRandomBlockType (monster1);
        _lefMonster.Setup (monster1);
        _rightMonster.Setup (monster2);
    }

    public void HandleMonsterSwitching (BlockType newType) {
        if (_lefMonster.Type == newType) {
            _lefMonster.Setup (ExclusiveRandomBlockType (newType, _rightMonster.Type));
        } else if (_rightMonster.Type == newType) {
            _rightMonster.Setup (ExclusiveRandomBlockType (newType, _lefMonster.Type));
        }
    }
    private BlockType ExclusiveRandomBlockType (BlockType excludeType, BlockType excludedType2 = BlockType.bronze) {
        List<BlockType> types = new List<BlockType> ();

        for (int i = 0; i < 4; i++) {
            if ((BlockType) i != excludeType && (BlockType) i != excludedType2) {
                Debug.Log ((BlockType) i);
                types.Add ((BlockType) i);
            }
        }
        int j = Random.Range (0, types.Count);
        return types[j];
    }

}