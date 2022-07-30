using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public Material objectOut;
    public Material objectIn;
    
    void OnTriggerEnter(Collider col)
    {
        print("Entrou");
        this.transform.parent.gameObject.GetComponent<MeshRenderer>().material = objectIn;
    }

    void OnTriggerExit(Collider col)
    {
        print("Saiu");
        this.transform.parent.gameObject.GetComponent<MeshRenderer>().material = objectOut;
    }
}
