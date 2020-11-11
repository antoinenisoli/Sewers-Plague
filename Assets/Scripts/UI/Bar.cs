using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Bar : MonoBehaviour
{
    protected Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void Fill(int value, int maxValue)
    {
        slider.value = value;
        slider.maxValue = maxValue;
    }
}
