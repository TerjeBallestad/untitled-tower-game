using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour {

    public Block SelectedBlock;
    public Vector3 initialTouchPosition;
    private Transform _selection;
    private bool _canSelect = true;
    private Vector2 DeltaVector;
    private Vector3 previousPosition;

    void Update () {
        if (Input.touchCount < 1) {
            return;
        }

        Touch touch = Input.GetTouch (0);
        Ray ray = Camera.main.ScreenPointToRay (touch.position);
        RaycastHit hit;
        Debug.Log ("touching");
        if (Physics.Raycast (ray, out hit)) {
            var selection = hit.transform;
            if (_canSelect == true) {
                SelectedBlock = selection.GetComponentInParent<Block> ();
            }
            if (SelectedBlock != null) {
                SelectedBlock.BeingTouched = true;
                if (touch.phase == TouchPhase.Began) {
                    initialTouchPosition = SelectedBlock.transform.position;
                    _canSelect = false;
                }
                DeltaVector += touch.deltaPosition * 0.02f;
                Vector3 MovementVector = initialTouchPosition;
                MovementVector.x += DeltaVector.x;
                MovementVector.y += DeltaVector.y;
                SelectedBlock.transform.position = MovementVector;
                // Debug.Log (DeltaVector);
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
                    if (SelectedBlock != null) {
                        SelectedBlock.GetComponent<Rigidbody2D> ().AddForce ((SelectedBlock.transform.position - previousPosition) * 100, ForceMode2D.Impulse);
                        SelectedBlock.BeingTouched = false;
                        SelectedBlock = null;
                    }
                    DeltaVector = Vector3.zero;
                    _canSelect = true;
                }
            } else {
                _canSelect = true;
                DeltaVector = Vector3.zero;

            }
        }
        if (SelectedBlock != null) {
            previousPosition = SelectedBlock.transform.position;
        }
    }
}