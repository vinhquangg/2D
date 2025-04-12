using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSideHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateHealBar(float currentValue, float maxValue)
    {
        slider.value = currentValue/maxValue;
    }
}
