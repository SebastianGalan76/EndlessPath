using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class CoinSystem : MonoBehaviour {
    private static CoinSystem instance;

    [SerializeField] private UICoin ui;

    private int amount;

    private void Awake() {
        instance = this;

        amount = PlayerPrefs.GetInt("Coins", 0);

        ui.ChangeAmount(amount);
    }

    public void PickUp() {
        Add(1);

        ui.PickUp(amount);
    }

    public void Add(int value) {
        ChangeAmount(value);
    }
    public void Take(int value) {
        ChangeAmount(-value);
    }
    public bool CheckAmount(int value) {
        return amount >= value;
    }

    private void ChangeAmount(int value) {
        amount += value;

        PlayerPrefs.SetInt("Coins", amount);
        PlayerPrefs.Save();

        ui.ChangeAmount(amount);
    }

    public static CoinSystem GetInstance() {
        return instance;
    }
}
