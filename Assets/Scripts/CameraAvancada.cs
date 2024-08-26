using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAvancada : MonoBehaviour
{
    public Vector3 cameraMoveVel = Vector3.zero;
    public GameObject segueOBJ;
    public float limiteAng = 65;
    public float inputSensit = 155.0f;
    public float mouseX, mouseY;
    public float rotY = 0, rotX = 0;
    public Vector3 rot;

    private Quaternion localRot;


    void Start()
    {
        Init();
    }

    void Update()
    {
        Atualizacao();
    }

    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, segueOBJ.transform.position, ref cameraMoveVel, 0.1f);
    }

    void Init()
    {
        rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    void Atualizacao()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        rotY += mouseY * inputSensit * Time.deltaTime;
        rotX += mouseX * inputSensit * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -limiteAng, limiteAng);

        localRot = Quaternion.Euler(rotX, rotY, 0);

        transform.rotation = localRot;
    }
}
