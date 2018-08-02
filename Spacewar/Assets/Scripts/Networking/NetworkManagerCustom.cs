using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkManagerCustom : NetworkManager {

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void StartupHost()
    {
        SetPort();
        singleton.StartHost();
    }

    public void JoinGame()
    {
        SetIPAddress();
        SetPort();
        singleton.StartClient();
    }

    void SetIPAddress()
    {
        string ipAddress = GameObject.Find("InputFieldIPAddress").transform.GetChild(2).GetComponent<Text>().text;
        singleton.networkAddress = ipAddress;
    }
    
    void SetPort()
    {
        int port = 0;
        bool success = int.TryParse(GameObject.Find("InputFieldPort").GetComponent<InputField>().text, out port);
        singleton.networkPort = port;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu")
        {
            //SetupMenuSceneButtons();
            StartCoroutine(SetupMenuSceneButtons());
        } else
        {
            SetupOtherSceneButtons();
        }
    }

    IEnumerator SetupMenuSceneButtons()
    {
        yield return new WaitForSeconds(.3f);
        GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.AddListener(StartupHost);

        GameObject.Find("ButtonJoinGame").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonJoinGame").GetComponent<Button>().onClick.AddListener(JoinGame);
    }

    void SetupOtherSceneButtons()
    {
        GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.AddListener(singleton.StopHost);
    }
}
