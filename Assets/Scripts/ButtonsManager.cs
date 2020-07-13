using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonsManager : MonoBehaviour
{
    public GameObject leftButton;
    public GameObject rightButton;

    void goLeft()
    {
        if(leftButton != null)
            EventSystem.current.SetSelectedGameObject(leftButton);
    }

    void goRight()
    {
        if (leftButton != null)
            EventSystem.current.SetSelectedGameObject(rightButton);
    }
}
