using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FixCamera : NetworkBehaviour
{

    public float cameraFollowSpeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            // exit from update if this is not the local player
            return;
        }

        Camera.main.transform.position = transform.position - transform.forward * 10; // + transform.up * 3;
        Camera.main.transform.LookAt(transform.position);
        //Camera.main.transform.parent = transform;
    }

    private void Update()
    {
        float interpolation = cameraFollowSpeed * Time.deltaTime;

        Vector3 position = Camera.main.transform.position;
        position.y = Mathf.Lerp(Camera.main.transform.position.y, transform.position.y, interpolation);
        position.x = Mathf.Lerp(Camera.main.transform.position.x, transform.position.x, interpolation);

        Camera.main.transform.position = position;
    }
}
