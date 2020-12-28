using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    public Rigidbody rbgc;
    Vector3 launchPoint = new Vector3(0, 0);

    void Start()
    {
        rbgc = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        TriggerUpdate();
        PlayFX(transform.position);
        rbgc.velocity = Vector3.zero;
        rbgc.angularVelocity = Vector3.zero;
        ClickableObjectPool.Instance.ReturnToPool(this);
    }

    void OnBecameInvisible()
    {
        rbgc.velocity = Vector3.zero;
        rbgc.angularVelocity = Vector3.zero;
        ClickableObjectPool.Instance.ReturnToPool(this);
    }

    public void Launch(float x, float y, float z, ForceMode mode)
    {
        rbgc.AddForceAtPosition(new Vector3(x, y, z), launchPoint, mode);
    }

    public void SetStartPosition(float x, float y, float z)
    {
        transform.position = new Vector3(x, y, z);
    }

    public void disableGravity()
    {
        rbgc.useGravity = false;
    }

    public void enableGravity()
    {
        rbgc.useGravity = true;
    }

    public void SetActive(bool setting)
    {
        gameObject.SetActive(setting);
    }

    public virtual void TriggerUpdate()
    {

    }

    public virtual void PlayFX(Vector3 position)
    {

    }
}
