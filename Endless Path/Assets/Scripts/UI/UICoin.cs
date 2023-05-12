using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UICoin : MonoBehaviour
{
    [SerializeField] private Text amount;
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject coinPickUpPrefab;
    [SerializeField] private Transform player;

    private RectTransform canvasRect;

    private void Awake() {
        canvasRect = gameObject.transform.parent.GetComponent<RectTransform>();
    }

    public void ChangeAmount(int coinAmount) {
        amount.text = coinAmount.ToString();
    }

    public void PickUp(int coinAmount) {
        PickUpAnimation();

        StartCoroutine(Wait());
        IEnumerator Wait() {
            yield return new WaitForSeconds(0.35f);
            
            animator.Play("TCoinPickUp");
            
            StartCoroutine(Wait2());

            IEnumerator Wait2() {
                yield return new WaitForSeconds(0.1f);
                
                ChangeAmount(coinAmount);
            }
        }
        

        return;

        void PickUpAnimation() {
            Vector2 viewportPosition = Camera.main.WorldToViewportPoint(player.position);
            Vector2 playerScreenPosition = new Vector2(
            ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

            GameObject coinPickUp = Instantiate(coinPickUpPrefab);
            coinPickUp.transform.SetParent(gameObject.transform, false);
            coinPickUp.GetComponent<RectTransform>().anchoredPosition = playerScreenPosition;
        }
    }
}
