using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{

    bool isRewinding = false;
    float countDown = 0.0f;
    float recordTime = 1.5f;

    List<PointInTime> pointsInTime;

    Rigidbody rb;

    void Start()
    {
        pointsInTime = new List<PointInTime>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            if (GameManager.Instance.isRewindAbilityReady())
                StartRewind();
    }

    void ToggleRewind()
    {
        if (isRewinding)
            StopRewind();
        else
            StartRewind();
    }

    void FixedUpdate()
    {
        if (isRewinding)
        {
            Rewind();
            countDown = countDown - Time.fixedDeltaTime;
        }
        else if (countDown <= 0.0f && isRewinding)
            StopRewind();
        else
            Record();
    }

    void Rewind()
    {
        if (pointsInTime.Count > 0)
        {
            PointInTime pointInTime = pointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            pointsInTime.RemoveAt(0);
        }
        else
        {
            StopRewind();
        }

    }

    void Record()
    {
        if (pointsInTime.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }

        pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
    }

    public void StartRewind()
    {
        isRewinding = true;
        rb.isKinematic = true;
        Time.timeScale = 0.5f;
        countDown = recordTime;
    }

    public void StopRewind()
    {
        isRewinding = false;
        rb.isKinematic = false;
        Time.timeScale = 1.0f;
        GameManager.Instance.ResetRewindAbility();
    }

    public bool HasTriggeredRewinding()
    {
        return isRewinding;
    }
}