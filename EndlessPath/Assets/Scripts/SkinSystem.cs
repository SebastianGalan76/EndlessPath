using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SkinSystem : MonoBehaviour
{
    public PlayerSkin playerSkin;
    public ShopSkin shopSkin;

    public GameObject POpenGift;
    private GameObject GiftObj;
    private Image SkinImage;
    public ShopSystem shopSystem;

    private int selectedSkinThemeID, selectedSkinID;

    private void Start()
    {
        reloadSelectedSkinData();
    }

    public void reloadSelectedSkinData() {
        selectedSkinThemeID = PlayerPrefs.GetInt("selectedSkinThemeID");
        selectedSkinID = PlayerPrefs.GetInt("selectedSkinID");
    }

    public Color32 getSkinColor(int skinThemeID, int skinID) {
        if (skinThemeID != 0) {
            return Color.white;
        }
        return playerSkin.DotSkins[skinID];
    }
    public Sprite getSkin(bool forPlayer, int skinThemeID, int skinID) {
        if (skinThemeID == 0)
        {
            if (forPlayer)
            {
                return playerSkin.DotSkin;
            }
            else
            {
                return shopSkin.DotSkin;
            }
        }
        else if (skinThemeID == 1) //Ball
        {
            if (forPlayer)
            {
                return playerSkin.BallSkins[skinID];
            }
            else
            {
                return shopSkin.BallSkins[skinID];
            }
        }
        else if (skinThemeID == 2) //Food
        {
            if (forPlayer)
            {
                return playerSkin.FoodSkins[skinID];
            }
            else
            {
                return shopSkin.FoodSkins[skinID];
            }
        }
        else if (skinThemeID == 3) //Animal
        {
            if (forPlayer)
            {
                return playerSkin.AnimalSkins[skinID];
            }
            else
            {
                return shopSkin.AnimalSkins[skinID];
            }
        }
        else if (skinThemeID == 4) //Rocket
        {
            if (forPlayer)
            {
                return playerSkin.RocketSkins[skinID];
            }
            else
            {
                return shopSkin.RocketSkins[skinID];
            }
        }
        else { return null; }
    }

    public void UnlockRandomSkin() {
        GiftObj = POpenGift.transform.Find("Panel").Find("Gift").gameObject;
        SkinImage = GiftObj.transform.Find("Skin").Find("Skin").GetComponent<Image>();
    RandomAgain:

        if (!allowToUnlockRandomSkin()) {
            Debug.Log("Wszystkie skiny sa odblokowane!");
            return;
        }

        int randomTheme = Random.Range(0, 5);
        if (randomTheme == 4) {
            int random = Random.Range(0, 100);
            if (random < 90) {
                goto RandomAgain;
            }
        }

        int randomSkin = Random.Range(0, playerSkin.getSkinAmount(randomTheme));
        int randomSkinExtra = randomSkin;
        for (int i = 0; i < playerSkin.getSkinAmount(randomTheme); i++) {
            if (!skinIsBought(randomTheme, randomSkin))
            {
                //UnlockSkin
                unlockSkin();
                break;
            }
            else {
                randomSkin++;
                if (randomSkin >= playerSkin.getSkinAmount(randomTheme)) {
                    randomSkin = 0;
                }
                if (!skinIsBought(randomTheme, randomSkin))
                {
                    //UnlockSkin
                    unlockSkin();
                    break;
                }
            }

            if (randomSkinExtra == randomSkin) {
                goto RandomAgain;
            }
        }

        void unlockSkin() {
            SkinImage.color = getSkinColor(randomTheme, randomSkin);
            SkinImage.sprite = getSkin(false, randomTheme, randomSkin);
            SkinImage.SetNativeSize();

            GiftObj.GetComponent<Animator>().Play("OpenGift");

            shopSystem.unlockSkin(randomTheme, randomSkin);
        }
    }

    public bool allowToUnlockRandomSkin() {
        for (int i = 0; i < 5; i++) {
            for (int y = 0; y < playerSkin.getSkinAmount(i); y++)
            {
                if (!skinIsBought(i,y)) {
                    return true;
                }
            }
        }
        return false;
    }

    public bool skinIsBought(int skinThemeID, int skinID) {
        if (PlayerPrefs.GetInt("Skin" + skinThemeID + "." + skinID) == 0) {
            return false;
        }
        return true;
    }
    public int SelectedSkinThemeID {
        get { return selectedSkinThemeID; }
        set { selectedSkinThemeID = value; }
    }
    public int SelectedSkinID { 
        get { return selectedSkinID; }
        set { selectedSkinID = value; }
    }
}

[System.Serializable]
public class PlayerSkin {
    public Sprite DotSkin;
    public Color32[] DotSkins;
    public Sprite[] BallSkins;
    public Sprite[] FoodSkins;
    public Sprite[] AnimalSkins;
    public Sprite[] RocketSkins;

    public int getSkinAmount(int skinThemeID) {
        if (skinThemeID == 0) {
            return DotSkins.Length;
        }
        else if (skinThemeID == 1)
        {
            return BallSkins.Length;
        }
        else if (skinThemeID == 2)
        {
            return FoodSkins.Length;
        }
        else if (skinThemeID == 3)
        {
            return AnimalSkins.Length;
        }
        else if (skinThemeID == 4)
        {
            return RocketSkins.Length;
        }
        return 0;
    }
}

[System.Serializable]
public class ShopSkin {
    public Sprite DotSkin;
    public Sprite[] BallSkins;
    public Sprite[] FoodSkins;
    public Sprite[] AnimalSkins;
    public Sprite[] RocketSkins;
}
