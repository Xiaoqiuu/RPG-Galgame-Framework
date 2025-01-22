using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Please hold this script on the object 
/// </summary>
public class ObjectEntity : MonoBehaviour
{
    private Transform _transform;

    private void Start()
    {
        _transform = transform;
    }

    public void move(Vector2 T)
    {
        _transform.Translate(T);
    }
}
