using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CamSegue : MonoBehaviour
{
    public Transform cabeca;
    public Transform[] pos;
    public int id;
    public Vector3 vel = Vector3.zero;
    private RaycastHit hit;
    private float rotVel, rotacao;
    public Transform player;

    void Start()
    {
        rotVel = 100;
        id = 0;
    }

    private void Update()
    {
        AjusteCamera();

        if (Mathf.Abs(PlayerController.movY) == 0)
        {
            RotacaoCam(cabeca);
        }
        else
        {
            RotacaoCam(player);

        }
    }

    void LateUpdate()
    {
        transform.LookAt(cabeca);
        if (!Physics.Linecast(cabeca.position, pos[id].position))
        {
            transform.position = Vector3.SmoothDamp(transform.position, pos[id].position, ref vel, 0.4f);
            Debug.DrawLine(cabeca.position, pos[id].position);
        }
        else if (Physics.Linecast(cabeca.position, pos[id].position, out hit))
        {
            transform.position = Vector3.SmoothDamp(transform.position, hit.point, ref vel, 0.4f);

        }
    }

    private void AjusteCamera()
    {
        if (Input.GetButtonDown("CameraAjust"))
            if (id == pos.Count() - 1)
            {
                id = 0;
            }
            else
            {
                id++;
            }
    }

    void RotacaoCam(Transform obj)
    {
        rotacao = Input.GetAxis("CameraRot") * rotVel;
        rotacao *= Time.deltaTime;

        obj.Rotate(0, rotacao, 0);
    }
}
