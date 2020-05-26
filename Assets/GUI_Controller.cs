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

    public void StartAbility1()
    {

    }

    public void StartAbility2()
    {

    }

    public void StartAbility3()
    {

    }

    public void StartAbility4()
    {

    }

    private void Start()
    {
        if(Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            lifeObject.SetActive(false);
            //TODO YURI desactiver les ability du player
        }
        else
        {
            //TODO YURI desactiver les ability du master
        }
    }
}
