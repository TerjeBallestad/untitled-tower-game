using System.Collections;
using UnityEngine;
public class SlowMo : NormalPower {

    public SlowMo (TowerManager tower) : base (tower) { }

    public override IEnumerator InitializeState () {
        Time.timeScale = 0.6f;
        return base.InitializeState ();
    }
    public override IEnumerator EndState () {
        Time.timeScale = 1f;
        return base.EndState ();
    }
    public override void UpdateState () {
        base.UpdateState ();
    }
}