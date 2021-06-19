using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skin : MonoBehaviour
{
    public ShopSystem shop;
    bool isBought;
    int price;
    public int skinThemeID;
    int skinID;

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => onClick());
        int.TryParse(gameObject.name, out skinID);
        skinID--;

        loadSkinValue();
    }

    void onClick() {
        if (isBought)
        {
            selectSkin();
        }
        else {
            buySkin();
        }

        void buySkin() {
            if (shop.system.coins.checkCoins(price))
            {
                shop.system.ui.refreshPBuyingSkin(price, skinThemeID, skinID);
                shop.setActivePBuyingSkin(true);

                shop.skinToBuy.SkinObj = gameObject;
                shop.skinToBuy.Price = price;
                shop.skinToBuy.SkinThemeID = skinThemeID;
                shop.skinToBuy.SkinID = skinID;
            }
            else
            {
                shop.system.ui.setActivePNotEnoughCoins(true);
            }
        }
        void selectSkin() {
            shop.SelectedSkin.transform.Find("SelectedSkin").gameObject.SetActive(false);
            shop.SelectedSkin = gameObject;

            transform.Find("SelectedSkin").gameObject.SetActive(true);

            PlayerPrefs.SetInt("selectedSkinThemeID", skinThemeID);
            PlayerPrefs.SetInt("selectedSkinID", skinID);
            PlayerPrefs.Save();

            shop.system.player.setPlayerSkin();
        }
    } 

    public void loadSkinValue() {
        if (PlayerPrefs.GetInt("Skin" + skinThemeID + "." + skinID) == 1)
        {
            transform.Find("Price").gameObject.SetActive(false);
            isBought = true;
        }
        else {
            int.TryParse(transform.Find("Price").transform.GetChild(0).GetComponent<Text>().text, out price);
            isBought = false;
        }
    }
}
