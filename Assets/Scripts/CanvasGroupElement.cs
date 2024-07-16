using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using System;

public class CanvasGroupElement : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private AnimationDirection onEnableDirection;
    [SerializeField] private AnimationDirection onDisableDirection;

    [SerializeField] private float animationDuration = 0.65f;
    [SerializeField] private float startAnimationPositionOffset = 650f;

    private Vector3 originalPosition;
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }

    public void EnableElement()
    {
        SetStartPosition(onEnableDirection);
        canvasGroup.alpha = 0f;
        SetCanvasInteractable(false);
        DOTween.Init();
        canvasGroup.DOFade(1f, animationDuration);
        transform.DOMove(originalPosition, animationDuration)
            .OnComplete( () => SetCanvasInteractable(true));
    }
    
    public void EnableElement(int direction)
    {
        bool isInRange = !string.IsNullOrEmpty(Enum.GetName(typeof(AnimationDirection), direction));
        AnimationDirection animationDirection = isInRange == true ? (AnimationDirection)direction : AnimationDirection.NONE;
        SetStartPosition(animationDirection);
        canvasGroup.alpha = 0f;
        SetCanvasInteractable(false);
        DOTween.Init();
        canvasGroup.DOFade(1f, animationDuration);
        transform.DOMove(originalPosition, animationDuration)
            .OnComplete( () => SetCanvasInteractable(true));
    }

    public void DisableElement()
    {
        SetTargetPosition(onDisableDirection);
        canvasGroup.alpha = 1f;
        SetCanvasInteractable(false);
        DOTween.Init();
        canvasGroup.DOFade(0f, animationDuration);
        transform.DOMove(targetPosition, animationDuration)
            .OnComplete(() => {
                    SetCanvasInteractable(false);
                    transform.position = originalPosition;
                });
    }

    public void DisableElement(int direction)
    {
        bool isInRange = !string.IsNullOrEmpty(Enum.GetName(typeof(AnimationDirection), direction));
        AnimationDirection animationDirection = isInRange == true ? (AnimationDirection)direction : AnimationDirection.NONE;
        SetTargetPosition(animationDirection);
        canvasGroup.alpha = 1f;
        SetCanvasInteractable(false);
        DOTween.Init();
        canvasGroup.DOFade(0f, animationDuration);
        transform.DOMove(targetPosition, animationDuration)
            .OnComplete(() => {
                    SetCanvasInteractable(false);
                    transform.position = originalPosition;
                });
    }

    public bool GetVisibility() {
        return canvasGroup.interactable;
    }

    [Button]
    public void InvertVisibility()
    {
        canvasGroup.alpha = canvasGroup.alpha == 0f ? 1f : 0f;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
        canvasGroup.interactable = canvasGroup.interactable == true ? false : true;
    }

    public void SetCanvasInteractable(bool status)
    {
        canvasGroup.blocksRaycasts = status;
        canvasGroup.interactable = status;
    }

    private void SetStartPosition(AnimationDirection direction)
    {
        switch (direction)
        {
            case AnimationDirection.NONE :
                originalPosition = transform.position;
                targetPosition = transform.position;
                break;
            case AnimationDirection.DOWN:
                originalPosition = transform.position;
                transform.position += new Vector3(0f, startAnimationPositionOffset, 0f);
                break;
            case AnimationDirection.UP:
                originalPosition = transform.position;
                transform.position -= new Vector3(0f, startAnimationPositionOffset, 0f);
                break;
            case AnimationDirection.LEFT:
                originalPosition = transform.position;
                transform.position -= new Vector3(startAnimationPositionOffset, 0f, 0f);
                break;
            case AnimationDirection.RIGHT:
                originalPosition = transform.position;
                transform.position += new Vector3(startAnimationPositionOffset, 0f, 0f);
                break;
            default:
                break;
        }
    }

    private void SetTargetPosition(AnimationDirection direction)
    {
        switch (direction)
        {
            case AnimationDirection.NONE:
                originalPosition = transform.position;
                targetPosition = transform.position;
                break;
            case AnimationDirection.DOWN:
                originalPosition = transform.position;
                targetPosition = transform.position - new Vector3(0f, startAnimationPositionOffset, 0f);
                break;
            case AnimationDirection.UP:
                originalPosition = transform.position;
                targetPosition = transform.position + new Vector3(0f, startAnimationPositionOffset, 0f);
                break;
            case AnimationDirection.LEFT:
                originalPosition = transform.position;
                targetPosition = transform.position - new Vector3(startAnimationPositionOffset, 0f, 0f);
                break;
            case AnimationDirection.RIGHT:
                originalPosition = transform.position;
                targetPosition = transform.position + new Vector3(startAnimationPositionOffset, 0f, 0f);
                break;
            default:
                break;
        }
    }
}
