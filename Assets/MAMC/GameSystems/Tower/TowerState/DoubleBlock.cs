using System.Collections;
using UnityEngine;
public class DoubleBlock : PowerUp {

    public DoubleBlock (TowerManager tower) : base (tower) { }
    public override IEnumerator InitializeState () {
        return base.InitializeState ();
    }

    public override void MergeBlocks (BlockList list) {
        base.MergeBlocks (list);
        int amountToSpawn = Mathf.FloorToInt (list.Count / 3) * 2;
        switch (list.Type) {
            case BlockType.green:
                Tower.SpawnBlockTypeAtLocation (BlockType.bronze, list.Origin.transform.position, amountToSpawn);
                break;
            case BlockType.purple:
                Tower.SpawnBlockTypeAtLocation (BlockType.bronze, list.Origin.transform.position, amountToSpawn);
                break;
            case BlockType.red:
                Tower.SpawnBlockTypeAtLocation (BlockType.bronze, list.Origin.transform.position, amountToSpawn);
                break;
            case BlockType.blue:
                Tower.SpawnBlockTypeAtLocation (BlockType.bronze, list.Origin.transform.position, amountToSpawn);
                break;
            case BlockType.bronze:
                Tower.SpawnBlockTypeAtLocation (BlockType.silver, list.Origin.transform.position, amountToSpawn);
                break;
            case BlockType.silver:
                Tower.SpawnBlockTypeAtLocation (BlockType.gold, list.Origin.transform.position, amountToSpawn);
                break;
            case BlockType.gold:
                Tower.SpawnBlockTypeAtLocation (BlockType.diamond, list.Origin.transform.position, amountToSpawn);
                break;
            case BlockType.diamond:
                Tower.SpawnBlockTypeAtLocation (BlockType.diamond, list.Origin.transform.position, amountToSpawn);
                break;
            default:
                Debug.Log (list.gameObject.name + " does not have a valid type");
                break;
        }
    }
    public override void UpdateState () {
        currentTimeRemaining = currentTimeRemaining -= Time.deltaTime;
        if (currentTimeRemaining < 0) Tower.progressBar.gameObject.SetActive (false);
        else Tower.progressBar.UpdateCurrentFill (maxTimeRemaining, currentTimeRemaining, Color.green);
    }
    public override IEnumerator EndState () {
        return base.EndState ();
    }
}