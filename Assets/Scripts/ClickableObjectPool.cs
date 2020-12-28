using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObjectPool : MonoBehaviour
{
    ClickableObject[] clickables;

    public static ClickableObjectPool Instance { get; private set; }
    Queue<ClickableObject> objects = new Queue<ClickableObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void SetPool(ClickableObject[] clickables, int maxSize)
    {
        for (int i = 0; i < maxSize; i++)
        {
            var newObject = Instantiate(clickables[Random.Range(0, clickables.Length)]);
            newObject.SetActive(false);
            objects.Enqueue(newObject);
        }
    }

    public ClickableObject Get()
    {
        if (objects.Count == 0)
            return null;
        return objects.Dequeue();
    }

    public void ReturnToPool(ClickableObject objectsToReturn)
    {
        objectsToReturn.SetActive(false);
        objects.Enqueue(objectsToReturn);
    }
}
