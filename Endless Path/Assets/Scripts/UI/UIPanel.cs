using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    protected void OpenPanel(GameObject panel, Animator animator) {
        panel.SetActive(true);
        animator.Play("ShowPanel");
    }

    protected void ClosePanel(GameObject panel, Animator animator) {
        animator.Play("ClosePanel");

        StartCoroutine(wait());
        IEnumerator wait() {
            yield return new WaitForSeconds(0.3f);
            panel.SetActive(false);
        }
    }
}
