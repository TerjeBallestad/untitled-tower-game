using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    // Start is called before the first frame update
    void Start () {

    }
    private void OnTriggerEnter2D (Collider2D other) {
        other.GetComponent<IEatable> ().Eat ();
    }

    // Update is called once per frame
    void Update () {

    }
}