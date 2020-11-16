using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMove : MonoBehaviour
{
    Transform transform;
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(gameObject.GetComponentInChildren<Rigidbody2D>().position);
    }
}
