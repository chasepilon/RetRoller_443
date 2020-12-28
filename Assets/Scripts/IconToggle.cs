using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconToggle : MonoBehaviour
{
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void ToggleIcon(bool setting)
    {
        if(setting)
        {
            image.color = new Color(251f, 84f, 182f, 255f);
        }
        else
        {
            image.color = new Color(255f, 255f, 255f, 70f);
        }
    }
}
