using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    public Image powerBarFill;
    public Image cooldownBarFill;

    private Image[] childrenImg;

    private void Awake()
    {
        childrenImg = GetComponentsInChildren<Image>();
    }

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
        Color newColor;

        foreach (Image img in childrenImg)
        {
            newColor = img.color;
            newColor.a = alpha;
            img.color = newColor;
        }
    }
}
