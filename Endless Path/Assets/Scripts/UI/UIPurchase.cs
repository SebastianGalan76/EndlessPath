using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIPurchase : UIPanel
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Animator animator;

    [SerializeField] private Image skinImage;
    [SerializeField] private Text skinPriceText;

    [SerializeField] private UINotEnoughCoins notEnoughCoins;

    private Skin skin;

    public void OpenPanel(Skin skin) {
        this.skin = skin;

        base.OpenPanel(panel, animator);

        RefreshPanel();
    }

    public void Confirm() {
        if(CoinSystem.GetInstance().CheckAmount(skin.price)) {
            skin.Buy();

            ClosePanel();
        } else {
            notEnoughCoins.OpenPanel();
        }
    }

    public void Refuse() {
        ClosePanel();
    }

    private void ClosePanel() {
        base.ClosePanel(panel, animator);
    }

    private void RefreshPanel() {
        skinImage.sprite = skin.shopSprite;
        skinPriceText.text = skin.price.ToString();

        skinImage.SetNativeSize();
    }
}
