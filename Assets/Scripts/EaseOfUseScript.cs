using Unity.Netcode;
using UnityEngine;

public class EaseOfUseScript : MonoBehaviour
{
    void OnGUI()
    {
        if (GUI.Button(new Rect(50, 50, 150, 50), "StartHost"))
            NetworkManager.Singleton.StartHost();
            
        if (GUI.Button(new Rect(50, 150, 150, 50), "StartClient"))
            NetworkManager.Singleton.StartClient();
    }
}
