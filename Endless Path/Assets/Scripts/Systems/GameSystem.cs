using System.Collections;
using UnityEngine;

public class GameSystem : MonoBehaviour {
    public BackgroundSystem background;
    public Player player;
    public AdSystem ad;

    [SerializeField] private UISystem ui;

    private PathSystem pathSystem;

    private bool pause;
    private int score;
    private float time;

    private void Awake() {
        pathSystem = GetComponent<PathSystem>();

        if(PlayerPrefs.GetInt("FirstLaunch") == 0) {
            FirstGameLaunch();
        }

        pause = true;
    }

    private void Start() {
        SkinSystem.GetInstance().Initialize();
        GiftSystem.GetInstance().Initialize();

        Settings.GetInstance().LoadSettings();

        pathSystem.StartGame();

        //Sets the default position of the camera
        Camera.main.transform.position = new Vector3(0, 0f, -10f);
    }

    private void Update() {
        //Changes the player's speed, number of score, background color according to the number of score.
        GameProgress();

        void GameProgress() {
            if(!pause && !player.IsDead()) {
                time += Time.deltaTime;
                if(time >= 1) {
                    score++;
                    time = 0;

                    if(score % 10 == 0) {
                        ChangeBackgroundColor();

                        //Sets the amount of score during the background color change animation
                        StartCoroutine(wait());
                        IEnumerator wait() {
                            yield return new WaitForSeconds(0.15f);
                            ui.SetScoreNumber(score);
                        }
                    } else {
                        ui.SetScoreNumber(score);
                    }

                    //Increases the player's speed (Min: 3f, Max: 4.05f)
                    if(score % 10 == 0 && score < 100) {
                        player.ChangePlayerSpeed(true, 0.15f);
                    }
                    if(score % 100 == 0) {
                        player.ChangePlayerSpeed(true, 0.2f);
                    }
                }
            }
        }
    }

    public void RestartGame() {
        ad.CheckInterstitialAdValue();

        //Generate new path
        pathSystem.StartGame();

        //Resurrections after death
        player.Resurrection();

        //Sets the default position of the camera
        Camera.main.transform.position = new Vector3(0, 0f, -10f);

        background.ChangeParticles();

        StartCoroutine(Wait());
        IEnumerator Wait() {
            yield return new WaitForSeconds(0.3f);

            PlayGame();
        }

    }
    public void PlayGame() {
        player.PlayGame();

        //Waits for end of the animation
        StartCoroutine(wait());

        //Shows the amount of score two seconds after start the game
        StartCoroutine(showScore());

        ui.ChangeBackgroundButtonVisibility(false, 0);

        IEnumerator wait() {
            yield return new WaitForSeconds(0.3f);

            time = 0;
            score = 0;
            pause = false;
        }
        IEnumerator showScore() {
            yield return new WaitForSeconds(2f);
            if(!player.IsDead()) {
                ui.ScoreAnimation("ShowScore");
            }
        }
    }
    public void Die() {
        pause = true;
        ui.SetScoreNumber(score);

        if(CheckNewRecord()) {
            PlayerPrefs.SetInt("Record", score);
            PlayerPrefs.Save();

            GooglePlayServices.PostToLeaderboard();
            GooglePlayServices.PostToAchievements();

            ui.NewRecord(true);
            ui.SetRecordNumber(score);
        } else {
            ui.NewRecord(false);
        }

        StartCoroutine(Wait());
        IEnumerator Wait() {
            yield return new WaitForSeconds(0.2f);

            ui.Die();
        }

        bool CheckNewRecord() {
            return PlayerPrefs.GetInt("Record") < score;
        }
    }


    private void ChangeBackgroundColor() {
        background.ChangeBackgroundColor();
        ui.ScoreAnimation("ChangeColorScore");
    }

    private void FirstGameLaunch() {
        PlayerPrefs.SetInt("currentSkinCategoryID", 0);
        PlayerPrefs.SetInt("currentSkinID", 0);
        PlayerPrefs.SetInt("skin:DOT.0", 0);

        PlayerPrefs.SetInt("Settings-Vibes", 1);
        PlayerPrefs.SetInt("Settings-Particles", 1);
        PlayerPrefs.SetFloat("Settings-Volume", 0.2f);        

        PlayerPrefs.SetInt("FirstLaunch", 1);

        PlayerPrefs.SetInt("Gifts", 15);

        PlayerPrefs.Save();
    }

    public bool GetPause() {
        return pause;
    }
    public int GetScore() {
        return score;
    }
}