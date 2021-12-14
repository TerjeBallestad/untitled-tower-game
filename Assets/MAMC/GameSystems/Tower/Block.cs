using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IEatable {

    public BlockType Type;
    [SerializeField] private Material RedMaterial;
    [SerializeField] private Material PurpleMaterial;
    [SerializeField] private Material BlueMaterial;
    [SerializeField] private Material GreenMaterial;
    private TowerManager Tower;
    private MeshRenderer MeshRenderer;
    private bool grounded;

    public void Setup (BlockType type, TowerManager tower) {
        Type = type;
        Tower = tower;
        MeshRenderer = GetComponent<MeshRenderer> ();
        // matches = new List<Block> ();

        switch (Type) {
            case BlockType.green:
                MeshRenderer.material = GreenMaterial;
                break;
            case BlockType.purple:
                MeshRenderer.material = PurpleMaterial;
                break;
            case BlockType.red:
                MeshRenderer.material = RedMaterial;
                break;
            case BlockType.blue:
                MeshRenderer.material = BlueMaterial;
                break;
            default:
                Debug.Log (gameObject.name + " does not have a valid type");
                break;
        }
    }

    public void Eat () {
        Tower.EatBlock (this);
    }

    private void OnCollisionEnter2D (Collision2D other) {

        Block block = other.gameObject.GetComponent<Block> ();

        if (other.gameObject.tag == "ground") {
            grounded = true;
        }

        if (!block) return;

    }
    private void OnCollisionExit2D (Collision2D other) {
        Block block = other.gameObject.GetComponent<Block> ();

        if (other.gameObject.tag == "ground") {
            grounded = false;
        }
        if (!block || !isActiveAndEnabled) return;

    }

}

public enum BlockType {
    purple,
    blue,
    red,
    green,
}