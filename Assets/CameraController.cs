using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensX;
    public float sensY; 
    public float speed;

    float xRotation;
    float yRotation;


    void Movement() {
        if (Input.GetKey("w"))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if (Input.GetKey("s")) {
            transform.Translate(-Vector3.forward * speed * Time.deltaTime);
        }
        if (Input.GetKey("d")) 
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        if (Input.GetKey("a"))
        {
            transform.Translate(-Vector3.right * speed * Time.deltaTime);
        }
        if (Input.GetKey("space"))
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(-Vector3.up * speed * Time.deltaTime, Space.World);
        }
        

    }    

    void Rotation()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensX;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    void CameraReset()
    {
        if (Input.GetKeyDown("f"))
        {
            transform.position = new Vector3(0, 0, -10);
            xRotation = 0f;
            yRotation = 0f;
        }
    }


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        sensX = 500.0f;
        sensY = 500.0f;
        speed = 10.0f;

        transform.position = new Vector3(0, 0, -10);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Movement();
        Rotation();
        CameraReset();
    }
}
