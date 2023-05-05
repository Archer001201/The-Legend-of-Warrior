using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateBar : MonoBehaviour
{
    public Image healthImage;
    public Image healthDelayImage;
    public Image powerImage;

    private void Update() {
        if(healthDelayImage.fillAmount > healthImage.fillAmount){
            healthDelayImage.fillAmount -= Time.deltaTime*0.25f;
        }
    }

    public void OnHealthChange(float percentage){
        healthImage.fillAmount = percentage;
    }
}
