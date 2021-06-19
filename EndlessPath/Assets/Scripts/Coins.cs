using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public Text coins;
    public GameObject CoinsAmount;
    private GameSystem system;

    private void Start()
    {
        system = GetComponent<GameSystem>();
        reloadCoinsAmount(false);
    }

    public void addCoins(int amount, bool animation = true) {
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + amount);
        PlayerPrefs.Save();

        reloadCoinsAmount(animation);
    }

    public void takeCoins(int amount) {
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - amount);
        PlayerPrefs.Save();

        reloadCoinsAmount(false);
    }

    public bool checkCoins(int amount) {
        if (PlayerPrefs.GetInt("Coins") >= amount) {
            return true;
        }
        return false;
    }

    public void reloadCoinsAmount(bool animation) {
        if (animation)
        {
            system.ui.pickUpCoins();

            StartCoroutine(wait());
            IEnumerator wait()
            {
                yield return new WaitForSeconds(0.35f);
                CoinsAmount.GetComponent<Animator>().Play("TCoinPickUp");
                StartCoroutine(wait2());
            }
            IEnumerator wait2()
            {
                yield return new WaitForSeconds(0.1f);
                coins.text = PlayerPrefs.GetInt("Coins").ToString();
            }
        }
        else {
            coins.text = PlayerPrefs.GetInt("Coins").ToString();
        }
    }
}
