using System.Collections;
using UnityEngine;
public class InitiateTower : TowerState {

    public InitiateTower (TowerManager tower) : base (tower) { }
    public override IEnumerator Start () {
        yield return new WaitForSeconds (2f);
    }
}