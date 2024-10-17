using System;
using System.Net;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class EaseOfUseScript : MonoBehaviour
{
    private UnityTransport mUnityTransport;
    private string mIpAdress;

    void Awake()
    {
        mUnityTransport = GetComponent<UnityTransport>();
        mIpAdress = "Enter IP";
    }

    void OnGUI()
    {
        var style = new GUIStyle();
        style.fontSize = 30;
        if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsClient)
        {
            GUI.enabled = false;
            var text = GUI.TextField(
                new Rect(0, 0, 150, 0),
                mUnityTransport.ConnectionData.Address,
                style
            );
            GUI.enabled = true;
            if (NetworkManager.Singleton.IsServer)
                if (GUI.Button(new Rect(50, 750, 150, 50), "Disconnect"))
                    NetworkManager.Singleton.Shutdown();
        }
        else
        {
            mIpAdress = GUI.TextField(new Rect(0, 0, 150, 30), mIpAdress, 16);
            var isIpValid = IsValidIP(mIpAdress);
            if (isIpValid)
            {
                mUnityTransport.SetConnectionData(mIpAdress, 7777);
            }
            GUI.enabled = isIpValid;
            if (GUI.Button(new Rect(50, 150, 150, 50), "StartClient"))
                NetworkManager.Singleton.StartClient();
            GUI.enabled = true;
            if (GUI.Button(new Rect(50, 50, 150, 50), "StartHost"))
                NetworkManager.Singleton.StartHost();
            if (GUI.Button(new Rect(50, 250, 150, 50), "StartServer"))
                NetworkManager.Singleton.StartServer();
        }
    }

    private bool IsValidIP(string text)
    {
        return IPAddress.TryParse(text, out IPAddress ip);
    }
}
