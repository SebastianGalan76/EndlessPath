using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UINotEnoughCoins : UIPanel, IPanel
{
    [SerializeField] private GameObject panel;

    private Animator animator;

    private void Awake() {
        animator = panel.GetComponent<Animator>();
    }

    public void OpenPanel() {
        base.OpenPanel(panel, animator);
    }


    public void ClosePanel() {
        base.ClosePanel(panel, animator);
    }
}
