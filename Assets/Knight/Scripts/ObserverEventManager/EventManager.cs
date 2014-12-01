using UnityEngine;
using System.Collections;
using System;


public class CustomArg : EventArgs
{
    private int myNum = 0;
    public CustomArg(int num)
    {
        this.myNum = num;
    }
    public int GetNumber()
    {
        return this.myNum;
    }
}
public class EventManager : MonoBehaviour
{
    public delegate void mChangeWaveDelegate(EventManager EM, CustomArg e);
    public event mChangeWaveDelegate mChangeWaveEvent;
    public void ChangeLevelEvent(int n)
    {
        CustomArg _myArg = new CustomArg(n);
        if (mChangeWaveEvent != null) mChangeWaveEvent(this, _myArg);
    }
}