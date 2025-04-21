using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    // requirements
    private CharacterController _characterController;

    [Header("UI")]
    [SerializeField] private Slider _staminaSlider;
    [SerializeField] private Image staminaFillImage;
    private Color targetColor;

    [Header("Player Settings")]
    [SerializeField] private float _playerMoveSpeed = 7f;
    [SerializeField] private float _playerSprintSpeed = 12f;
    [SerializeField] private float _jumpPower = 3f;

    [Header("Stamina Settings")]
    public float MaxStamina = 100f;
    public float StaminaDecreaseRate = 20f;
    public float StaminaRecoveryRate = 30f;
    [SerializeField] private float _currentStamina;
    [SerializeField] private float _staminaRecoveryDelay = 2f;
    private float _recoveryTimer = 0f;
    private bool _canRecoverStamina = true;

    [Header("Double Jump Settings")]
    [SerializeField] private int _maxJumpCount = 2;
    private int currentJumpCount = 0;

    [Header("Wall Ride Settings")]
    [SerializeField] private float _wallRideVelocity = 2f;
    private bool _isWallRiding = false;

    [Header("Slide Settings")]
    [SerializeField] private float _slideDashForce = 20f;
    [SerializeField] private float _slideDuration = 0.3f;
    [SerializeField] private float _slideCooldown = 1f;
    [SerializeField] private float _slideStaminaCost = 30f;
    private bool _isSliding = false;
    private float _slideTimer = 0f;
    private float _slideCooldownTimer = 0f;
    private Vector3 _slideDirection;

    private const float GRAVITY = -9.8f;
    private float _gVelocity = 0f;
    private bool _isJumping = false;
    private bool _isWallContacted = false;
    private bool _isSprinting = false;

    private void Awake()
    {
        if(_characterController == null) _characterController = GetComponent<CharacterController>();

        _currentStamina = MaxStamina;
    }

    private void Start()
    {
        if (_staminaSlider != null)
        {
            _staminaSlider.maxValue = MaxStamina;
            _staminaSlider.value = _currentStamina;
        }
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float velocity = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, velocity);
        direction = direction.normalized;
        direction = Camera.main.transform.TransformDirection(direction);
        
        GroundCheck();
        WallRideCheck();

        SprintFunction();
        SlideFunction(direction);
        JumpFuction();
        WallRideFunction();
        
        _gVelocity += GRAVITY * Time.deltaTime;
        direction.y = _gVelocity;

        float currentSpeed = _isSprinting ? _playerSprintSpeed : _playerMoveSpeed; // 삼항 연산자 너무 좋다 ㅎㅎ 개편함
        
        if (!_isSliding)
        {
            _characterController.Move(direction * currentSpeed * Time.deltaTime);
        }
        else
        {
            _characterController.Move(_slideDirection * _slideDashForce * Time.deltaTime);
        }
    }

    private void GroundCheck()
    {
        if (_characterController.collisionFlags == CollisionFlags.Below)
        {
            _isJumping = false;
            currentJumpCount = 0;
        }
    }
    
    private void WallRideCheck()
    {
        if (_characterController.collisionFlags == CollisionFlags.Sides)
        {
            _isWallContacted = true;
        }
        else
        {
            _isWallContacted = false;
        }
    }
    private void JumpFuction()
    {
        if (Input.GetButtonDown("Jump") && currentJumpCount < _maxJumpCount)
        {
            _gVelocity = _jumpPower;
            _isJumping = true;
            currentJumpCount++;
        }
    }

    private void SprintFunction()
    {
        if (!_isWallRiding)
        {
            if (Input.GetKey(KeyCode.LeftShift) && _currentStamina > 0)
            {
                _isSprinting = true;
                UseStamina();
            }
            else
            {
                _isSprinting = false;
                RecoverStamina();       
            }
        }

        StaminaUIUpdate();
    }

    private void WallRideFunction()
    {
        if (_isWallContacted && Input.GetKey(KeyCode.Space) && _currentStamina > 0)
        {
            _isWallRiding = true;
            _gVelocity = _wallRideVelocity;
            UseStamina();
        }
        else
        {
            _isWallRiding = false;
        }
    }

    private void UseStamina()
    {
        _currentStamina -= StaminaDecreaseRate * Time.deltaTime;
        _currentStamina = Mathf.Max(_currentStamina, 0f);
        _canRecoverStamina = false;
        _recoveryTimer = 0f;
        StaminaUIUpdate();
    }

    private void RecoverStamina()
    {
        if (!_canRecoverStamina)
        {
            _recoveryTimer += Time.deltaTime;
            if (_recoveryTimer >= _staminaRecoveryDelay)
            {
                _canRecoverStamina = true;
            }
        }
        
        if (_canRecoverStamina && _currentStamina < MaxStamina)
        {
            _currentStamina += StaminaRecoveryRate * Time.deltaTime;
            _currentStamina = Mathf.Min(_currentStamina, MaxStamina);
            StaminaUIUpdate();
        }
    }

    private void SlideFunction(Vector3 moveDirection)
    {
        if (_slideCooldownTimer > 0)
        {
            _slideCooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.E) && !_isSliding && _slideCooldownTimer <= 0 && moveDirection.magnitude > 0)
        {
            _isSliding = true;
            _slideTimer = _slideDuration;
            _slideCooldownTimer = _slideCooldown;
            _slideDirection = moveDirection;
            _currentStamina -= _slideStaminaCost;
        }

        if (_isSliding)
        {
            _slideTimer -= Time.deltaTime;
            if (_slideTimer <= 0)
            {
                _isSliding = false;
            }
        }
    }

    private void StaminaUIUpdate()
    {
        if (_staminaSlider != null) _staminaSlider.value = _currentStamina;

        if (staminaFillImage != null)
        {
            float ratio = _currentStamina / MaxStamina;

            if (ratio > 0.5f)
                staminaFillImage.color = Color.green;
            else if (ratio > 0.2f)
                staminaFillImage.color = Color.yellow;
            else
                staminaFillImage.color = Color.red;

            staminaFillImage.color = Color.Lerp(staminaFillImage.color, targetColor, Time.deltaTime * 10f);
        }
    }
}