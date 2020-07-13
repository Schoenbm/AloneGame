using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransform : MonoBehaviour
{
    public Transform playerTransform;

    private bool blocked;

    void Update()
    {
        if(!blocked)
            this.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -10);
    }

    public void setBlocked(bool pBool)
    {
        blocked = pBool;
    }
}
