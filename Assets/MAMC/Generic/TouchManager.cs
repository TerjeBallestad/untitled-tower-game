using UnityEngine;

public class TouchManager : MonoBehaviour {

    public Block SelectedBlock;
    public Vector3 initialTouchPosition;
    private Transform _selection;
    private bool _canSelect = true;
    private bool _touchDisabled;
    private Vector2 DeltaVector;
    private Vector3 previousPosition;

    private void Start () {
        GameManager.Instance.OnGameOver += DisableTouchInput;
        GameManager.Instance.OnGameBegin += EnableTouchInput;
    }

    private void DisableTouchInput () {
        _touchDisabled = true;
    }

    private void EnableTouchInput () {
        _touchDisabled = false;
    }

    void Update () {

        if (_touchDisabled || Input.touchCount < 1) { return; }

        Touch touch = Input.GetTouch (0);
        Ray ray = Camera.main.ScreenPointToRay (touch.position);
        RaycastHit hit;

        if (Physics.Raycast (ray, out hit)) {
            Transform selection = hit.transform;
            Debug.Log ("touching " + selection.gameObject.name);
            if (_canSelect == true) {
                SelectedBlock = selection.GetComponentInParent<Block> ();
            }
            if (SelectedBlock != null) {
                SelectedBlock.StartTouching ();
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

            } else {
                _canSelect = true;
                DeltaVector = Vector3.zero;

            }
        }
        if (SelectedBlock != null) {
            previousPosition = SelectedBlock.transform.position;
        }

        if (SelectedBlock != null) {
            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
                SelectedBlock.GetComponent<Rigidbody2D> ().AddForce ((SelectedBlock.transform.position - previousPosition) * 100, ForceMode2D.Impulse);
                StartCoroutine (SelectedBlock.StopTouching ());
                SelectedBlock = null;
                DeltaVector = Vector3.zero;
                _canSelect = true;
            }
        }
    }
}