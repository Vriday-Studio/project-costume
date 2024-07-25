using System.Collections;
using System.Collections.Generic;
using NuitrackSDK;
using UnityEngine;

public class NuitrackAvatarHandler : MonoBehaviour
{
    [SerializeField] private CanvasGroupElement startMenu;
    [SerializeField] private CanvasGroupElement costumeMenu;
    [SerializeField] private CanvasGroupElement qrMenu;
    [SerializeField] private CanvasGroupElement doneMenu;
    [SerializeField] private CostumeController costumeController;
    [SerializeField] private float maxIdleTime = 60f;

    private float currentIdleTimer;
    private bool isIdle = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UserData user = NuitrackManager.Users.Current;
        
        if(user == null) {
        }

        if(user != null) {
            currentIdleTimer = 0f;
            isIdle = false;
        }

        if(!isIdle && user == null) {
            currentIdleTimer += Time.deltaTime;
            if(currentIdleTimer >= maxIdleTime) {
                startMenu.EnableElement();
                costumeMenu.DisableElement();
                qrMenu.DisableElement();
                doneMenu.DisableElement();

                costumeController.ResetCostume();

                isIdle = true;
                currentIdleTimer = 0f;
            }
        }
    }
}
