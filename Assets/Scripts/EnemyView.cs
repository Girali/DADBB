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
        lifeBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 110);
        lifeText.text = life.ToString();
    }

    public void UpdateLife(int life,int maxLife)
    {
        lifeBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(0, 110, life / (float)maxLife));
        lifeText.text = life.ToString();
    }
}
