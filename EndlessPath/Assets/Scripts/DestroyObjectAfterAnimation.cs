using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectAfterAnimation : MonoBehaviour
{
    float delay = 0.5f;

    void Start()
    {
        Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
    }
}
