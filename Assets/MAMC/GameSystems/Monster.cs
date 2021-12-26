using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
    public BlockType Type { get; private set; }

    [SerializeField] private Material RedMaterial;
    [SerializeField] private Material PurpleMaterial;
    [SerializeField] private Material BlueMaterial;
    [SerializeField] private Material GreenMaterial;
    private int _rage;
    private MeshRenderer _meshRenderer;
    private TowerManager _tower;

    private void Start () {
        _rage = 0;
        _tower = GameManager.Instance.TowerManager;
        _meshRenderer = GetComponent<MeshRenderer> ();
        Setup (BlockType.green);
    }

    public void Setup (BlockType type) {
        Type = type;
        switch (Type) {
            case BlockType.green:
                _meshRenderer.material = GreenMaterial;
                break;
            case BlockType.purple:
                _meshRenderer.material = PurpleMaterial;
                break;
            case BlockType.red:
                _meshRenderer.material = RedMaterial;
                break;
            case BlockType.blue:
                _meshRenderer.material = BlueMaterial;
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D (Collider2D other) {
        Block block = other.GetComponent<Block> ();
        if (block == null) return;
        if (block.Type != Type) {
            _rage++;
        } else {
            _rage--;
        }
        block.GetEaten (this);
    }

}