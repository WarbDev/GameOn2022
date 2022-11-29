using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{

    [SerializeField] Slider slider;
    [SerializeField] UIPlayerEvents events;

    private void Start()
    {
        slider.maxValue = events.MaxHealth;
        slider.value = events.MaxHealth;
        events.HealthChanged += SetHealth;
    }

    public void SetHealth(int heal)
    {

        DOTween.To(() => slider.value, x => slider.value = x, (float) heal, 1).SetEase(Ease.InSine);
        //slider.value = 0;
        Debug.Log("I AM HURT");
    }
}
