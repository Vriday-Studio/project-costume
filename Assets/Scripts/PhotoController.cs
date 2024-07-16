using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZXing.QrCode.Internal;

public class PhotoController : MonoBehaviour
{
    private Vector2Int photoDimension = new Vector2Int(1080, 1920);

    [SerializeField] private RawImage photoResultUI;
    [SerializeField] private RawImage qrResultUI;
    [SerializeField] private CanvasGroupElement menuUI;
    [SerializeField] private CanvasGroupElement cameraUI;
    [SerializeField] private CanvasGroupElement qrUI;
    [SerializeField] private TextMeshProUGUI uploadingText;

    [Header("Photo UI To Remove")]
    [SerializeField] private List<CanvasGroupElement> photoUIToHide;
    [SerializeField] private TextMeshProUGUI countdownText;
    private float currentTimer = 3.5f;

    private bool isOnCountdown = false;
    private bool isCapturingImage = false;
    private bool isUploading = false;
    private string filePath;
    private string fileName;

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    private void FixedUpdate()  {
        if(!isOnCountdown) return;

        if(currentTimer <= 0f) {        
            StartCoroutine(InitiateTakePicture());
        }

        currentTimer -= Time.deltaTime;
        countdownText.text = currentTimer.ToString("F0");
    }


    public void TakePicture() {
        if(isCapturingImage)
            return;
        
        countdownText.gameObject.SetActive(true);

        isCapturingImage = true;
        isOnCountdown = true;

        foreach (var ui in photoUIToHide)
            ui.DisableElement();
    }

    public IEnumerator InitiateTakePicture() {
        isOnCountdown = false;
        countdownText.gameObject.SetActive(false);

        filePath = "Assets/Screenshots/";
        if(!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);
        
        fileName = "ScreenCapture_" + System.DateTime.UtcNow.ToString("ydd-MM-yyyy-HH-mm-ss") + ".png";
        filePath = Path.Join(filePath, fileName);

        ScreenCapture.CaptureScreenshot(filePath);
        
        Debug.Log($"File saved at : {filePath}");

        yield return new WaitForSeconds(0.5f);

        var pngByte = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(photoResultUI.texture.width, photoResultUI.texture.height);
        texture.LoadImage(pngByte);
        photoResultUI.texture = texture;

        foreach (var ui in photoUIToHide)
            ui.EnableElement();

        menuUI.DisableElement();
        cameraUI.EnableElement();
        
        isCapturingImage = false;
    }

    public void UploadPicture() {
        if(!File.Exists(filePath) || isUploading) {
            Debug.LogWarning($"Either file doesnt exist at : {filePath} or it's still uploading");
            return;
        }
        
        isUploading = true;
        cameraUI.SetCanvasInteractable(false);

        uploadingText.gameObject.SetActive(true);
        uploadingText.text = "Sedang mengunggah foto...";
        
        var pngByte = File.ReadAllBytes(filePath);
        var dateExpires = DateTime.UtcNow.AddMinutes(30);
        StartCoroutine(PhotoHosting.Upload(OnUploadSuccess, OnUploadFailed, fileName, pngByte, dateExpires));
    }
    
    public void SetQRCodeUI(string link) {
        var stringToQR = QrHandler.Encode(link, 256, 256);
        
        Texture2D encodedTexture = new Texture2D(256, 256);
        encodedTexture.SetPixels32(stringToQR);
        encodedTexture.Apply();

        qrResultUI.texture = encodedTexture;
    }

    private void OnUploadFailed()
    {
        Debug.Log("Failed callback");
        cameraUI.SetCanvasInteractable(true);
        uploadingText.text = "Foto gagal diunggah.";
        
        isUploading = false;
    }

    private void OnUploadSuccess(PhotoHostingResponse response)
    {
        Debug.Log($"Success callback {response.link}");
        
        uploadingText.text = "Foto berhasil diunggah!";
        isUploading = false;
        SetQRCodeUI(response.link);

        cameraUI.DisableElement();
        qrUI.EnableElement();
    }
}
