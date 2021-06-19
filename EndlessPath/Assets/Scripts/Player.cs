using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameSystem system;
    private SkinSystem skinSystem;
    public bool dead = true;

    private float playerSpeed = 2.25f;

    public Sprite DotSkin;
    public Color[] DotSkins;
    public Sprite[] BallSkins;
    public Sprite[] FoodSkins;
    public Sprite[] AnimalSkins;

    public GameObject pickUpCoinParticle, pickUpGiftParticle;
    public Slider slider;

    public AudioClip[] audioClip;

    private float deltaX;
    private Touch touch;
    private Vector2 touchPos;

    private CameraSystem camera;

    private void Start()
    {
        skinSystem = system.skinSystem;
        camera = Camera.main.GetComponent<CameraSystem>();
    }

    private void Update()
    {
        if (!system.getPause())
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(0, 1), playerSpeed * Time.deltaTime);
            if (Input.touchCount == 1) {
                touch = Input.GetTouch(0);

                touchPos = Camera.main.ScreenToWorldPoint(touch.position);

                switch (touch.phase){
                    case TouchPhase.Began:
                        deltaX = touchPos.x - transform.position.x;
                        break;
                    case TouchPhase.Moved:
                        if (touchPos.x - deltaX > -3.8f && touchPos.x - deltaX < 3.8f)
                        {
                            transform.position = new Vector2(touchPos.x - deltaX, transform.position.y);
                        }
                        else {
                            death();
                        }
                        break;
                    case TouchPhase.Ended:
                        deltaX = 0;
                        break;
                }
            }
            if (Application.platform != RuntimePlatform.Android)
            {
                transform.position = new Vector2(slider.value, transform.position.y);
            }
        }

        if (Input.GetKeyDown(KeyCode.K)) {
            death();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Coin" && collision.gameObject.name != "Gift" && !system.getDead())
        {
            if (collision.transform.parent.GetComponent<Animator>())
            {
                collision.transform.parent.GetComponent<Animator>().speed = 0;
            }

            death();
            return;
        }
        if (collision.gameObject.name == "Coin")
        {
            pickUpCoin(collision.gameObject);
        }
        else if (collision.gameObject.name == "Gift") {
            pickUpGift(collision.gameObject);
        }

        GooglePlayServices.postToAchievements();
    }

    //Executes after a collision with an edge of a path.
    public void death() {
        GetComponent<Animator>().Play("PlayerDeath");
        system.death();

        system.playAudio(1);

        StartCoroutine(wait());
        IEnumerator wait()
        {
            yield return new WaitForSeconds(0.1f);
            if (PlayerPrefs.GetInt("Settings-Vibes") == 1)
            {
                Vibration.Vibrate(400);
            }
        }
    }

    //Executes after a collision with a crystal. coin - collision object with name "Coin"
    private void pickUpCoin(GameObject coin)
    {
        system.coins.addCoins(1);

        PlayerPrefs.SetInt("AchievementsCoins", PlayerPrefs.GetInt("AchievementsCoins") + 1);
        PlayerPrefs.Save();

        system.playAudio(0);

        pickUpCoinParticle.transform.position = coin.transform.position;
        pickUpCoinParticle.GetComponent<ParticleSystem>().Play();

        Destroy(coin);
    }
    private void pickUpGift(GameObject gift) {
        PlayerPrefs.SetInt("Gifts", PlayerPrefs.GetInt("Gifts") + 1);
        PlayerPrefs.Save();

        PlayerPrefs.SetInt("AchievementsGifts", PlayerPrefs.GetInt("AchievementsGifts") + 1);
        PlayerPrefs.Save();

        system.playAudio(0);

        system.ui.pickUpGifts();

        pickUpGiftParticle.transform.position = gift.transform.position;
        pickUpGiftParticle.GetComponent<ParticleSystem>().Play();

        Destroy(gift);
    }

    //Changes the speed of the player
    public void setPlayerSpeed(bool increase, float value) {
        if (increase) { playerSpeed += value; }
        else
        {
            playerSpeed = value;
        }
        camera.playerSpeed = playerSpeed;
    }
    public float getPlayerSpeed() {
        return playerSpeed;
    }

    //Resurections after death (restart game)
    public void playerResurrection() {
        GetComponent<Animator>().Play("PlayerResurrection");

        slider.value = 0;
    }

    public void setPlayerSkin() {
        skinSystem.reloadSelectedSkinData();

        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = skinSystem.getSkinColor(skinSystem.SelectedSkinThemeID, skinSystem.SelectedSkinID);
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = skinSystem.getSkin(true,skinSystem.SelectedSkinThemeID, skinSystem.SelectedSkinID);
    }
}
