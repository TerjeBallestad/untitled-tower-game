using System.Collections;
public abstract class PowerUp {
    protected TowerManager Tower;
    public PowerUp (TowerManager tower) {
        Tower = tower;
    }
    public virtual IEnumerator InitializeState () {
        yield break;
    }

    public virtual void MergeBlocks (BlockList list) {

    }

    public virtual void UpdateState () {

    }
    public virtual IEnumerator EndState () {
        yield break;
    }
}