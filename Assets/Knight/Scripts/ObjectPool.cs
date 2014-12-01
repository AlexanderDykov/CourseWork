using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Knight.Interfaces;

public class ObjectPool : MonoBehaviour, IObjectPool {

    private static ObjectPool _instance;
    public static ObjectPool Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ObjectPool>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != _instance)
                Destroy(this.gameObject);
        }
    }

    public List<GameObject> Create(GameObject obj, int count)
    {
        List<GameObject> lst = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            GameObject newObj = Instantiate(obj) as GameObject;
            newObj.SetActive(false);
            lst.Add(newObj);
        }
        return lst;
    }

    public void Spawn(GameObject obj, Vector3 position, Quaternion rotation)
    {
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
    }

    public void Remove(GameObject obj)
    {
        obj.SetActive(false);
    }
}
