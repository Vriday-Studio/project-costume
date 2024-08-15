using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public static class PhotoHosting
{
    public static IEnumerator Upload(Action<PhotoHostingResponse> onPayloadSuccess, Action onPaylaodFailed, string filename, byte[] fileBinary, DateTime expires, int maxDownloads = 1, bool autoDelete = true) {
        WWWForm form = new();

        form.AddBinaryData(filename, fileBinary);
        // form.AddField("expires", expires.ToString("yyyy-MM-ddThh:mm:ss.000Z"));
        // form.AddField("id", "2ee47cd8-8c47-4779-bd34-4718443195d8");
        // form.AddField("key", "7FUPNX3.YJKAA4M-GC8M2G6-KZDYRFS-AK7PM7J");
        form.AddField("maxDownloads", maxDownloads);
        form.AddField("autoDelete", autoDelete.ToString());

        using(var unityWebRequest = UnityWebRequest.Post("https://file.io/", form)) {
            // unityWebRequest.SetRequestHeader("Authorization", "Token 555myToken555");

            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result != UnityWebRequest.Result.Success) 
            {
                onPaylaodFailed?.Invoke();
                Debug.Log($"Failed to upload : {unityWebRequest.result} - {unityWebRequest.error}");
            }
            else 
            {
                var json = unityWebRequest.downloadHandler.text;
                var response = JsonConvert.DeserializeObject<PhotoHostingResponse>(json);

                onPayloadSuccess?.Invoke(response);
                Debug.Log($"Finished Uploading. {response.link}, {response.id}, {response.success}");
            }
        }
        
    }
}
