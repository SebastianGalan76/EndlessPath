using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    public GameSystem system;

    public Text[] TScore;
    public Text[] TRecord;

    public GameObject PMenu, PShop, PDead;

    public GameObject[] BPlay_Background;

    public GameObject PGooglePlay;
    public GameObject PBuyingSkin;
    public GameObject PNotEnoughCoins;
    public GameObject POpenGift;
    public GameObject PSettings;
    public GameObject PAdsAreNotLoaded;

    public GameObject UIGiftPrefab;
    public GameObject BGift;

    public GameObject RestartCircle;
    public GameObject CoinPickUpAnimationPrefab, GiftPickUpAnimationPrefab;
    
    public GameObject TTip;

    public GameObject[] GooglePlayAutomaticLoginSwitch;

    private void Start()
    {
        reloadGiftButton();
        playAnimation(TTip, "TTipFall");
        setRecordNumber(PlayerPrefs.GetInt("Record"));
        loadSettings();
    }

    public void death() {
        changeTip(1);

        reloadGiftButton();
        playAnimation(PMenu, "PMenuShow");
        changeBPlayStatus(true, 0.4f);

        PDead.SetActive(true);
        playAnimation(TTip, "TTipFall");
        scoreAnimation("HideScore");
    }
    public void restart() {
        playAnimation(PMenu, "PMenuHide");
        playAnimation(RestartCircle, "RestartShow");

        StartCoroutine(wait());
        IEnumerator wait() {
            yield return new WaitForSeconds(0.5f);
            PDead.SetActive(false);
            system.restart();
        }
    }
    public void playGame()
    {
        if (!system.getDead())
        {
            playAnimation(PMenu, "PMenuHide");
            playAnimation(TTip, "TTipHide");
            system.playGame();
        }
        else {
            changeBPlayStatus(false, 0);
            playAnimation(TTip, "TTipHide");
            restart();
        }
    }

    public void setScoreNumber(int score)
    {
        for (int i = 0; i < TScore.Length; i++) {
            TScore[i].text = score.ToString();
            TScore[i].transform.GetChild(0).GetComponent<Text>().text = score.ToString(); 
        }
    }
    public void setRecordNumber(int record) {
        for (int i = 0; i < TRecord.Length; i++)
        {
            TRecord[i].text = record.ToString();
            TRecord[i].transform.GetChild(0).GetComponent<Text>().text = record.ToString();
        }
    }
    public void changeTip(int tipID) {
        Text[] tip = new Text[2];
        tip[0] = TTip.GetComponent<Text>();
        tip[1] = TTip.transform.GetChild(0).GetComponent<Text>();

        string tipText;
        if (tipID == 0)
        {
            tipText = "HOLD & DRAG TO PLAY";
        }
        else if (tipID == 1)
        {
            tipText = "TAP TO RESTART";
        }
        else {
            tipText = "";
        }

        tip[0].text = tipText;
        tip[1].text = tipText;
    }

    public void changeBPlayStatus(bool value, float delay) {
        if (delay > 0)
        {
            StartCoroutine(wait());

            IEnumerator wait()
            {
                yield return new WaitForSeconds(delay);
                changeStatus();
            }
        }
        else {
            changeStatus();
        }

        void changeStatus() {
            for (int i = 0; i < BPlay_Background.Length; i++)
            {
                BPlay_Background[i].SetActive(value);
            }
        }
    }
    public void newRecord(bool value) {
        //If value equals true, it shows panel with "new record" comunicat.
        PDead.transform.Find("PNewRecord").gameObject.SetActive(value);
        PDead.transform.Find("PNormal").gameObject.SetActive(!value);
    }

    public void setActivePShop(bool value)
    {
        if (value)
        {
            playAnimation(PMenu, "OpenShop-PMenu");
            playAnimation(PShop, "OpenShop-PShop");
            
            //TestMod
            //PlayerPrefs.SetInt("Coins", 1000);
            //PlayerPrefs.Save();
            //^^^
        }
        else
        {
            playAnimation(PMenu, "CloseShop-PMenu");
            playAnimation(PShop, "CloseShop-PShop");
        }
    }
    public void setActivePGooglePlay(bool value) {
        if (value)
        {
            PGooglePlay.SetActive(true);
            loadGooglePlayAutomaticLogin();
            playAnimation(PGooglePlay, "ShowPGooglePlay");
        }
        else {
            playAnimation(PGooglePlay, "HidePGooglePlay");

            StartCoroutine(wait());
            IEnumerator wait() {
                yield return new WaitForSeconds(0.3f);
                PGooglePlay.SetActive(value);
            }
        }
    }
    public void setActivePBuyingSkin(bool value) {
        if (value)
        {
            PBuyingSkin.SetActive(true);
            playAnimation(PBuyingSkin, "ShowPBuyingSkin");
        }
        else
        {
            playAnimation(PBuyingSkin, "HidePBuyingSkin");

            StartCoroutine(wait());
            IEnumerator wait()
            {
                yield return new WaitForSeconds(0.3f);
                PBuyingSkin.SetActive(value);
            }
        }
    }
    public void setActivePOpenGift(bool value) { 
        if (value)
        {
            GameObject gift = Instantiate(UIGiftPrefab);
            gift.transform.SetParent(POpenGift.transform.Find("Panel"), false);
            gift.name = "Gift";

            POpenGift.transform.Find("Panel").Find("CloseShadow").gameObject.SetActive(false);
            POpenGift.transform.Find("Panel").Find("OpenShadow").gameObject.SetActive(true);

            POpenGift.SetActive(true);
            playAnimation(POpenGift, "ShowPOpenGift");
        }
        else
        {
            playAnimation(POpenGift, "HidePOpenGift");

            StartCoroutine(wait());
            IEnumerator wait()
            {
                yield return new WaitForSeconds(0.3f);
                POpenGift.SetActive(false);

                Destroy(POpenGift.transform.Find("Panel").Find("Gift").gameObject);
            }
        }
    }
    public void setActivePSettings(bool value)
    {
        if (value)
        {
            loadSettings();
            PSettings.SetActive(true);
            loadGooglePlayAutomaticLogin();
            playAnimation(PSettings, "ShowPSettings");
        }
        else
        {
            playAnimation(PSettings, "HidePSettings");

            StartCoroutine(wait());
            IEnumerator wait()
            {
                yield return new WaitForSeconds(0.3f);
                PSettings.SetActive(value);
            }
        }
    }
    public void setActivePAdsAreNotLoaded(bool value)
    {
        if (value)
        {
            PAdsAreNotLoaded.SetActive(true);
            playAnimation(PAdsAreNotLoaded, "ShowPAdsAreNotLoaded");
        }
        else
        {
            playAnimation(PAdsAreNotLoaded, "HidePAdsAreNotLoaded");

            StartCoroutine(wait());
            IEnumerator wait()
            {
                yield return new WaitForSeconds(0.3f);
                PAdsAreNotLoaded.SetActive(value);
            }
        }
    }

    void loadSettings()
    {
        PSettings.transform.Find("Panel").Find("SVolume").GetComponent<Slider>().value = PlayerPrefs.GetFloat("Settings-Volume");
        //system.AudioSource[0].volume = PlayerPrefs.GetFloat("Settings-Volume");

        if (PlayerPrefs.GetInt("Settings-Vibes") == 1)
        {
            PSettings.transform.Find("Panel").Find("BVibes").Find("On").gameObject.SetActive(true);
            PSettings.transform.Find("Panel").Find("BVibes").Find("Off").gameObject.SetActive(false);
        }
        else
        {
            PSettings.transform.Find("Panel").Find("BVibes").Find("On").gameObject.SetActive(false);
            PSettings.transform.Find("Panel").Find("BVibes").Find("Off").gameObject.SetActive(true);
        }

        if (PlayerPrefs.GetInt("Settings-Particles") == 1)
        {
            PSettings.transform.Find("Panel").Find("BParticles").Find("On").gameObject.SetActive(true);
            PSettings.transform.Find("Panel").Find("BParticles").Find("Off").gameObject.SetActive(false);
            system.bgSystem.setActiveParticles(true);
        }
        else
        {
            PSettings.transform.Find("Panel").Find("BParticles").Find("On").gameObject.SetActive(false);
            PSettings.transform.Find("Panel").Find("BParticles").Find("Off").gameObject.SetActive(true);
            system.bgSystem.setActiveParticles(false);
        }
    }
    public void changeSettings(string value)
    {
        if(PlayerPrefs.GetInt(value) == 1)
        {
            PlayerPrefs.SetInt(value, 0);
            system.bgSystem.setActiveParticles(false);
        }
        else
        {
            PlayerPrefs.SetInt(value, 1);
            system.bgSystem.setActiveParticles(true);
        }
        loadSettings();
    }
    public void SetSoundsVolume(float vol)
    {
        PlayerPrefs.SetFloat("Settings-Volume", vol);
        PlayerPrefs.Save();

        //system.AudioSource[0].volume = vol;
    }

    public void openGift() {
        system.skinSystem.UnlockRandomSkin();

        PlayerPrefs.SetInt("Gifts", PlayerPrefs.GetInt("Gifts") - 1);
        PlayerPrefs.Save();

        POpenGift.transform.Find("Panel").Find("OpenShadow").gameObject.SetActive(false);

        reloadGiftButton();

        StartCoroutine(wait());
        IEnumerator wait() {
            yield return new WaitForSeconds(2f);
            POpenGift.transform.Find("Panel").Find("CloseShadow").gameObject.SetActive(true);
        }   
    }
    public void reloadGiftButton() {
        if (PlayerPrefs.GetInt("Gifts") >= 1 && system.skinSystem.allowToUnlockRandomSkin())
        {
            BGift.transform.Find("GiftNumber").Find("Text").GetComponent<Text>().text = PlayerPrefs.GetInt("Gifts").ToString();
            BGift.SetActive(true);
        }
        else {
            BGift.SetActive(false);
        }
    }
    public void setActivePNotEnoughCoins(bool value)
    {
        if (value)
        {
            PNotEnoughCoins.SetActive(true);
            playAnimation(PNotEnoughCoins, "ShowPNotEnoughCoins");
        }
        else
        {
            playAnimation(PNotEnoughCoins, "HidePNotEnoughCoins");

            StartCoroutine(wait());
            IEnumerator wait()
            {
                yield return new WaitForSeconds(0.3f);
                PNotEnoughCoins.SetActive(value);
            }
        }
    }
    public void refreshPBuyingSkin(int price, int skinThemeID, int skinID)
    {
        Transform obj = PBuyingSkin.transform.Find("Panel").Find("Skin");
        obj.Find("Price").Find("Text").GetComponent<Text>().text = price.ToString();

        Image Skin = obj.Find("Skin").GetComponent<Image>();

        Skin.color = system.skinSystem.getSkinColor(skinThemeID, skinID);
        Skin.sprite = system.skinSystem.getSkin(false, skinThemeID, skinID);
        Skin.SetNativeSize();

    }
    public void rateGame() {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.Coresaken.EndlessPath");
    }

    public void changeGooglePlayAutomaticLogin() {
        if (PlayerPrefs.GetInt("GooglePlayAutomaticLogin") == 0)
        {
            PlayerPrefs.SetInt("GooglePlayAutomaticLogin", 1);
            PlayerPrefs.Save();
            GooglePlayServices.authenticateUser();

            loadGooglePlayAutomaticLogin();
        }
        else {
            PlayerPrefs.SetInt("GooglePlayAutomaticLogin", 0);
            PlayerPrefs.Save();

            loadGooglePlayAutomaticLogin();
        }
    }
    private void loadGooglePlayAutomaticLogin()
    {
        GooglePlayAutomaticLoginSwitch[0].SetActive(false);
        GooglePlayAutomaticLoginSwitch[1].SetActive(false);

        GooglePlayAutomaticLoginSwitch[PlayerPrefs.GetInt("GooglePlayAutomaticLogin")].SetActive(true);
    }
    public void pickUpCoins()
    {
        RectTransform canvasRect = gameObject.GetComponent<RectTransform>();
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(system.playerObj.transform.position);
        Vector2 Player_ScreenPosition = new Vector2(
        ((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

        GameObject coinPickUp = Instantiate(CoinPickUpAnimationPrefab);
        coinPickUp.transform.SetParent(gameObject.transform, false);
        coinPickUp.GetComponent<RectTransform>().anchoredPosition = Player_ScreenPosition;
    }
    public void pickUpGifts() {
        RectTransform canvasRect = gameObject.GetComponent<RectTransform>();
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(system.playerObj.transform.position);
        Vector2 Player_ScreenPosition = new Vector2(
        ((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

        GameObject giftPickUp = Instantiate(GiftPickUpAnimationPrefab);
        giftPickUp.transform.SetParent(gameObject.transform, false);
        giftPickUp.GetComponent<RectTransform>().anchoredPosition = Player_ScreenPosition;
    }
    public void playAnimation(GameObject obj, string animation)
    {
        obj.GetComponent<Animator>().Play(animation);
    }
    public void clickButton() {
        system.playAudio(2);
    }
    public void scoreAnimation(string animation)
    {
        TScore[0].gameObject.GetComponent<Animator>().Play(animation);
    }
    public void setTimeScale(float value)
    {
        Time.timeScale = value;
    }
}
