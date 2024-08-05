using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GenericButtonInteractable : MonoBehaviour, IInteractable
{
    public UnityEvent onClick;
    public float buttonCooldown = 0.2f;
    public float InteractTime() => buttonCooldown;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite pressedSprite;

    private bool isOnCooldown = false;

    public void Interact()
    {
        if(isOnCooldown) return;

        StartCoroutine(OnClicked(onClick));
        isOnCooldown = true;
        StartCoroutine(StartCooldown());
    }


    private IEnumerator StartCooldown() {
        yield return new WaitForSeconds(buttonCooldown);
        isOnCooldown = false;
    }

    private IEnumerator OnClicked(UnityEvent onClickAction) {
        buttonImage.sprite = pressedSprite;
        yield return new WaitForSeconds(0.1f);
        buttonImage.sprite = normalSprite;
        onClickAction?.Invoke();
    }

}
