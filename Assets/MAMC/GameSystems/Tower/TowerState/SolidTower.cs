using System.Collections;
using UnityEngine;
public class SolidTower : PowerUp {
    public float Strength = 10f;

    public SolidTower (TowerManager tower) : base (tower) { }

    public override IEnumerator InitializeState () {
        return base.InitializeState ();
    }
    public override IEnumerator EndState () {
        return base.EndState ();
    }
    public override void UpdateState () {
        base.UpdateState ();
        foreach (var block in Tower.Blocks) {
            Debug.Log (block.name);
            block.GetComponent<Rigidbody2D> ().AddForce (new Vector3 (-block.transform.position.x * Strength, 0));

        }
    }
}