using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyView : MonoBehaviour
{
    public Image lifeBar;
    public Text lifeText;

    public void Init(int life)
    {
        lifeBar.fillAmount = 1f;
        lifeText.text = life.ToString();
    }

    public void UpdateLife(int life,int maxLife)
    {
        lifeBar.fillAmount = life / maxLife;
        lifeText.text = life.ToString();
    }
}
