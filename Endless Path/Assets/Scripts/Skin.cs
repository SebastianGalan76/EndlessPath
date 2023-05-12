using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class Skin : MonoBehaviour {
    public Sprite playerSprite;
    public Sprite shopSprite;

    [HideInInspector] public int id;
    public SkinCategoryEnum category;

    public bool isLocked;
    public int price;

    private Text priceText;
    private Image skinImage;
    private GameObject priceObj;
    private GameObject selectedSkin;

    private void Awake() {
        gameObject.GetComponent<Button>().onClick.AddListener(() => OnClick());
    }

    public void Initialize() {
        priceObj = transform.Find("Price").gameObject;
        priceText = priceObj.transform.GetChild(0).GetComponent<Text>();
        skinImage = transform.Find("Skin").GetComponent<Image>();
        selectedSkin = transform.Find("SelectedSkin").gameObject;

        id = int.Parse(gameObject.name);

        isLocked = Convert.ToBoolean(PlayerPrefs.GetInt("skin:" + category.ToString() + "." + id, 1));

        RefreshSkinButton();
    }

    public void Buy() {
        CoinSystem.GetInstance().Take(price);

        UnlockSkin();
    }

    private void OnClick() {
        if(isLocked) {
            ShopSystem.GetInstance().OpenPurchasePanel(this);
        } else {
            SetCurrentSkin(true);
        }
    }

    public void SetCurrentSkin(bool value) {
        selectedSkin.SetActive(value);

        if(value) {
            SkinSystem.GetInstance().SetCurrentSkin(this);
        }
    }

    public void UnlockSkin() {
        isLocked = false;

        RefreshSkinButton();

        PlayerPrefs.SetInt("skin:" + category.ToString() + "." + id, 0);
        PlayerPrefs.Save();
    }

    private void RefreshSkinButton() {
        skinImage.sprite = shopSprite;

        if(isLocked) {
            priceText.text = price.ToString();
        } else {
            priceObj.SetActive(false);
        }

        skinImage.SetNativeSize();
    }
}
