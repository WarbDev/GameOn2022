using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using TMPro;

public class HealthBar : MonoBehaviour
{

    [SerializeField] Slider slider;
    [SerializeField] UIPlayerEvents events;
    [SerializeField] Image healthBarImage;
    [SerializeField] TextMeshProUGUI textMesh;

    private int durationOfChange = 1;

    private void Start()
    {
        slider.maxValue = events.MaxHealth;
        slider.value = events.MaxHealth;
        events.HealthChanged += SetHealth;
    }

    public void SetHealth(int heal)
    {

        DOTween.To(() => slider.value, x => slider.value = x, (float) heal, durationOfChange).SetEase(Ease.InSine);
        healthBarImage.DOColor(Color.Lerp(Color.red, Color.green, (float) heal / slider.maxValue), durationOfChange);
        
    }

    private void Update()
    {
        textMesh.text = "" + Mathf.Ceil(slider.value);
    }
}
