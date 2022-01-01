using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
    public BlockType Type { get; private set; }

    [SerializeField] private Sprite _redSprite;
    [SerializeField] private Sprite _purpleSprite;
    [SerializeField] private Sprite _blueSprite;
    [SerializeField] private Sprite _greenSprite;
    [SerializeField] private ProgressBar _rageBar;
    private float _rage;
    private SpriteRenderer _spriteRenderer;
    private TowerManager _tower;

    private void Start () {
        _tower = GameManager.Instance.TowerManager;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer> ();
        Setup (BlockType.green);
    }
    private void Update () {
        _rage += Time.deltaTime;
        _rageBar.UpdateCurrentFill (100f, _rage, Color.red);
    }

    public void Setup (BlockType type) {
        _rage = 0;
        Type = type;
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

    private void OnTriggerEnter2D (Collider2D other) {
        Block block = other.GetComponent<Block> ();
        if (block == null || block.BeingTouched == false) return;

        if (block.Type != Type) {
            _rage += 30f;
        } else {
            _rage -= 30f;
        }

        block.GetEaten (this);
    }
}