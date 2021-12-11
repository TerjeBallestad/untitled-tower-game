using System.Collections;
public abstract class TowerState {
    protected TowerManager Tower;
    public TowerState (TowerManager tower) {
        Tower = tower;
    }
    public virtual IEnumerator Start () {
        yield break;
    }
}