using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftSystem : MonoBehaviour
{
    private static GiftSystem instance;

    [SerializeField] private UIGift ui;

    private int amount;

    private void Awake() {
        instance = this;
    }

    public void Initialize() {
        amount = PlayerPrefs.GetInt("Gifts", 0);

        ShowGiftButton();
    }

    public void PickUp() {
        Add(1);

        ui.PickUp();
    }

    public void Add(int value) {
        ChangeAmount(value);
    }

    public void Take(int value) {
        ChangeAmount(-value);
    }

    public void OpenGift() {
        Take(1);

        Skin skin = SkinSystem.GetInstance().GetRandomLockedSkin();
        skin.UnlockSkin();

        ui.OpenGift(skin);
    }

    public void ShowGiftButton() {
        ui.ChangeGiftBtnVisibility(amount);
    }

    public void HideGiftButton() {
        ui.ChangeGiftBtnVisibility(0);
    }

    private void ChangeAmount(int value) {
        amount += value;

        PlayerPrefs.SetInt("Gifts", amount);
        PlayerPrefs.Save();

        ShowGiftButton();
    }

    public static GiftSystem GetInstance() {
        return instance;
    }
}
