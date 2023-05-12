using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Runtime.CompilerServices;

public class SkinSystem : MonoBehaviour
{
    private static SkinSystem instance;

    [SerializeField] private List<Skin> skins = new List<Skin>();
    [SerializeField] private Player player;

    private Skin currentSkin;

    private void Awake() {
        instance = this;
    }

    public void Initialize() {
        foreach(Skin skin in skins) {
            skin.Initialize();
        }

        LoadCurrentSkin();
    }

    public void SetCurrentSkin(Skin skin) {
        if(currentSkin != null) {
            currentSkin.SetCurrentSkin(false);
        }

        currentSkin = skin;

        player.SetSkin(currentSkin.playerSprite);

        PlayerPrefs.SetInt("currentSkinCategoryID", (int)skin.category);
        PlayerPrefs.SetInt("currentSkinID", skin.id);

        PlayerPrefs.Save();
    }

    private void LoadCurrentSkin() {
        SkinCategoryEnum category = GetCategoryEnumByID();
        int skinID = PlayerPrefs.GetInt("currentSkinID");

        FindCurrentSkin();

        void FindCurrentSkin() {
            foreach(Skin skin in skins) {
                if(skin.category == category && skin.id == skinID) {
                    skin.SetCurrentSkin(true);
                    break;
                }
            }
        }
        SkinCategoryEnum GetCategoryEnumByID() {
            return (SkinCategoryEnum)PlayerPrefs.GetInt("currentSkinCategoryID");
        }
    }

    public Skin GetRandomLockedSkin() {
        bool foundLockedSkin = false;
        int randomIndex;
        int a = 50;
        do {
            randomIndex = Random.Range(0, skins.Count-1);
            if(skins[randomIndex].isLocked) {
                foundLockedSkin = true;
            }
            a--;
        }while(!foundLockedSkin && a >=0);

        if(!foundLockedSkin) {
            randomIndex = Random.Range(0, skins.Count - 1);

            if(!skins[randomIndex].isLocked) {
                FoundNextLockedSkin();
            }
        }

        return skins[randomIndex];

        void FoundNextLockedSkin() {
            int index;
            for(int i = 0;i < skins.Count;i++) {
                index = (randomIndex+i)%(skins.Count-1);

                if(skins[index].isLocked) {
                    randomIndex = index;

                    break;
                }
            }

        }
    }
    public bool AllSkinsAreUnlocked() {
        foreach(Skin skin in skins) {
            if(skin.isLocked) {
                return false;
            }
        }

        return true;
    }

    public static SkinSystem GetInstance() {
        return instance;
    }
}