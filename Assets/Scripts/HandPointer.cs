using System.Collections;
using System.Collections.Generic;
using NuitrackSDK;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandPointer : MonoBehaviour
{
    public enum Hands { left = 0, right = 1 };

    [SerializeField] Hands currentHand;

    [Header("Visualization")]
    [SerializeField] Image visualPointer;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite pressSprite;

    [SerializeField]  GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    [SerializeField] EventSystem m_EventSystem;
    [SerializeField] RectTransform baseHandRect;
    [SerializeField] RectTransform parentRect;
    [SerializeField] Image loadingRect;
    float currentInteractTime = 0f;
    IInteractable currentInterractable;
    float currentInterractableIdleTimer = 0f;
    float maxCurrentInterractableIdleTimer = 2f;

    bool active = false;

    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
    }

    public bool Press
    {
        get; private set;
    }

    private void Start() {
        loadingRect.fillAmount = 0f;
    }

    void Update()
    {
        active = false;

        UserData user = NuitrackManager.Users.Current;

        if (user != null)
        {
            UserData.Hand handContent = currentHand == Hands.right ? user.RightHand : user.LeftHand;

            if (handContent != null)
            {
                m_PointerEventData = new PointerEventData(m_EventSystem);

                Vector3 lastPosition = baseHandRect.position;
                baseHandRect.anchoredPosition = handContent.AnchoredPosition(parentRect.rect, baseHandRect);
                m_PointerEventData.position = this.transform.position;

                List<RaycastResult> raycastResults = new();
                m_Raycaster.Raycast(m_PointerEventData, raycastResults);

                if(raycastResults.Count <= 0) {
                    if(loadingRect.fillAmount <= 0f) return;

                    currentInterractableIdleTimer += Time.deltaTime;
                    if(currentInterractableIdleTimer >= maxCurrentInterractableIdleTimer) {
                        currentInterractable = null;
                        currentInterractableIdleTimer = 0f;
                        loadingRect.fillAmount = 0f;
                    }
                    return;
                }

                if(raycastResults[0].gameObject.TryGetComponent<IInteractable>(out var interactable)) {
                    currentInterractable = interactable;
                    active = true;
                    currentInteractTime += Time.deltaTime;
                    loadingRect.fillAmount = currentInteractTime / interactable.InteractTime(); 

                    if(currentInteractTime >= currentInterractable.InteractTime()) {
                        currentInteractTime = 0f;
                        loadingRect.fillAmount = 0f;
                        interactable?.Interact();
                        active = false;
                        currentInterractable = null;
                    }
                }
                
                if(currentInterractable == null) {
                    if(loadingRect.fillAmount <= 0f) return;
                    
                    currentInterractableIdleTimer += Time.deltaTime;
                    if(currentInterractableIdleTimer >= maxCurrentInterractableIdleTimer) {
                        currentInterractableIdleTimer = 0f;
                        loadingRect.fillAmount = 0f;
                    }
                }

            }
            
            Press = Press && active;
        }
    }
}
