using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PowerBar : MonoBehaviour
{
    public Image powerBarFill;
    public Image cooldownBarFill;

    public Image[] childrenImg;

    public void SetPower(float power)
    {
        powerBarFill.fillAmount = power;
        setAlpha(power * 8);
    }
    
    public void SetCooldown(float cooldown)
    {
        cooldownBarFill.fillAmount = cooldown;
        setAlpha(cooldown * 8);
    }

    private void setAlpha(float alpha)
    {
        foreach (Image img in childrenImg)
        {
            Color newColor;

            newColor = img.color;
            newColor.a = alpha;
            img.color = newColor;
        }
    }
}
