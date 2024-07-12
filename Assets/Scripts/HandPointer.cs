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
    [SerializeField] RectTransform canvasRect;
    [SerializeField, Range(0, 10)] float minInteractTime = 0.5f;
    float currentInteractTime = 0f;

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
                m_PointerEventData.position = this.transform.localPosition;

                List<RaycastResult> raycastResults = new();
                m_Raycaster.Raycast(m_PointerEventData, raycastResults);

                if(raycastResults.Count <= 0) return;

                if(raycastResults[0].gameObject.TryGetComponent<IInteractable>(out var interactable)) {
                    Debug.Log($"Interactable detected!");
                    currentInteractTime += Time.deltaTime;
                    if(currentInteractTime >= minInteractTime) {
                        currentInteractTime = 0f;
                        interactable?.Interact();
                        Debug.Log($"Interactable executed");
                    }
                }

            }
        }
    }
}
