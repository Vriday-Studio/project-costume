using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PhotoController : MonoBehaviour, IInteractable
{
    private string IMAGE_PATH;

    [SerializeField] private RawImage photoResultUI;
    [SerializeField] private CanvasGroup cameraUI;

    // Start is called before the first frame update
    void Start()
    {
        IMAGE_PATH = Application.dataPath;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Interact()
    {
        var file = TakePicture();

        Texture2D tex = null;
        byte[] fileData;

        if (!File.Exists(file)) return;

        fileData = File.ReadAllBytes(file);
        tex = new Texture2D(2, 2);
        tex.LoadImage(fileData); //this will auto-resize the texture dimensions.

        photoResultUI.texture = tex;

        cameraUI.alpha = 1f;
        cameraUI.interactable = true;
        cameraUI.blocksRaycasts = true;
    }

    public string TakePicture() {
        var fileName = IMAGE_PATH + "_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        ScreenCapture.CaptureScreenshot(fileName);
        return fileName;
    }

}
