using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
    public BlockType Type { get; private set; }

    [SerializeField] private Sprite RedSprite;
    [SerializeField] private Sprite PurpleSprite;
    [SerializeField] private Sprite BlueSprite;
    [SerializeField] private Sprite GreenSprite;
    private int _rage;
    private SpriteRenderer _spriteRenderer;
    private TowerManager _tower;

    private void Start () {
        _rage = 0;
        _tower = GameManager.Instance.TowerManager;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer> ();
        Setup (BlockType.green);
    }

    public void Setup (BlockType type) {
        Type = type;
        switch (Type) {
            case BlockType.green:
                _spriteRenderer.sprite = GreenSprite;
                break;
            case BlockType.purple:
                _spriteRenderer.sprite = PurpleSprite;
                break;
            case BlockType.red:
                _spriteRenderer.sprite = RedSprite;
                break;
            case BlockType.blue:
                _spriteRenderer.sprite = BlueSprite;
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D (Collider2D other) {
        Block block = other.GetComponent<Block> ();
        if (block == null || block.BeingTouched == false) return;

        if (block.Type != Type) {
            _rage++;
        } else {
            _rage--;
        }
        block.GetEaten (this);
    }

}