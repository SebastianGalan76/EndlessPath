using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIHint : MonoBehaviour
{
    private static UIHint instance;

    public Text[] text;

    private Animator animator;

    private void Awake() {
        instance = this;

        animator = GetComponent<Animator>();
    }

    public void ShowHint(string message) {
        foreach(Text t in text) {
            t.text = message;
        }

        animator.SetBool("Show", true);
    }

    public void HideHint() {
        animator.SetBool("Show", false);
    }

    public static UIHint GetInstance() {
        return instance;
    }
}
