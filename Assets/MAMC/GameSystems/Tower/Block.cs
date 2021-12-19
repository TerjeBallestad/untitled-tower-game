using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IEatable {

    public BlockType Type;
    public int index;
    public BlockList MergeList;
    [SerializeField] private Material RedMaterial;
    [SerializeField] private Material PurpleMaterial;
    [SerializeField] private Material BlueMaterial;
    [SerializeField] private Material GreenMaterial;
    private TowerManager Tower;
    private MeshRenderer MeshRenderer;

    public void Setup (BlockType type, TowerManager tower) {
        Type = type;
        Tower = tower;
        index = -1;
        MeshRenderer = GetComponent<MeshRenderer> ();

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

    public void GetEaten () {
        Tower.EatBlock (this);
    }

}

public enum BlockType {
    purple,
    blue,
    red,
    green,
}