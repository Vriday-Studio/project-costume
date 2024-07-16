using UnityEngine;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

public static class QrHandler
{
    public static Color32[] Encode(string textToEncode, int width, int height) {
        var encodeOptions = new QrCodeEncodingOptions
        {
            Height = height,
            Width = width,
            Margin = 0,
            PureBarcode = false
        };
        encodeOptions.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);

        BarcodeWriter writer = new BarcodeWriter{
            Format = BarcodeFormat.QR_CODE,
            Options = encodeOptions
        };
        return writer.Write(textToEncode);
    }

}
