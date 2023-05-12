using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    [SerializeField] private GameSystem system;

    [SerializeField] private Text[] scoreT;
    [SerializeField] private Text[] recordScoreT;

    [SerializeField] private GameObject PDead;

    [SerializeField] private GiftSystem gift;
    [SerializeField] private GameObject backgroundButton;

    [SerializeField] private Animator animator, circleAnimator;

    private void Start()
    {
        UIHint.GetInstance().ShowHint("HOLD & DRAG TO PLAY");

        SetRecordNumber(PlayerPrefs.GetInt("Record", 0));
    }

    public void PlayGame()
    {
        if (!system.player.IsDead())
        {
            animator.Play("PMenuHide");
            system.PlayGame();
        }
        else {
            ChangeBackgroundButtonVisibility(false, 0);
            Restart();
        }

        gift.HideGiftButton();
        UIHint.GetInstance().HideHint();
    }

    public void Die() {
        UIHint.GetInstance().ShowHint("TAP TO RESTART");

        animator.Play("PMenuShow");

        ChangeBackgroundButtonVisibility(true, 0.4f);

        PDead.SetActive(true);

        ScoreAnimation("HideScore");

        gift.ShowGiftButton();
    }

    public void Restart() {
        animator.Play("PMenuHide");
        circleAnimator.Play("ShowCircle");

        StartCoroutine(wait());
        IEnumerator wait() {
            yield return new WaitForSeconds(0.5f);
            PDead.SetActive(false);
            system.RestartGame();

            circleAnimator.Play("HideCircle");
        }
    }

    public void SetScoreNumber(int score)
    {
        foreach(Text text in scoreT) {
            text.text = score.ToString();
        }
    }
    public void SetRecordNumber(int record) {
        foreach(Text text in recordScoreT) {
            text.text = record.ToString();
        }
    }

    public void ChangeBackgroundButtonVisibility(bool value, float delay) {
        StartCoroutine(Wait());

        IEnumerator Wait() {
            yield return new WaitForSeconds(delay);
            
            backgroundButton.SetActive(value);
        }
    }
    public void NewRecord(bool value) {
        //If value equals true, it shows panel with "new record" comunicat.
        PDead.transform.Find("PNewRecord").gameObject.SetActive(value);
        PDead.transform.Find("PNormal").gameObject.SetActive(!value);
    }

    public void RateGame() {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.Coresaken.EndlessPath");
    }

    public void PlayAnimation(GameObject obj, string animation)
    {
        obj.GetComponent<Animator>().Play(animation);
    }
    public void ScoreAnimation(string animation)
    {
        scoreT[0].gameObject.GetComponent<Animator>().Play(animation);
    }
}
