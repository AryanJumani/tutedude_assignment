using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ButtonManager : MonoBehaviourPunCallbacks
{
    public GameObject createPanel;
    public GameObject createAndJoinPanel;
    public GameObject joinPanel;
    public InputField createInputField;
    public InputField joinInputField;

    private void Start()
    {
        createPanel.SetActive(false);
        joinPanel.SetActive(false);
        createAndJoinPanel.SetActive(true);
    }
    public void onCreateSubmit()
    {
        createAndJoinPanel.SetActive(false);
        joinPanel.SetActive(false);
        createPanel.SetActive(true);
    }
    public void onBack()
    {
        createPanel.SetActive(false);
        joinPanel.SetActive(false);
        createAndJoinPanel.SetActive(true);
    }
    public void onJoinSubmit()
    {
        createPanel.SetActive(false);
        createAndJoinPanel.SetActive(false);
        joinPanel.SetActive(true);
    }

    public void createInput()
    {
        PhotonNetwork.CreateRoom(createInputField.text);
    }
    public void joinInput()
    {
        PhotonNetwork.JoinRoom(joinInputField.text);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("MainScene");
    }

}
