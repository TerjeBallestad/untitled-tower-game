using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
    public BlockType Type { get; private set; }

    [SerializeField] private float shakeMagnitude = 0.7f;
    [SerializeField] private Sprite _redSprite;
    [SerializeField] private Sprite _purpleSprite;
    [SerializeField] private Sprite _blueSprite;
    [SerializeField] private Sprite _greenSprite;
    [SerializeField] private ProgressBar _rageBar;
    public bool Pacified;
    private bool _shaking = false;
    private float _rage;
    private SpriteRenderer _spriteRenderer;
    private TowerManager _tower;

    private void Start () {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer> ();
        Setup (BlockType.green);
    }
    private void Update () {
        if (Pacified) return;
        _rage += Time.deltaTime;
        UpdateUI ();
        if (_rage > 99 && !_shaking) {
            StartCoroutine (ShakeTower (1f));
        }
    }

    public void Setup (BlockType type) {
        _rage = 0;
        Type = type;
        _tower = GameManager.Instance.TowerManager;
        _rageBar.gameObject.SetActive (true);
        switch (Type) {
            case BlockType.green:
                _spriteRenderer.sprite = _greenSprite;
                break;
            case BlockType.purple:
                _spriteRenderer.sprite = _purpleSprite;
                break;
            case BlockType.red:
                _spriteRenderer.sprite = _redSprite;
                break;
            case BlockType.blue:
                _spriteRenderer.sprite = _blueSprite;
                break;
            default:
                break;
        }
    }

    public void Pacify () {
        Pacified = true;
    }

    public void UnPacify () {
        Pacified = false;
    }

    public void AddRage (int rage) {
        if (Pacified) return;
        _rage += rage;
        UpdateUI ();
    }

    private void OnTriggerEnter2D (Collider2D other) {
        Block block = other.GetComponent<Block> ();
        if (block == null || block.BeingTouched == false) return;

        if ((int) block.Type < 4 && block.Type != Type) {
            AddRage (30);
        } else {
            AddRage (-30);
        }

        block.GetEaten (this);
    }

    private IEnumerator ShakeTower (float seconds) {
        _rage -= 10f;
        _shaking = true;
        Vector3 cameraInitialPosition = Camera.main.transform.position;
        List<Block> blocks = new List<Block> (_tower.Blocks);
        float endTime = Time.time + seconds;
        var instruction = new WaitForEndOfFrame ();
        foreach (var block in blocks) {
            block.InitialPosition = block.transform.position;
        }
        while (endTime > Time.time) {
            _rage -= Time.deltaTime * 30f;
            Camera.main.transform.position = cameraInitialPosition + Random.insideUnitSphere * shakeMagnitude;
            foreach (var block in blocks) {
                Vector3 target = block.InitialPosition + Random.insideUnitSphere * shakeMagnitude;
                target.y = block.transform.position.y;
                target.z = block.InitialPosition.z;
                block.transform.position = Vector3.MoveTowards (block.transform.position, target, 1);
            }
            yield return instruction;
        }
        Camera.main.transform.position = cameraInitialPosition;
        _shaking = false;
    }

    private void UpdateUI () {
        Color color = Color.grey;
        if (_rage > 80) color = Color.red;
        else if (_rage > 60) color = Color.yellow;
        else if (_rage > 40) color = Color.green;
        else if (_rage > 20) color = Color.blue;
        _rageBar.UpdateCurrentFill (100f, _rage, color);
    }
}