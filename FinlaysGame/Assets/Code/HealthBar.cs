using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Player Player;
    public Transform ForegroundSprite;
    public SpriteRenderer ForegroundRenderer; // required at least here so the color can be changed 
    public Color MaxHealthColor = new Color(255 / 255f, 63 / 255f, 63 /255f); // unity doesnt directly use the typical rgb values from 0-255. it uses a percentage 0%-100% where 0=0% and 255=100% hence the divide
    public Color MinHealthColor = new Color(64 / 255f, 137 / 255f, 255 / 255f);

    public void Update()
    {
        var healthPercent = Player.Health / (float)Player.MaxHealth;

        ForegroundSprite.localScale = new Vector3(healthPercent, 1, 1);
        ForegroundRenderer.color = Color.Lerp(MaxHealthColor, MinHealthColor, healthPercent );
    }

}

