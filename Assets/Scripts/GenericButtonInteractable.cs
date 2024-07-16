using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericButtonInteractable : MonoBehaviour, IInteractable
{
    public UnityEvent onClick;
    public float buttonCooldown = 0.2f;

    private bool isOnCooldown = false;

    public void Interact()
    {
        if(isOnCooldown) return;

        onClick.Invoke();
        isOnCooldown = true;
        StartCoroutine(StartCooldown());
    }

    private IEnumerator StartCooldown() {
        yield return new WaitForSeconds(buttonCooldown);
        isOnCooldown = false;
    }

}
