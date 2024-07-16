using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QRTestingSample : MonoBehaviour
{
    [SerializeField] private RawImage qrRaw;
    [SerializeField] private string encode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            CreateQR();
    }

    private void CreateQR() {
        var stringToQR = QrHandler.Encode(encode, 256, 256);
        
        Texture2D encodedTexture = new Texture2D(256, 256);
        encodedTexture.SetPixels32(stringToQR);
        encodedTexture.Apply();
        
        qrRaw.texture = encodedTexture;
    }
}
