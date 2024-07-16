using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PhotoHostingResponse
{
    public bool success;
    public int status;
    public string id;
    public string key;
    public string name;
    public string link;
    public DateTime expires;
    public string expiry;
}