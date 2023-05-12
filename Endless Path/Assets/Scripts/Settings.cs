using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Settings : MonoBehaviour
{
    private static Settings instance;

    public float soundVolume;

    public bool vibration;
    public bool particles;
    public bool autoLoginToGP;

    [SerializeField] private GameSystem system = default;
    [SerializeField] private UISettings ui = default;

    private void Awake() {
        instance = this;
    }

    public void LoadSettings() {
        soundVolume = PlayerPrefs.GetFloat("Settings-Volume", 0.2f);

        vibration = Convert.ToBoolean(PlayerPrefs.GetInt("Settings-Vibration", 1));
        particles = Convert.ToBoolean(PlayerPrefs.GetInt("Settings-Particles", 1));
        autoLoginToGP = Convert.ToBoolean(PlayerPrefs.GetInt("Settings-AutoLoginToGP", 0));

        system.background.ChangeParticlesVisibility(particles);

        ui.ChangeParticlesStatus(particles);
        ui.ChangeVibrationStatus(vibration);
        ui.ChangeSoundSliderValue(soundVolume);
        ui.ChangeAutoLoginStatus(autoLoginToGP);
    }

    public void SetSoundsVolume(float vol) {
        PlayerPrefs.SetFloat("Settings-Volume", vol);
        PlayerPrefs.Save();

        SoundSystem.GetInstance().ChangeSoundVolume(vol);
    }

    public void SwitchVibration() {
        vibration = !vibration;

        PlayerPrefs.SetInt("Settings-Vibration", Convert.ToInt16(vibration));
        PlayerPrefs.Save();

        ui.ChangeVibrationStatus(vibration);
    }

    public void SwitchParticles() {
        particles = !particles;

        PlayerPrefs.SetInt("Settings-Particles", Convert.ToInt16(particles));
        PlayerPrefs.Save();

        ui.ChangeParticlesStatus(particles);
    }

    public void SwitchAutoLoginToGP() {
        autoLoginToGP = !autoLoginToGP;

        PlayerPrefs.SetInt("Settings-AutoLoginToGP", Convert.ToInt16(autoLoginToGP));
        PlayerPrefs.Save();

        ui.ChangeAutoLoginStatus(autoLoginToGP);
    }

    public static Settings GetInstance() {
        return instance;
    }
}
