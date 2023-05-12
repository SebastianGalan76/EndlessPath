using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour {
    private static ShopSystem instance;

    [SerializeField] private UIShop ui;

    private void Awake() {
        instance = this;
    }

    public void OpenPurchasePanel(Skin skin) {
        ui.OpenPurchasePanel(skin);
    }

    public static ShopSystem GetInstance() {
        return instance;
    }
}
