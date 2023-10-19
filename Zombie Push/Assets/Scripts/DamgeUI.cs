using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamgeUI : MonoBehaviour
{
    bool isDamge=false;
    private void Start()
    {
        StartCoroutine("BindDelegate");
    }

    void Update()
    {
        if (isDamge)
        {
            this.GetComponent<Image>().color = new Color(0.5f,0f,0f);
        }
        else
        {
            this.GetComponent<Image>().color = Color.Lerp(this.GetComponent<Image>().color, Color.clear, Time.deltaTime * 5);
        }

        isDamge = false;
    }

    IEnumerator BindDelegate()
    {
        while (World.GameWorld == null)
        {
            Debug.Log("UI绑定");
            yield return null;
        }
        World.GameWorld.GetComponent<DelegateManager>().OnPlayerBeDamge += PlayerDamgeUI;
        Debug.Log("UI委托绑定完成");
    }
    private void PlayerDamgeUI(int CurHP) 
    {
        isDamge = true;
    }
}
