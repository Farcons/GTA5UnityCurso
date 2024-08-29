using UnityEngine;

public class PenduradoController : MonoBehaviour
{
    public GameObject rootP;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mao"))
        {
            //other.GetComponentInParent<PersonagemCod>().Pendurado(rootP.transform);
        }
    }
}