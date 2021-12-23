using System.Collections;
using UnityEngine;
public class NormalState : TowerState {

    public NormalState (TowerManager tower) : base (tower) { }
    public override IEnumerator Start () {
        yield return new WaitForSeconds (2f);
    }
}