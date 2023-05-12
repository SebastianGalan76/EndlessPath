using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIGift : UIPanel, IPanel
{
    [SerializeField] private Text amount;
    [SerializeField] private GameObject giftBtn;

    [SerializeField] private GameObject giftPickUpPrefab;
    [SerializeField] private Transform player;

    [SerializeField] private GameObject panel;

    [SerializeField] private GameObject openBtn;
    [SerializeField] private GameObject closeBtn;

    [SerializeField] private Image skinFromGift;
    [SerializeField] private Animator giftAnimator;

    private RectTransform canvasRect;
    private Animator animator;

    private void Awake() {
        canvasRect = gameObject.transform.parent.GetComponent<RectTransform>();

        animator = panel.GetComponent<Animator>();
    }

    public void PickUp() {
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(player.position);
        Vector2 Player_ScreenPosition = new Vector2(
        ((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

        GameObject giftPickUp = Instantiate(giftPickUpPrefab);
        giftPickUp.transform.SetParent(gameObject.transform, false);
        giftPickUp.GetComponent<RectTransform>().anchoredPosition = Player_ScreenPosition;
    }

    public void ChangeGiftBtnVisibility(int giftAmount) {
        if(giftAmount > 0 && !SkinSystem.GetInstance().AllSkinsAreUnlocked()) {
            amount.text = giftAmount.ToString();

            giftBtn.SetActive(true);

            return;
        }

        giftBtn.SetActive(false);
    }

    public void OpenGift(Skin skin) {
        skinFromGift.sprite = skin.shopSprite;
        skinFromGift.SetNativeSize();

        giftAnimator.Play("OpenGift");

        ShowPanelButton(closeBtn, 2f);
    }

    public void OpenPanel() {
        ShowPanelButton(openBtn);

        base.OpenPanel(panel, animator);
    }

    public void ClosePanel() {
        base.ClosePanel(panel, animator);
        giftAnimator.Rebind();
    }

    private void ShowPanelButton(GameObject button, float delay = 0f) {
        openBtn.SetActive(false);
        closeBtn.SetActive(false);

        StartCoroutine(wait());
        IEnumerator wait() {
            yield return new WaitForSeconds(delay);
            button.SetActive(true);
        }
    }
}
