using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour {
    private TextMeshProUGUI _text;
    private float _time;

    void Start () {
        _text = GetComponent<TextMeshProUGUI> ();
        Restart ();
    }

    public void Restart () {
        _time = 120;
    }

    public void AddTime (float seconds) {
        _time += seconds;
    }

    void Update () {
        _time -= Time.deltaTime;
        _text.SetText (TimeSpan.FromSeconds (_time).ToString ("mm\\:ss\\.ff"));
    }
}