using UnityEngine;

public class BlockPool : ObjectPool<Block> {
    public override void OnObjectCreation (Block newBlock) {
        Rigidbody2D rb = newBlock.GetComponent<Rigidbody2D> ();
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints2D.None;

    }
    public override void OnObjectReturn (Block block) {
        if (block.MergeList != null) {
            block.MergeList.Remove (block);
        }
    }

}