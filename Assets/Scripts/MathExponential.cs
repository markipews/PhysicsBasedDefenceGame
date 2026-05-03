using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MathExponential : MonoBehaviour
{
    [FormerlySerializedAs("increase")] public float increaseExp;
    [FormerlySerializedAs("decrease")] public float decreaseExp;

    public float fallOff = 0.5f;

    public float onCallNumber;
    public bool increaseOnCallNumber;

    private Vector2 cursorPosRelative;
    
    void Update()
    {
        cursorPosRelative = MouseHandler.Instance.mousePosFinal -  (Vector2) transform.position;
        
        increaseExp = Mathf.Exp(fallOff * cursorPosRelative.magnitude);
        decreaseExp = Mathf.Exp(-fallOff * cursorPosRelative.magnitude);
        //Debug.Log("Decrease: " + decrease + " Increase:" + increase);
        
        if (increaseOnCallNumber)
        {
            onCallNumber *= 1.01f;
            return;
        }
        onCallNumber = 1;


    }

    /*public float OnCallIncrease(int id)
    {
            onCallNumber[id] *= 1.01f;
            increaseOnCallNumber = false;
            return;
        onCallNumber[id] = 1;
        return onCallNumber[id];
    }*/
}
