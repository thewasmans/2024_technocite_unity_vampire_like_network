using Unity.Netcode;
using UnityEngine;

public class EaseOfUseScript : MonoBehaviour
{
    void OnGUI()
    {
        if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsClient)
        {
            if (GUI.Button(new Rect(50, 750, 150, 50), "Disconnect"))
                NetworkManager.Singleton.Shutdown();
        }
        else
        {
            if (GUI.Button(new Rect(50, 50, 150, 50), "StartHost"))
                NetworkManager.Singleton.StartHost();

            if (GUI.Button(new Rect(50, 150, 150, 50), "StartClient"))
                NetworkManager.Singleton.StartClient();
            if (GUI.Button(new Rect(50, 250, 150, 50), "StartServer"))
                NetworkManager.Singleton.StartServer();
        }
    }
}
