using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IEatable {

    public BlockType Type;
    public Block Above;
    public Block Below;
    [SerializeField] private float GracePeriod = 3;
    [SerializeField] private Material RedMaterial;
    [SerializeField] private Material PurpleMaterial;
    [SerializeField] private Material BlueMaterial;
    [SerializeField] private Material GreenMaterial;
    private TowerManager Tower;
    private MeshRenderer MeshRenderer;
    private List<Block> matches;
    private bool grounded;
    private IEnumerator aboveNullifier;
    private IEnumerator belowNullifier;

    public void Setup (BlockType type, TowerManager tower) {
        Type = type;
        Tower = tower;
        MeshRenderer = GetComponent<MeshRenderer> ();
        aboveNullifier = DelayedNullAbove ();
        belowNullifier = DelayedNullBelow ();
        matches = new List<Block> ();

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

    private void OnDisable () {
        StopAllCoroutines ();
    }

    private void Update () {
        if (grounded) {
            // CheckForMatches ();
        }
    }

    public void CheckForMatches () {
        Block currentBlock = this;
        Block origin = this;

        int count = 0;
        for (int i = 0; i < Tower.TargetBlockCount; i++) {

            if (currentBlock.Type == Type) {
                matches.Add (currentBlock);
                if (currentBlock.Above) {
                    currentBlock = currentBlock.Above;
                } else {
                    break;
                }
            }
        }
        // if (currentBlock) {
        //     currentBlock.CheckForMatches ();
        // }
        Debug.Log (count);
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

        if (block.transform.position.y > transform.position.y) {
            Above = block;
            StopCoroutine (aboveNullifier);
        } else {
            Below = block;
            StopCoroutine (belowNullifier);
        }
    }
    private void OnCollisionExit2D (Collision2D other) {
        Block block = other.gameObject.GetComponent<Block> ();

        if (other.gameObject.tag == "ground") {
            grounded = false;
        }
        Tower.CheckForMatches ();
        if (!block || !isActiveAndEnabled) return;

        if (block.transform.position.y > transform.position.y) {
            StartCoroutine (aboveNullifier);
        } else {
            StartCoroutine (belowNullifier);
        }
    }

    private IEnumerator DelayedNullAbove () {
        yield return new WaitForSeconds (GracePeriod);
        Above = null;
    }

    private IEnumerator DelayedNullBelow () {
        yield return new WaitForSeconds (GracePeriod);
        Below = null;
    }

}

public enum BlockType {
    purple,
    blue,
    red,
    green,
}