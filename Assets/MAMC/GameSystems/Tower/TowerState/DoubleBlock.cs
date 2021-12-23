using System.Collections;
using UnityEngine;
public class DoubleBlock : PowerUp {

    public DoubleBlock (TowerManager tower) : base (tower) { }
    public override IEnumerator InitializeState () {
        return base.InitializeState ();
    }

    public override void MergeBlocks () {
        base.MergeBlocks ();

    }

    private void Update () {
        UpdateState ();
    }
    public override void UpdateState () {
        base.UpdateState ();
    }
    public override IEnumerator EndState () {
        return base.EndState ();
    }
}