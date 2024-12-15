using System;
using UnityEngine;

public class PlayerDistance : MonoBehaviour
{
    public GameObject center;
    public float maxDistance;
    public float triggerDistance;
    public float timer;
    public float triggerTimer;
    public Transform anotherPlayerPos;
    public PlayerController playerController1;
    public PlayerController playerController2;
    public float distance;
    public bool warning = false;
    
    private DistanceJoint2D _distanceJoint;

    private PlayerAnimate _playerAnimate;
    // private SpringJoint2D _springJoint2D;
    
    private void Start()
    {
        _distanceJoint = GetComponent<DistanceJoint2D>();
        _playerAnimate = GetComponent<PlayerAnimate>();
        // _springJoint2D = GetComponent<SpringJoint2D>();
        if (_distanceJoint == null)
        {
            Debug.LogError("no distance component");
            _distanceJoint = gameObject.AddComponent<DistanceJoint2D>();
        }
        // else if (_springJoint2D)
        // {
        //     
        // }

        maxDistance = _distanceJoint.distance;
        playerController1.OnPlayerLock += CalculateDistance;
        playerController1.OnPlayerLock += ChangeDistance;
        playerController1.OnPlayerUnLock += ChangeMaxDistance;
        playerController2.OnPlayerLock += CalculateDistance;
        playerController2.OnPlayerLock += ChangeDistance;
        playerController2.OnPlayerUnLock += ChangeMaxDistance;
    }

    private void Update()
    {
        CalculateDistance();
        if (distance >= triggerDistance)
        {
            warning = true;
            timer += Time.deltaTime;
            if (timer >= triggerTimer)
            {
                _playerAnimate.DeathPageAppear();
                timer = 0;
            }
        }
        else
        {
            warning = false;
            timer = 0;
        }

        UpdateCenterPosition();
    }

    private void OnDestroy()
    {
        playerController1.OnPlayerLock -= CalculateDistance;
        playerController1.OnPlayerUnLock -= ChangeDistance;
        playerController1.OnPlayerUnLock -= ChangeMaxDistance;
        playerController2.OnPlayerLock -= CalculateDistance;
        playerController2.OnPlayerUnLock -= ChangeDistance;
        playerController2.OnPlayerUnLock -= ChangeMaxDistance;
    }

    public void CalculateDistance()
    {
        distance = Math.Abs(Vector2.Distance(transform.position, anotherPlayerPos.position));
    }

    public void ChangeDistance()
    {
        _distanceJoint.distance = distance;
        // _springJoint2D.distance = distance;
    }

    public void ChangeMaxDistance()
    {
        print("max distance init!");
        _distanceJoint.distance = maxDistance;
        // _springJoint2D.distance = maxDistance;
    }
    
    private void UpdateCenterPosition()
    {
        if (playerController1 != null && playerController2 != null && center != null)
        {
            Vector3 centerPosition = (playerController1.transform.position + playerController2.transform.position) / 2;
            center.transform.position = centerPosition;
        }
    }
}
