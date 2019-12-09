using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public GameObject target;
    public Vector2 verticalOffset;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
        {
            transform.position = new Vector3(
                    0,
                    target.transform.position.y + verticalOffset.y,
                    transform.position.z
                );
        }
    }
}
