using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _textLabel;
    private float _time;
    private float _startTime;
    private bool _shouldUpdate;
    [HideInInspector] public event Action OnTimerEnd;

    void Start () {
        OnTimerEnd += GameManager.Instance.EndGame;
        GameManager.Instance.OnGameOver += StopTimer;
        GameManager.Instance.OnGameBegin += Restart;
        Restart ();
    }

    public void SetGameDuration (float seconds) {
        _startTime = seconds;
    }
    public void Restart () {
        _time = _startTime;
        _shouldUpdate = true;
    }

    public void StopTimer () {
        _shouldUpdate = false;
    }

    public void AddTime (float seconds) {
        _time += seconds;
        _textLabel.SetText (TimeSpan.FromSeconds (_time).ToString ("mm\\:ss\\.ff"));
    }

    void Update () {
        if (!_shouldUpdate) return;
        _time -= Time.deltaTime;
        if (_time * 100 < 0) {
            OnTimerEnd?.Invoke ();
            _shouldUpdate = false;
        }
        _textLabel.SetText (TimeSpan.FromSeconds (_time).ToString ("mm\\:ss\\.ff"));
    }

}