using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandCollapse : MonoBehaviour
{
    private Animator animator;
    private bool isExpanded = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Toggle()
    {
        isExpanded = !isExpanded;
        animator.SetBool("isExpanded", isExpanded);
    }
}

