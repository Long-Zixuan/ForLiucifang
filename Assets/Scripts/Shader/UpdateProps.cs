// jave.lin 2019.08.15
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UpdateProps : MonoBehaviour
{
    private static int nHash;
    private static int pHash;
    
    public GameObject mirrorPlane;

    public Material[] objMats;

    // Start is called before the first frame update
    void Start()
    {
        //objMat = obj.GetComponent<MeshRenderer>().sharedMaterial;
        nHash = Shader.PropertyToID("n");
        pHash = Shader.PropertyToID("p");
    }

    // Update is called once per frame
    void Update()
    {
        // n==normal of plane
        // p==position of plane
        var n = mirrorPlane.transform.up;
        var p = mirrorPlane.transform.position;
        foreach (var objMat in objMats)
        {
            objMat.SetVector(nHash, n);
            objMat.SetVector(pHash, p);
        }
    }
}

