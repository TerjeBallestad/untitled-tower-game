using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {
    private Image _mask, _fill;

    private void Start () {
        _mask = transform.GetChild (0).GetChild (0).GetComponent<Image> ();
        _fill = transform.GetChild (0).GetChild (0).GetChild (0).GetComponent<Image> ();
        gameObject.SetActive (false);
    }

    public void UpdateCurrentFill (float maximum, float current, Color color) {
        float fillAmount = current / maximum;
        _mask.fillAmount = fillAmount;
        _fill.color = color;
    }
}