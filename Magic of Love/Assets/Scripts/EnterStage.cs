﻿using UnityEngine;
using System.Collections;

public class EnterStage : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<WorldManager>().Enter(int.Parse(collision.name));
    }
}
