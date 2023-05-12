using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : UIPanel, IPanel
{
    [SerializeField] private GameObject panel;

    [SerializeField] private Image vibrationStatus, particlesStatus;
    [SerializeField] private Sprite statusOn, statusOff;

    [SerializeField] private Image autoLoginStatus;
    [SerializeField] private Sprite autoLoginOn, autoLoginOff;

    [SerializeField] private Slider soundSlider;

    private Animator animator;

    private void Awake() {
        animator = panel.GetComponent<Animator>();
    }

    public void OpenPanel() {
        base.OpenPanel(panel, animator);
    }

    public void ClosePanel() {
        base.ClosePanel(panel, animator);
    }

    public void ChangeVibrationStatus(bool status) {
        vibrationStatus.sprite = GetStatus(status);
    }

    public void ChangeParticlesStatus(bool status) {
        particlesStatus.sprite = GetStatus(status);
    }

    public void ChangeSoundSliderValue(float value) {
        soundSlider.value = value;
    }

    public void ChangeAutoLoginStatus(bool status) {
        autoLoginStatus.sprite = status ? autoLoginOn : autoLoginOff;
    }

    private Sprite GetStatus(bool status) {
        return status ? statusOn : statusOff;
    }
}
