using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHpBar : MonoBehaviour
{
    public Image hpBarFillImg;
    public void UpdateUIHpBar(int maxHp, int currentHp, int subValue)
    {
        hpBarFillImg.fillAmount = Mathf.Clamp01((float)(currentHp - subValue) / maxHp);
    }
    public void ResetHpBar()
    {
        hpBarFillImg.fillAmount = 1f;
    }
}
