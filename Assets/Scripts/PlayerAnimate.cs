using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayerAnimate : MonoBehaviour
{
    public GameObject deathPage;
    private AudioManager _audioManager;
    public GameObject audioMobj;
    private PlayerController _playerController;
    private Animator _animator;
    private Rigidbody2D _rb2D;
    private ScaleManager _scaleManager;

    private bool bloodGound = false;
    
    
    private static readonly int IsGround = Animator.StringToHash("isGround");
    private static readonly int HighestPoint = Animator.StringToHash("highestPoint");
    private static readonly int Jump = Animator.StringToHash("jump");
    private static readonly int Sit = Animator.StringToHash("sit");

    private void Awake()
    {
        deathPage.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
        _rb2D = GetComponent<Rigidbody2D>();
        _scaleManager = GetComponent<ScaleManager>();

        _audioManager = audioMobj.GetComponent<AudioManager>();
            
        _playerController.OnJumpPressed += TriggerJumpAnimation;
        _scaleManager.OnScaleSmall += TriggerSmallAnimation;
        _scaleManager.OnBoom += TriggerBoomAnimation;
        _playerController.OnPlayerLock += TriggerSitAnimation;
        _playerController.OnPlayerUnLock += ResetTriggerSitAnimation;
        _playerController.OnTransferPressed += TriggerSmallAnimation;
    }

    private void OnDestroy()
    {
        if (_playerController != null)
        {
            _playerController.OnJumpPressed -= TriggerJumpAnimation;
            _scaleManager.OnScaleSmall -= TriggerSmallAnimation;
            _scaleManager.OnBoom -= TriggerBoomAnimation;
            _playerController.OnPlayerLock -= TriggerSitAnimation;
            _playerController.OnPlayerUnLock -= ResetTriggerSitAnimation;
            // _playerController.OnTransferPressed -= TriggerSmallAnimation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        CheckHighestPoint();
        CheckSpeed();
        CheckHP();
    }

    private void GroundCheck()
    {
        _animator.SetBool(IsGround, _playerController.isGround);
    }

    public void OnGroundSound()
    {
        _audioManager.playerGround.Play();
    }

    public void MoveSound()
    {
        if (bloodGound)
        {
            _audioManager.moveOnBlood.Play();
        }
        else
        {
            _audioManager.moveOnFat.Play();
        }
    }
    private void CheckSpeed()
    {
        _animator.SetFloat("speed", Math.Abs(_rb2D.velocity.x));
    }

    private void CheckHighestPoint()
    {
        if (_rb2D.velocity.y < 0 && _rb2D.velocity.y > -0.5)
        {
            // print("Highest point: " + _rb2D.velocity.y);
            _animator.SetTrigger(HighestPoint);
        }
        else
        {
            _animator.ResetTrigger(HighestPoint);
        }
    }

    private void TriggerJumpAnimation()
    {
        print("trigger jump set!");
        _animator.SetTrigger(Jump);
        _audioManager.playerJump.Play();
    }


    private void TriggerSmallAnimation()
    {
        // _animator.SetBool("transfer",_scaleManager.difLevel);
        _animator.SetTrigger("transfer");
        print("Transfer Small");
        _audioManager.playerTransfer.Play();
    }

    private void TriggerBigAnimation()
    {
        _animator.ResetTrigger("transfer");
        print("Transfer Big");
    }

    private void TriggerSitAnimation()
    {
        _audioManager.playerSit.Play();
        _animator.SetBool(Sit,true);
    }
    
    private void ResetTriggerSitAnimation()
    {
        _animator.SetBool(Sit, false);
    }

    private void CheckHP()
    {
        _animator.SetFloat("hp", _rb2D.velocity.y);
    }

    private void TriggerBoomAnimation()
    {
        _audioManager.playerBoom.Play();
        _animator.SetTrigger("boom");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        print("trigger");
        if (other.CompareTag("Fan"))
        {
            print("fan trigger");
            _animator.SetTrigger("fan");
            _animator.ResetTrigger("outFan");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        print("Out!");
        if (other.CompareTag("Fan"))
        {
            _animator.ResetTrigger("fan");
            _animator.SetTrigger("outFan");
        }
    }

    public void DeathPageAppear()
    {       
        Debug.Log("Death Screen Activated!");
        deathPage.SetActive(true);
        Time.timeScale = 0;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Fat"))
        {
            print("move On Fat!");
            bloodGound = false;
        }

        if (other.collider.CompareTag("Blood"))
        {
            bloodGound = true;
        }
    }
    
}
