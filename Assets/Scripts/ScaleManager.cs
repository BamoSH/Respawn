using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;


public class ScaleManager : MonoBehaviour
{
    [Header("重量和体积等级")]
    public int weight;
    public int level; 
    public float volume;
    [Header("放大倍数")] public float[] volumes;
    [Header("放大质量")] public float[] jumpSpeeds;
    [Header("速度大小")] public float[] speeds;
    [Header("空中速度大小")] public float[] speedForces;
    
    [Header("单位体积")]
    public float unitVolume;

    [Header("是否传递")] public bool transferOpen;
    public bool difLevel;
    
    private PlayerController _playerController;
    private Rigidbody2D _rigidbody2D;
    private Animator anim;
    
    public delegate void WeightChangeControl(int unit);
    
    public event WeightChangeControl OnWeightChanged;
    public event Action OnScaleBig;
    public event Action OnScaleSmall;
    public event Action OnBoom;
    public delegate void VolumeChangeControl(int level);

    private Vector3 currentScale;
    private Vector3 targetScale;
    
    private void Awake()
    {
        unitVolume = transform.localScale.x;
        print("unitVolume" + unitVolume);
        _playerController = GetComponent<PlayerController>();
        print("init playerController!");
        _rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    
    public void ChangeWeight(int unit)
    {
        weight -= unit; 
        CheckLevel();
    }

    public void AddWeight(int unit)
    {
        print("Add weight");
        weight += unit;
        CheckLevel();
    }

    private void FixedUpdate()
    {
        transform.localScale = Vector3.Lerp(currentScale, targetScale, 3f);
    }
    public void ChangeState()
    {
        // change scale
        print("Level " + level);
        
        currentScale = transform.localScale = new Vector3(volume, volume, volume);
        volume = unitVolume * volumes[level - 1];
        
        targetScale = new Vector3(volume, volume, volume);
            
        // transform.localScale = new Vector3(volume, volume, volume);
        
        // change jumpSpeed
        print("jumpSpeed" + _playerController.jumpSpeed);
        _playerController.jumpSpeed = jumpSpeeds[level - 1];
        // change speed on ground;
        _playerController.speed = speeds[level - 1];
        // change speed force
        _playerController.speedForce= speedForces[level - 1];
        OnScaleBig?.Invoke();
    }

    public void TransferScale(int unit)
    {
        if (transferOpen)
        {
            // ChangeWeight(unit);
            print("Transfer open");
            OnWeightChanged?.Invoke(unit);
        }
    }



    private void OnEnable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckLevel()
    {
        if (weight == 0)
        {
            weight = 1;
            // todo: play an animate
            print("Animate!");
            transferOpen = false;
            OnScaleSmall?.Invoke();
        }
        else if (weight == 1)
        {        
            if (level != 1)
            {
                difLevel = true;
                level = 1;
                OnScaleSmall?.Invoke();
            }
            else
            {
                difLevel = false;
            }
            // CheckDifLevel(level,1);
            print("weight is 1");
            ChangeState();
        }
        else if (weight >= 2 && weight <5)
        {
            // CheckDifLevel(level,2);
            if (level != 2)
            {
                difLevel = true;
                level = 2;
                OnScaleSmall?.Invoke();
            }
            else
            {
                difLevel = false;
            }
            transferOpen = true;
            ChangeState();
        }
        else if (weight >= 5 && weight < 8)
        {
            // CheckDifLevel(level,3);
            if (level != 3)
            {
                difLevel = true;
                level = 3;
                OnScaleSmall?.Invoke();
            }
            else
            {
                difLevel = false;
            }
            transferOpen = true;
            ChangeState();
        }
        else if (weight >= 8)
        {
            // todo: death page
            print("BOOM!");
            transferOpen = false;
            OnBoom?.Invoke();
        }
    }

    private void CheckDifLevel(int temLevel, int futureLevel)
    {
        if (temLevel != futureLevel)
        {
            difLevel = true;
            temLevel = futureLevel;
            print(temLevel);
        }
        else
        {
            difLevel = false;
        }
    }
}
