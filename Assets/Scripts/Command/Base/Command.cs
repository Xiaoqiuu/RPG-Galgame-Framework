using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    protected float _time;

    public float time => _time;
    
    public virtual void Execute()
    {
        Debug.Log("Execute Command");
    }

    public virtual void Undo()
    {
        Debug.Log("Undo Command");
    }
    
    public virtual void Redo()
    {
        Debug.Log("Redo Command");
    }

}
