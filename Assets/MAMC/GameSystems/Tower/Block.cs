using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    public BlockType Type;
    private int _index;
    public int Index { get { return _index; } set { _index = value; } }
    public BlockList MergeList;
    private bool _beingTouched;
    public bool BeingTouched { get { return _beingTouched; } private set { _beingTouched = value; } }

    [HideInInspector] public Vector3 InitialPosition; // Used for shaking from Monster.cs
    [SerializeField] private Material RedMaterial;
    [SerializeField] private Material PurpleMaterial;
    [SerializeField] private Material BlueMaterial;
    [SerializeField] private Material GreenMaterial;
    [SerializeField] private Material BronzeMaterial;
    [SerializeField] private Material SilverMaterial;
    [SerializeField] private Material GoldMaterial;
    [SerializeField] private Material DiamondMaterial;
    private TowerManager Tower;
    private MeshRenderer MeshRenderer;

    public void Setup (BlockType type, TowerManager tower) {
        Type = type;
        Tower = tower;
        _index = -1;
        _beingTouched = false;
        MeshRenderer = transform.GetChild (1).GetComponent<MeshRenderer> ();

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
            case BlockType.bronze:
                MeshRenderer.material = BronzeMaterial;
                break;
            case BlockType.silver:
                MeshRenderer.material = SilverMaterial;
                break;
            case BlockType.gold:
                MeshRenderer.material = GoldMaterial;
                break;
            case BlockType.diamond:
                MeshRenderer.material = DiamondMaterial;
                break;
            default:
                Debug.Log (gameObject.name + " does not have a valid type");
                break;
        }
    }

    public void GetEaten (Monster monster) {
        Tower.EatBlock (monster, this);
    }

    // private void OnCollisionEnter2D (Collision2D other) {
    //     if (!_beingTouched && _index != 0 && other.gameObject.CompareTag ("ground")) {
    //         GameManager.Instance.EndGame ();
    //     }
    // }

    public static BlockType ExclusiveRandomBlockType (BlockType excludeType, BlockType excludedType2 = BlockType.bronze) {
        List<BlockType> types = new List<BlockType> ();

        for (int i = 0; i < 4; i++) {
            BlockType current = (BlockType) i;
            if (current != excludeType && current != excludedType2) {
                types.Add (current);
            }
        }
        int r = Random.Range (0, types.Count);
        return types[r];
    }

    public IEnumerator StopTouching () {
        yield return new WaitForSeconds (1);
        _beingTouched = false;
    }
    public void StartTouching () {
        _beingTouched = true;
    }

}

public enum BlockType {
    purple,
    blue,
    red,
    green,
    bronze,
    silver,
    gold,
    diamond,
    coin,
    time,
    multiplier,
    brick,
    bomb,
    acid,
    increaseMergeQuality,
}