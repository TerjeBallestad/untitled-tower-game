using System.Collections;
using UnityEngine;
public class MorePoints : NormalPower {
    public float Factor = 10f;

    public MorePoints (TowerManager tower) : base (tower) { }

    public override IEnumerator InitializeState () {
        return base.InitializeState ();
    }
    public override void MergeBlocks (BlockList list) {
        base.MergeBlocks (list);
        // points = points * factor

    }
    public override IEnumerator EndState () {
        return base.EndState ();
    }
    public override void UpdateState () {
        base.UpdateState ();
    }
}