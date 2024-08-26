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
    public bool gatilho;


    void Start()
    {
        rootAlvo = null;
        gatilho = false;
    }


    void FixedUpdate()
    {
        if (!rootAlvo)
            return;

        if (estaPendurado && gatilho)
        {
            transform.position = rootAlvo.position;
            gatilho = false;
        }

        transform.position = rootAlvo.position;
    }

    void Update()
    {
        float move = Input.GetAxis("Vertical") * vel;
        float rotacao = Input.GetAxis("Horizontal");

        move *= Time.deltaTime;

        if (!morto && !estaPendurado)
            transform.Rotate(0, rotacao, 0);

        if (estaPendurado)
        {
            if (rotacao >= 1)
            {
                heroiAnim.SetBool("PenduradoDir", true);
            }
            else if (rotacao <= -1)
            {
                heroiAnim.SetBool("PenduradoEsq", true);
            }
            else
            {
                heroiAnim.SetBool("PenduradoDir", false);
                heroiAnim.SetBool("PenduradoEsq", false);
            }
        }

        if (move != 0)
            heroiAnim.SetBool("Andar", true);
        else
            heroiAnim.SetBool("Andar", false);

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

    

    //IK

    private void OnAnimatorIK(int layerIndex)
    {
        heroiAnim.SetLookAtWeight(1);
        heroiAnim.SetLookAtPosition(alvo.position);
    }

}
