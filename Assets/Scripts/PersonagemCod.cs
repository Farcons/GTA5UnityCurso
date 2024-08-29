using Cinemachine;
using System.Linq;
using UnityEngine;

public class PersonagemCod : MonoBehaviour
{
    public float inputX, inputZ;
    public Vector3 dirMoveDesejada;
    public float velRotDesejada = 0.1f;
    public Animator anim;
    public float speed;
    public float permiteRotPlayer = 0.3f;
    public Camera cam;
    public float verticalVel;
    public Vector3 movVector;

    public CinemachineVirtualCamera vcam;
    public float[] posCam;
    public int id;
    public CinemachineFramingTransposer composer;

    void Start()
    {
        anim = GetComponent<Animator>();
        cam = Camera.main;

        //composer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        //composer.m_CameraDistance = posCam[id];
    }

    void Update()
    {
        //InputMagnitude();

        if (Input.GetButton("CameraAjust"))
        {
            if (Input.GetButtonDown("CameraAjust"))
                if (id == posCam.Count() - 1)
                    id = 0;
                else
                    id++;

            composer.m_CameraDistance = posCam[id];
        }
    }

    private void PlayerMoveRot()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        Vector3 frente = cam.transform.forward;
        Vector3 direita = cam.transform.right;

        frente.Normalize();
        direita.Normalize();

        dirMoveDesejada = frente * inputZ + direita * inputX;
        Quaternion rot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirMoveDesejada), velRotDesejada);

        transform.rotation = new Quaternion(0, rot.y, 0, rot.w);
    }

    private void InputMagnitude()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        anim.SetFloat("Z", inputZ, 0, Time.deltaTime * 2);
        anim.SetFloat("X", inputX, 0, Time.deltaTime * 2);

        speed = new Vector2(inputX, inputZ).sqrMagnitude;

        if (speed > permiteRotPlayer)
        {
            anim.SetFloat("InputMagnitude", speed, 0.1f, Time.deltaTime);
            //PlayerMoveRot();
        }
        else if (speed < permiteRotPlayer)
        {
            anim.SetFloat("InputMagnitude", speed, 0.1f, Time.deltaTime);
        }
    }
}
