using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour {
    private TextMeshProUGUI _text;
    private float _time;
    private bool _shouldUpdate;
    [HideInInspector] public event Action OnTimerEnd;

    void Start () {
        _text = GetComponent<TextMeshProUGUI> ();
        OnTimerEnd += GameManager.Instance.EndGame;
        GameManager.Instance.OnGameOver += StopTimer;
        GameManager.Instance.OnGameBegin += Restart;
        Restart ();
    }

    public void Restart () {
        _time = 20;
        _shouldUpdate = true;
    }

    public void StopTimer () {
        _shouldUpdate = false;
    }

    public void AddTime (float seconds) {
        _time += seconds;
        _text.SetText (TimeSpan.FromSeconds (_time).ToString ("mm\\:ss\\.ff"));
    }

    void Update () {
        if (!_shouldUpdate) return;
        _time -= Time.deltaTime;
        if (_time * 100 < 0) {
            OnTimerEnd?.Invoke ();
            _shouldUpdate = false;
        }
        _text.SetText (TimeSpan.FromSeconds (_time).ToString ("mm\\:ss\\.ff"));
    }

}