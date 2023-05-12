using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour {
    private static SoundSystem instance;

    [SerializeField] private AudioClip buttonClickSound = default;
    [SerializeField] private AudioClip pickUpSound = default;
    [SerializeField] private AudioClip failSound = default;

    private AudioSource audioSource;

    private void Awake() {
        instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string soundName = "buttonClick") {
        switch(soundName) {
            case "buttonClick":
                audioSource.PlayOneShot(buttonClickSound);
                break;
            case "pickUp":
                audioSource.PlayOneShot(pickUpSound);
                break;
            case "fail":
                audioSource.PlayOneShot(failSound);
                break;
        }
    }

    public void ChangeSoundVolume(float volume) {
        audioSource.volume = volume;
    }

    public static SoundSystem GetInstance() {
        return instance;
    }
}
