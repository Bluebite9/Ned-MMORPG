using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour
{

    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    [SerializeField]
    string dontDrawLayerName = "DontDraw";
    //[SerializeField]
    //GameObject playerGraphics;

    //[SerializeField]
    //GameObject playerUIPrefab;
    //[HideInInspector]
    //public GameObject playerUIInstance;

    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            //disable player graphics
            //SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            ////create player UI
            //playerUIInstance = Instantiate(playerUIPrefab);
            //playerUIInstance.name = playerUIPrefab.name;

            //// configure player
            //PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            //if (ui == null)
            //{
            //    Debug.LogError("Eroare la obtinerea UI-ului pt player");
            //}

            //ui.SetPlayer(GetComponent<Player>());

            GetComponent<Player>().SetupPlayer();

            
        }
    }

    [Command]
    void CmdSetUsername(string playerID, string username)
    {
        Debug.Log("playerId: " + playerID);
        Debug.Log("username: " + username);
        Player player = GameManager.GetPlayer(playerID);

        if (player != null)
        {
            Debug.Log(username + " has joined!");
            player.username = username;
        }
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GameManager.RegisterPlayer(_netID, _player);
    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    void OnDisable()
    {
        //Destroy(playerUIInstance);
        //playerUIInstance.SetActive(false);

        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
        }

        GameManager.UnRegisterPlayer(transform.name);
    }
}
