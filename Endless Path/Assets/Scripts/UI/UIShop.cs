using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using UnityEngine;

public class UIShop : MonoBehaviour
{
    [SerializeField] private UIPurchase purchase;

    [SerializeField] private Animator menuAnimator;
    [SerializeField] private Animator animator;

    [SerializeField] private Image[] categoriesBtn;
    [SerializeField] private Sprite categoryBtn, selectedCategoryBtn;

    [SerializeField] private GameObject[] categories;

    [SerializeField] private Text[] categoryName;

    public void OpenPanel() {
        menuAnimator.Play("OpenShop-PMenu");
        animator.Play("OpenShop-PShop");
    }

    public void ClosePanel() {
        menuAnimator.Play("CloseShop-PMenu");
        animator.Play("CloseShop-PShop");
    }

    public void OpenPurchasePanel(Skin skin) {
        purchase.OpenPanel(skin);
    }

    public void ChangeCategory(int categoryID) {
        ChangeCategoryButton();
        ChangeCategoryContainer();
        ChangeCategoryName();

        void ChangeCategoryContainer() {
            CloseAllCategory();

            categories[categoryID].SetActive(true);

            void CloseAllCategory() {
                foreach(GameObject c in categories) {
                    c.SetActive(false);
                }
            }
        }
        void ChangeCategoryButton() {
            CloseAllCategory();

            categoriesBtn[categoryID].sprite = selectedCategoryBtn;

            void CloseAllCategory() {
                foreach(Image i in categoriesBtn) {
                    i.sprite = categoryBtn;
                }
            }
        }
        void ChangeCategoryName() {
            SkinCategoryEnum cat = (SkinCategoryEnum)categoryID;
            foreach(Text text in categoryName) {
                text.text = cat.ToString()+"S";
            }
        }
    }
}
