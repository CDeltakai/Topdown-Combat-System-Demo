using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] Button serverButton;
    [SerializeField] Button hostButton;
    [SerializeField] Button clientButton;

    void Awake()
    {
        serverButton.onClick.AddListener(StartServer);
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);
    }

    void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }

    void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }


}
