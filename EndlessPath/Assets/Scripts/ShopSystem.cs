using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    public GameSystem system;
    private SkinSystem skinSystem;
    public skinToBuy skinToBuy;

    public GameObject SelectedSkin;

    public Image[] ShopThemeList;
    public Sprite SelectedTheme, UnselectedTheme;

    public GameObject[] DotSkins;
    public GameObject[] BallSkins;
    public GameObject[] FoodSkins;
    public GameObject[] AnimalSkins;
    public GameObject[] RocketSkins;

    public GameObject[] ShopTheme;

    public Text ShopThemeName;
    public string[] ShopThemeNames;

    private void Start()
    {
        skinSystem = system.skinSystem;
        setSelectedSkin();
    }

    public void setActivePBuyingSkin(bool value) {
        system.ui.setActivePBuyingSkin(value);
    }
    public void openShopThema(int themeID) {

        for (int i = 0; i < ShopTheme.Length; i++) {
            ShopTheme[i].SetActive(false);
            ShopThemeList[i].sprite = UnselectedTheme;
        }

        ShopThemeName.text = ShopThemeNames[themeID - 1];
        ShopThemeName.transform.GetChild(0).GetComponent<Text>().text = ShopThemeNames[themeID - 1];

        ShopTheme[themeID-1].SetActive(true);
        ShopThemeList[themeID-1].sprite = SelectedTheme;
    }
    public void buySkin() {
        system.coins.takeCoins(skinToBuy.Price);
        PlayerPrefs.SetInt("Skin" + skinToBuy.SkinThemeID + "." + skinToBuy.SkinID, 1);
        PlayerPrefs.Save();

        Debug.Log("buySkin");

        skinToBuy.SkinObj.GetComponent<Skin>().loadSkinValue();
        setActivePBuyingSkin(false);
    }

    public void unlockSkin(int skinThemeID, int skinID)
    {
        PlayerPrefs.SetInt("Skin" + skinThemeID + "." + skinID, 1);
        PlayerPrefs.Save();

        Debug.Log("unlockSkin");
        if (skinThemeID == 0)
        {
            DotSkins[skinID].GetComponent<Skin>().loadSkinValue();
        }
        else if (skinThemeID == 1) {
            BallSkins[skinID].GetComponent<Skin>().loadSkinValue();
        }
        else if (skinThemeID == 2)
        {
            FoodSkins[skinID].GetComponent<Skin>().loadSkinValue();
        }
        else if (skinThemeID == 3)
        {
            AnimalSkins[skinID].GetComponent<Skin>().loadSkinValue();
        }
        else if (skinThemeID == 4)
        {
            RocketSkins[skinID].GetComponent<Skin>().loadSkinValue();
        }
    }

    void setSelectedSkin() {
        skinSystem.reloadSelectedSkinData();
        if (skinSystem.SelectedSkinThemeID == 0)
        {
            DotSkins[skinSystem.SelectedSkinID].transform.Find("SelectedSkin").gameObject.SetActive(true);
            SelectedSkin = DotSkins[skinSystem.SelectedSkinID];
        }
        else if (skinSystem.SelectedSkinThemeID == 1)
        {
            BallSkins[skinSystem.SelectedSkinID].transform.Find("SelectedSkin").gameObject.SetActive(true);
            SelectedSkin = BallSkins[skinSystem.SelectedSkinID];
        }
        else if (skinSystem.SelectedSkinThemeID == 2)
        {
            FoodSkins[skinSystem.SelectedSkinID].transform.Find("SelectedSkin").gameObject.SetActive(true);
            SelectedSkin = FoodSkins[skinSystem.SelectedSkinID];
        }
        else if (skinSystem.SelectedSkinThemeID == 3) {
            AnimalSkins[skinSystem.SelectedSkinID].transform.Find("SelectedSkin").gameObject.SetActive(true);
            SelectedSkin = AnimalSkins[skinSystem.SelectedSkinID];
        }
        else if (skinSystem.SelectedSkinThemeID == 4)
        {
            RocketSkins[skinSystem.SelectedSkinID].transform.Find("SelectedSkin").gameObject.SetActive(true);
            SelectedSkin = RocketSkins[skinSystem.SelectedSkinID];
        }
    }
}

[System.Serializable]
public class skinToBuy{
    GameObject skinObj;
    int price, skinThemeID, skinID;

    public GameObject SkinObj {
        get { return skinObj; }
        set { skinObj = value; }
    }
    public int Price {
        get { return price; }
        set { price = value; }
    }
    public int SkinThemeID {
        get { return skinThemeID; }
        set { skinThemeID = value; }
    }
    public int SkinID {
        get { return skinID; }
        set { skinID = value; }
    }
}
