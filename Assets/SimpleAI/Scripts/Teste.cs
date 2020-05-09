using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Teste : MonoBehaviour
{


    public Rect lifeBarRect; public Rect lifeBarLabelRect; public Rect lifeBarBackgroundRect; public Texture2D lifeBarBackground; public Texture2D lifeBar;
    public GameObject PlayerData;


    private float LifeBarWidth = 300f;

    public float life = 100;

    void OnGUI()
    {


        lifeBarRect.width = LifeBarWidth * (life / 200);
        lifeBarRect.height = 20;
        lifeBarBackgroundRect.width = LifeBarWidth;
        lifeBarBackgroundRect.height = 20;
        GUI.DrawTexture(lifeBarBackgroundRect, lifeBarBackground);
        GUI.DrawTexture(lifeBarRect, lifeBar);
        GUI.Label(lifeBarLabelRect, "LIFE");

    }


}
