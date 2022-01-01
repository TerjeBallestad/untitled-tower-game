using System.Collections;
using UnityEngine;
public class Slowmo : NormalPower {
    public float Strength = 10f;

    public Slowmo (TowerManager tower) : base (tower) { }

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