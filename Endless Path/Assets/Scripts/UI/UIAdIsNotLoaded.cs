using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAdIsNotLoaded : UIPanel, IPanel
{
    private static UIAdIsNotLoaded instance;

    [SerializeField] private GameObject panel;

    private Animator animator;

    private void Awake() {
        instance = this;

        animator = panel.GetComponent<Animator>();
    }

    public void OpenPanel() {
        base.OpenPanel(panel, animator);
    }

    public void ClosePanel() {
        base.ClosePanel(panel, animator);
    }

    public static UIAdIsNotLoaded GetInstance() {
        return instance;
    }
}
