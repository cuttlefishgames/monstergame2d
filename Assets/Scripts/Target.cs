using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public void Update()
    {
        var camToWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var pos = new Vector3(camToWorldPos.x, camToWorldPos.y, 0);
        transform.position = pos;
    }
}
