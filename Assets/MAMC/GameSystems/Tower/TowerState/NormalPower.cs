using System.Collections;
using UnityEngine;
public class NormalPower : PowerUp {

    public NormalPower (TowerManager tower) : base (tower) { }
    public override IEnumerator InitializeState () {
        return base.InitializeState ();
    }

    public override void MergeBlocks () {
        base.MergeBlocks ();

    }
    public override void UpdateState () {
        base.UpdateState ();
    }
    public override IEnumerator EndState () {
        return base.EndState ();
    }
}