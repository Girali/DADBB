using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkController : MonoBehaviourPunCallbacks
{
    byte maxPlayersPerRoom = 4;
    bool isConnecting;
    public UnityEngine.UI.Text feedback;
    public UnityEngine.UI.Button playButton;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void FeedbackUser(string text)
    {
        feedback.text = text;
    }

    public void Connect()
    {
        isConnecting = true;
        playButton.interactable = false;
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
            FeedbackUser("Joining room...");
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            FeedbackUser("Connecting...");
        }
    }

    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
            FeedbackUser("Joining room...");
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom("MasterRoom", new RoomOptions() { MaxPlayers = this.maxPlayersPerRoom });
        FeedbackUser("Creating room...");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        isConnecting = false;
        FeedbackUser("Failed...");
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("MainScene");
    }
}