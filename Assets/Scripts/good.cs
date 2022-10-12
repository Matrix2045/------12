using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class good : MonoBehaviour
{
    public delegate void PostGood();

    public PostGood go;
    // Start is called before the first frame update
    void Start()
    {

    }
    void OnMouseDown()
    {
        if (go != null)
            go();

    }
    // Update is called once per frame
    void Update()
    {

    }
}
