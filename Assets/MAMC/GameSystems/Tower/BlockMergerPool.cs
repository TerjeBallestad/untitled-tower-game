public class BlockMergerPool : ObjectPool<BlockList> {

    public override void OnObjectCreation (BlockList newList) { }
    public override void OnObjectReturn (BlockList blockList) {
        blockList.Clear ();
    }
}