using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCommand : Command
{
    Vector2 _trans;

    public ObjectCommand(Vector3 m, float t)
    {
        _trans = m;
        _time = t;
    }
    
    public override void Execute()
    {
        
    }
}
