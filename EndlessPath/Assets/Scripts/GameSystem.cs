using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameSystem : MonoBehaviour
{
    public Player player;
    public SkinSystem skinSystem;
    public UISystem ui;
    public Coins coins;
    public BackgroundSystem bgSystem;
    public BackgroundParticlesSystem particlesSystem;
    public AdSystem ad;

    public GameObject playerObj;
    public GameObject pathsParent;

    public GameObject[] paths;
    public bool[] pathsOn;

    private int[] pathHistory = new int[5];
    private float nextPathYPos;

    public GameObject[] decorations;
    public GameObject[] shadows;
    public GameObject coin, gift;

    public AudioSource[] AudioSource;

    public GameObject Background;

    private bool pause, dead;

    private int score;
    private float time;

    public GameObject SoundObjectPrefab;
    public AudioClip[] audioClip;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("FirstLaunch") == 0)
        {
            firstGameLaunch();
        }
    }

    private void Start()
    {
        loadGame();
    }

    private void Update()
    {
        //Changes the player's speed, number of score, background color according to the number of score.
        gameProgress();

        void gameProgress() {
            if (!pause && !dead)
            {
                time += Time.deltaTime;
                if (time >= 1)
                {
                    score++;
                    time = 0;

                    if (score % 10 == 0)
                    {
                        changeBackgroundColor();

                        //Sets the amount of score during the background color change animation
                        StartCoroutine(wait());
                        IEnumerator wait() {
                            yield return new WaitForSeconds(0.15f);
                            ui.setScoreNumber(score);
                        }
                    }
                    else {
                        ui.setScoreNumber(score);
                    }

                    //Increases the player's speed (Min: 3f, Max: 4.05f)
                    if (score % 10 == 0 && score < 100) {
                        player.setPlayerSpeed(true, 0.15f);
                    }
                    if (score % 100 == 0) {
                        player.setPlayerSpeed(true, 0.2f);
                    }
                }
            }
        }
    }

    public void restart() {
        //Removes all paths from the scene
        for (int i = 0; i < pathsParent.transform.childCount; i++) {
            Destroy(pathsParent.transform.GetChild(i).gameObject);
        }

        ad.checkInterstitialAdValue();

        //Resurrections after death
        player.playerResurrection();

        particlesSystem.generateParticles();

        loadGame();
        ui.playAnimation(ui.RestartCircle, "RestartHide");

        StartCoroutine(wait());
        IEnumerator wait() {
            yield return new WaitForSeconds(0.3f);
            
            playGame();
        }
        
    }
    public void loadGame() {
        for (int i = 0; i < pathHistory.Length; i++) {
            pathHistory[i] = 0;   
        }

        //Sets default values after starting the game or dead
        nextPathYPos = 0;
        pause = true;
        dead = false;

        //Sets the default speed and position of the player.
        player.setPlayerSpeed(false, 2.25f);
        player.transform.position = new Vector3(0, 1);
        //Reloads player's skin
        player.setPlayerSkin();

        //Sets the default position of the camera
        Camera.main.transform.position = new Vector3(0, 0f, -10f);

        //Reloads the number of coins
        coins.reloadCoinsAmount(false);

        //Load path
        for (int i = 0; i < 10; i++) {
            loadNextPaths();
        }
    }
    public void playGame() {

        //Waits for end of the animation
        StartCoroutine(wait());

        //Shows the amount of score two seconds after start the game
        StartCoroutine(showScore());

        ui.changeBPlayStatus(false, 0);

        IEnumerator wait()
        {
            yield return new WaitForSeconds(0.3f);

            time = 0;
            score = 0;
            pause = false;
            dead = false;
        }
        IEnumerator showScore()
        {
            yield return new WaitForSeconds(2f);
            if (!dead)
            {
                ui.scoreAnimation("ShowScore");
            }
        }
    }
    public void death()
    {
        pause = true;
        dead = true;
        ui.setScoreNumber(score);

        if (breakRecord())
        {
            PlayerPrefs.SetInt("Record", score);
            PlayerPrefs.Save();

            GooglePlayServices.postToLeaderboard();
            GooglePlayServices.postToAchievements();

            ui.newRecord(true);
            ui.setRecordNumber(score);
        }
        else {
            ui.newRecord(false);
        }

        StartCoroutine(wait());
        IEnumerator wait()
        {
            yield return new WaitForSeconds(0.2f);

            ui.death();
        }

        bool breakRecord() {
            if (PlayerPrefs.GetInt("Record") < score)
            {
                return true;
            }
            else {
                return false;
            }
        }
    }

    public void loadNextPaths() {
        RandomAgain:
        int randomValue = Random.Range(1, paths.Length);
        if (nextPathYPos == 0) { randomValue = 0; }
        if (!pathsOn[randomValue] || !checkPathHistory(randomValue)) {
            goto RandomAgain;
        }

        for (int i = pathHistory.Length - 1; i > 0; i--) {
            pathHistory[i] = pathHistory[i-1];
        }
        pathHistory[0] = randomValue;

        //Instantiates a random path object
        GameObject path = Instantiate(paths[randomValue]);
        path.transform.parent = pathsParent.transform;
        path.transform.position = new Vector3(0, nextPathYPos);

        nextPathYPos += path.GetComponent<Path>().getPathLength();

        //Instantiates a random path decoration object
        int randomDecoration = Random.Range(0, decorations.Length);
        path.GetComponent<Path>().generateDecoration(decorations[randomDecoration], shadows[randomDecoration]);
        
        path.GetComponent<Path>().generateCoinsOrGift(coin, gift);
    }
    private bool checkPathHistory(int value) {
        for (int i = 0; i < pathHistory.Length; i++) {
            if (value == pathHistory[i] && value!=0) {
                return false;
            }
        }
        return true;
    }
    private void changeBackgroundColor()
    {
        Background.GetComponent<BackgroundSystem>().changeColor();
        ui.scoreAnimation("ChangeColorScore");
    }

    private void firstGameLaunch() {
        PlayerPrefs.SetInt("selectedSkinTheme", 0);
        PlayerPrefs.SetInt("selectedSkinID", 0);

        PlayerPrefs.SetInt("Settings-Vibes", 1);
        PlayerPrefs.SetInt("Settings-Particles", 1);
        PlayerPrefs.SetFloat("Settings-Volume", 0.2f);

        PlayerPrefs.SetInt("Skin0.0", 1);

        PlayerPrefs.SetInt("FirstLaunch", 1);
        PlayerPrefs.Save();
    }

    public void playAudio(int audioID) {
        GameObject audio = Instantiate(SoundObjectPrefab);
        audio.transform.SetParent(playerObj.transform);

        audio.GetComponent<AudioSource>().clip = getAudioClip();
        audio.GetComponent<AudioSource>().volume = getAudioVolume();
        audio.GetComponent<AudioSource>().Play();
        Destroy(audio, audio.GetComponent<AudioSource>().clip.length);

        AudioClip getAudioClip()
        {
            return audioClip[audioID];
        }
        float getAudioVolume()
        {
            if (audioID == 1)
            {
                return (PlayerPrefs.GetFloat("Settings-Volume") * 4f);
            }
            
            return PlayerPrefs.GetFloat("Settings-Volume");
        }
    }

    public bool getPause() {
        return pause;
    }
    public bool getDead() {
        return dead;
    }
    public int getScore() {
        return score;
    }
}