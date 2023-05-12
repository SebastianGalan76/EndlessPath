using GooglePlayGames.BasicApi;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameSystem system;

    [SerializeField] private CameraSystem cameraSystem;
    [SerializeField] private SpriteRenderer skin;

    [SerializeField] private GameObject pickUpCoinParticle;
    [SerializeField] private GameObject pickUpGiftParticle;

    private PlayerMovement movement;

    private Animator animator;
    private bool dead;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(IsDead()) {
            return;
        }

        switch(collision.gameObject.name) {
            case "Coin":
                PickUpCoin(collision.gameObject);
                break;
            case "Gift":
                PickUpGift(collision.gameObject);
                break;
            
            default:
                if(collision.transform.parent.GetComponent<Animator>()) {
                    collision.transform.parent.GetComponent<Animator>().speed = 0;
                }

                Die();
                break;
        }

        GooglePlayServices.PostToAchievements();

        void PickUpCoin(GameObject coin) {
            IncreaseStat("AchievementsCoins");
            PlaySound("pickUp");

            CoinSystem.GetInstance().PickUp();

            ShowParticle(pickUpCoinParticle, coin.transform.position);
            Destroy(coin);
        }
        void PickUpGift(GameObject gift) {
            IncreaseStat("AchievementsGifts");
            IncreaseStat("Gifts");
            PlaySound("pickUp");

            GiftSystem.GetInstance().PickUp();

            ShowParticle(pickUpGiftParticle, gift.transform.position);
            Destroy(gift);
        }

        void IncreaseStat(string achievementName) {
            PlayerPrefs.SetInt(achievementName, PlayerPrefs.GetInt(achievementName, 0) + 1);
            PlayerPrefs.Save();
        }
    }

    public void PlayGame() {
        //Sets the default speed and position of the player.
        ChangePlayerSpeed(false, 2.25f);
        transform.position = new Vector3(0, 1);

        movement.ResetSlider();

        dead = false;
    }

    //Executes after a collision with an edge of a path.
    public void Die() {
        dead = true;

        PlayAnimation("PlayerDeath");
        PlaySound("fail");

        system.Die();

        StartCoroutine(Wait());
        IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.1f);
            if (Settings.GetInstance().vibration)
            {
                Vibration.Vibrate(400);
            }
        }
    }

    //Resurections after death (restart game)
    public void Resurrection() {
        PlayAnimation("PlayerResurrection");

        PlayGame();
    }

    public void SetSkin(Sprite skin) {
        this.skin.sprite = skin;
    }

    public void ChangePlayerSpeed(bool increase, float value) {
        movement.ChangePlayerSpeed(increase, value);

        cameraSystem.ChangeSpeed(movement.GetPlayerSpeed());
    }

    private void PlayAnimation(string animationName) {
        animator.Play(animationName);
    }
    private void PlaySound(string soundName) {
        SoundSystem.GetInstance().PlaySound(soundName);
    }
    private void ShowParticle(GameObject particle, Vector3 position) {
        particle.transform.position = position;
        particle.GetComponent<ParticleSystem>().Play();
    }

    public Vector3 GetPlayerPostion() {
        return transform.position;
    }
    public bool IsDead() {
        return dead;
    }
}
