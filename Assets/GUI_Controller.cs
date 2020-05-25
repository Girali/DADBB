using UnityEngine;
using UnityEngine.UI;

public class GUI_Controller : MonoBehaviour
{
    static GUI_Controller instance;
    public Timer timer;
    public Text life;
    public GameObject lifeObject;

    public static GUI_Controller Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GUI_Controller>();
            return instance;
        }
    }

    private void Start()
    {
        if(Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            lifeObject.SetActive(false);
        }
    }
}
