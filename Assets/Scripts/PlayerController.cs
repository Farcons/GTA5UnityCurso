using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float vel = 6, velRot = 150;

    [SerializeField]
    private Animator heroiAnim;

    [SerializeField]
    private Transform alvo;

    [SerializeField]
    private bool morto = false;
    
    private bool estaPendurado = false;
    private Transform rootAlvo;
    public Transform parede;
    public bool gatilho;
    public Transform mao, obj;

    public static float movX, movY;

    void Start()
    {
        rootAlvo = null;
        gatilho = false;
    }

    void AjustaRotacao()
    {
        if (Vector3.Distance(transform.position, parede.position) <= 3.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, parede.rotation, 1);
        }
    }

    void FixedUpdate()
    {
        if (!rootAlvo)
            return;

        if (estaPendurado && gatilho)
        {
            transform.position = new Vector3(transform.position.x, rootAlvo.position.y, rootAlvo.position.z);
            gatilho = false;
        }
    }

    void Update()
    {
        movY = Input.GetAxis("Vertical");
        movX = Input.GetAxis("Horizontal");
        heroiAnim.SetFloat("Y", movY, 0.1f,Time.deltaTime);
        heroiAnim.SetFloat("X", movX, 0.1f, Time.deltaTime);

        if (estaPendurado)
        {
            if (movX >= 1)
            {
                heroiAnim.SetBool("PenduradoDir", true);
            }
            else if (movX <= -1)
            {
                heroiAnim.SetBool("PenduradoEsq", true);
            }
            else
            {
                heroiAnim.SetBool("PenduradoDir", false);
                heroiAnim.SetBool("PenduradoEsq", false);
            }
        }

        if (morto && Input.GetKeyDown(KeyCode.Z))
            heroiAnim.SetTrigger("Levantar");

        if (morto && Input.GetKeyUp(KeyCode.Z))
        {
            morto = false;

            heroiAnim.ResetTrigger("Levantar");
            heroiAnim.ResetTrigger("morte");
        }

        if (!morto && Input.GetKeyDown(KeyCode.Space))
        {
            if (estaPendurado)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                estaPendurado = false;
                heroiAnim.SetTrigger("Descendo");
                heroiAnim.ResetTrigger("Pendurado");
                rootAlvo = null;
                return;
            }

            heroiAnim.SetTrigger("Pulo");
        }

        if (!morto && Input.GetKeyUp(KeyCode.Space))
        {
            heroiAnim.ResetTrigger("Pulo");
        }

        if (estaPendurado && Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(Subindo());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            heroiAnim.SetTrigger("morte");
            morto = true;
        }
    }

    public void Pendurado(Transform alv)
    {
        if (estaPendurado)
        {
            return;
        }

        heroiAnim.SetTrigger("Pendurado");
        GetComponent<Rigidbody>().isKinematic = true;
        estaPendurado = true;
        //rootAlvo = alv;
    }

    IEnumerator Subindo()
    {
        heroiAnim.SetTrigger("Subindo");
        heroiAnim.ResetTrigger("Pendurado");

        yield return new WaitForSeconds(3.24f);
        estaPendurado = false;
        gatilho = true;
        GetComponent<Rigidbody>().isKinematic = false;
        rootAlvo = null;
    }
    

    //IK

    private void OnAnimatorIK(int layerIndex)
    {
        heroiAnim.SetLookAtWeight(heroiAnim.GetFloat("Ik_val"));
        heroiAnim.SetLookAtPosition(alvo.position);

        if (heroiAnim.GetFloat("Ik_val") > 0.9f)
        {
            obj.parent = mao;
            obj.localPosition = new Vector3(-1.499938f, 1.010078f, -0.7899401f);
        }

        heroiAnim.SetIKPositionWeight(AvatarIKGoal.RightHand, heroiAnim.GetFloat("Ik_val"));
        heroiAnim.SetIKPosition(AvatarIKGoal.RightHand, alvo.position);
    }

}
