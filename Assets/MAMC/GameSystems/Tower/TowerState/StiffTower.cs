using System.Collections;
using UnityEngine;
public class RigidTower : TowerState {

    public RigidTower (TowerManager tower) : base (tower) { }
    public override IEnumerator Start () {
        yield return new WaitForSeconds (2f);

    }
}