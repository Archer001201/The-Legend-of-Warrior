using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    public string a;
    [SerializeField] private int num;
    void Start()
    {
        Debug.Log(a);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
