using System.Collections;
using UnityEngine;
public class Matching : TowerState {

    private BlockList Matches;
    private float MatchTime = 3f;

    public Matching (TowerManager tower, BlockList matches) : base (tower) {
        Matches = matches;
    }

    public override IEnumerator Start () {
        yield return new WaitForSeconds (MatchTime);
        foreach (var block in Matches.Blocks) {
            Tower.DespawnBlock (block);
        }
    }

}