using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    public Image powerBarFill;
    public Image cooldownBarFill;

    public void SetPower(float power)
    {
        powerBarFill.fillAmount = power;
    }
    
    public void SetCooldown(float cooldown)
    {
        cooldownBarFill.fillAmount = cooldown;
    }
}
