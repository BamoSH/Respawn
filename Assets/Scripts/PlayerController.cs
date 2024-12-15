using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerIndex
    {
        Player1 = 1,
        Player2 = 2
    }
    [Header("基本参数")]
    [Tooltip("")]
    public float speed = 5f;

    public float speedForce;
    public float maxSpeed = 10f;
    public float jumpSpeed = 10f;
    public float transferSpeed = 10f;
    // public float airDrag = 3f;
    public PlayerIndex playerIndex;

    [Header("初始重量体积控制")] 
    [SerializeField] private int weight = 1;
    [SerializeField] private int volumeLevel = 1;
    
    public ScaleManager anotherScaleManager;
    public PlayerController anotherPlayerController;
    public Transform anotherAnchorPos;
    public Transform anchorPos;
    public float journeyLength;
    public float fractionOfJourney;
    
    public GameObject prefab;
    
    private ScaleManager _scaleManager;
    private ScaleManager _anotherPlayerScaleManager;
    private PlayerAnimate _playerAnimate;
    
    private Rigidbody2D _rb2D;
    private float _playerHorizontalInput;
    private float _playerVerticalInput;
    
    private String _moveAxis;
    private String _jump;
    private String _transfer;
    private String _sit;
    
    private bool _isRight = true;
    [Header("物理检测参数")] public Vector2 bottomOffset;
    public float checkRadius;
    public float width;
    public float height;
    public LayerMask groundLayer;
    
    public bool isGround = true;
    public bool isLocked = false;
    public bool onTransfer = false;
    public bool anotherSit = false;
    public bool collide = false;
    private Vector3 lockedPosition;
    private Vector3 lockedScale;

    public event Action OnPlayerLock;
    public event Action OnPlayerUnLock;
    public event Action OnJumpPressed;
    public event Action OnTransferPressed;

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _scaleManager = GetComponent<ScaleManager>();
        _anotherPlayerScaleManager = anotherScaleManager;
        InitPlayerControl();
        // print(_anotherPlayerScaleManager.weight);
    }

    private void OnEnable()
    {
        _scaleManager.OnWeightChanged += _anotherPlayerScaleManager.AddWeight;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitScaleManager();
        _playerAnimate = GetComponent<PlayerAnimate>();
    }

    // Update is called once per frame
    void Update()
    {
        journeyLength = Vector3.Distance(anchorPos.transform.position, anotherAnchorPos.position);
        GroundCheck();
        // Move();
        Transfer();
        Move();
        Jump();
        Sit();
    }

    private void FixedUpdate()
    {

    }

    private void Move()
    {
        _playerHorizontalInput = Input.GetAxis(_moveAxis);
        if (Input.GetAxis(_moveAxis)!=0)
        {
            // if (collide && !isGround)
            // {
            //     _playerHorizontalInput = 0;
            // }
            if (anotherPlayerController.isLocked && !isGround)
            {
                // print("use addForce");
                anotherSit = true;
                _rb2D.AddForce(new Vector2(_playerHorizontalInput * speedForce, 0), ForceMode2D.Force);
                if (Math.Abs(_rb2D.velocity.x) > maxSpeed)
                {
                    _rb2D.velocity = new Vector2(Mathf.Sign(_rb2D.velocity.x) * speedForce, _rb2D.velocity.y);
                }
            }
            else
            {
                anotherSit = false;
                _rb2D.velocity = new Vector2(speed * _playerHorizontalInput, _rb2D.velocity.y);
            }
            
            // if (isGround)
            // {
            //     // print("use velocity");
            //     _rb2D.velocity = new Vector2(speed * _playerHorizontalInput, _rb2D.velocity.y);
            // }
            // else
            // {
            //     // // print("use addForce");
            //     // _rb2D.AddForce(new Vector2(_playerHorizontalInput * speedForce, 0), ForceMode2D.Force);
            //     // if (Math.Abs(_rb2D.velocity.x) > maxSpeed)
            //     // {
            //     //     _rb2D.velocity = new Vector2(Mathf.Sign(_rb2D.velocity.x) * speedForce, _rb2D.velocity.y);
            //     // }
            //
            // }
            
            // _rb2D.velocity = new Vector2(speed * _playerHorizontalInput, _rb2D.velocity.y);
        }
        if (_playerHorizontalInput > 0 && !_isRight || _playerHorizontalInput < 0 && _isRight)
        {
            Flip();
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown(_jump) && isGround)
        {
            OnJumpPressed?.Invoke();
            print(_jump);
            print("Jump!");
            _rb2D.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }
    }

    private void Flip()
    {
        _isRight = !_isRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    private void InitPlayerControl()
    {
        if (playerIndex == PlayerIndex.Player1)
        {
            _moveAxis = "Horizontal1";
            _jump = "Jump1";
            _transfer = "Transfer1";
            _sit = "Sit1";
        }
        else if (playerIndex == PlayerIndex.Player2)
        {
            _moveAxis = "Horizontal2";
            _jump = "Jump2";
            _transfer = "Transfer2";
            _sit = "Sit2";
        }
    }

    private void Transfer()
    {
        if (Input.GetButtonDown(_transfer) && onTransfer == false)
        {
            print("On transfer!");
            onTransfer = true;
            GameObject fat = null;
            _scaleManager.ChangeWeight(1);
            // OnTransferPressed?.Invoke();
            if (_scaleManager.transferOpen)
            {
                fat = Instantiate(prefab, anchorPos.transform.position, Quaternion.identity);
            }
            StartCoroutine(MoveToTarget(fat,anchorPos, anotherAnchorPos));
            // _scaleManager.TransferScale(1);
        }
    }

    private void Sit()
    {
        if (Input.GetButtonDown(_sit) && isGround)
        {
            isLocked = true;
            _rb2D.constraints = RigidbodyConstraints2D.FreezePosition;
            OnPlayerLock?.Invoke();
        }

        if (Input.GetButtonUp(_sit))
        {
            _rb2D.constraints = RigidbodyConstraints2D.None;
            _rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            isLocked = false;
            OnPlayerUnLock?.Invoke();
        }
        // if (Input.GetButtonDown(_sit) && isGround)
        // {
        //     print("Lock");
        //     isLocked = true;
        //     lockedPosition = transform.position;
        //     lockedScale = transform.localScale;
        //     OnPlayerLock?.Invoke();
        // }
        // if (Input.GetButton(_sit) && isGround)
        // {
        //     isLocked = true;
        //     print("Sit down");
        // }
        // if (Input.GetButtonUp(_sit))
        // {
        //     print("Stand up");
        //     isLocked = false;
        //     OnPlayerUnLock?.Invoke();
        // }
        // if (isLocked && isGround)
        // {
        //     transform.position = lockedPosition;
        //     transform.localScale = lockedScale;
        // }
        // else if (isLocked && !isGround)
        // {
        //     isLocked = false;
        //     OnPlayerUnLock?.Invoke();
        // }
    }
    
    
    IEnumerator MoveToTarget(GameObject obj, Transform startPoint, Transform endPoint)
    {
        float startTime = Time.time;
        float jouneyLength = Vector3.Distance(startPoint.position, endPoint.position);
        while (obj != null && Vector3.Distance(obj.transform.position, endPoint.position) > 0.01f)
        {
            // 每帧移动对象直到接近目标位置
            float distCovered = (Time.time - startTime) * transferSpeed;
            float fractionOfJourney = distCovered / jouneyLength;
            jouneyLength = Vector3.Distance(startPoint.position, endPoint.position);
            obj.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, fractionOfJourney);
            // obj.transform.position = Vector3.MoveTowards(obj.transform.position, target.position, speed * Time.deltaTime);
            yield return null;
        }
        _scaleManager.TransferScale(1);
        Destroy(obj);
        onTransfer = false;
    }

    public void GroundCheck()
    {
        // isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRadius * Math.Abs(transform.localScale.x), groundLayer);
        
        // LayerMask combinedLayer = groundLayer | playerLayer;
        Vector2 boxCenter = (Vector2)transform.position;
        Vector2 boxSize = new Vector2(width, height);
        Collider2D hit = Physics2D.OverlapBox(boxCenter, boxSize * Math.Abs(transform.localScale.x), 0, groundLayer);
        // 更新地面状态
        isGround = hit != null;  // 如果 hit 不为 null，则表示与地面或玩家发生了碰撞
    }
    private void InitScaleManager()
    {
        _scaleManager.weight = weight;
        _scaleManager.level = volumeLevel;
        _scaleManager.volume = transform.localScale.x;
        _scaleManager.CheckLevel();
    }

    private void EventDelegate()
    {
        _scaleManager.OnWeightChanged += _anotherPlayerScaleManager.AddWeight;
    }

    private void OnDrawGizmosSelected()
    {
        // Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRadius * Math.Abs(transform.localScale.x));
        Gizmos.color = Color.red;
        Vector2 boxCenter = (Vector2)transform.position + bottomOffset;
        Vector2 boxSize = new Vector2(width, height);        
        Gizmos.DrawWireCube(boxCenter, boxSize * Math.Abs(transform.localScale.x));
    }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     collide = true;
    // }
    //
    // private void OnCollisionExit2D(Collision2D other)
    // {
    //     collide = false;
    // }
}
