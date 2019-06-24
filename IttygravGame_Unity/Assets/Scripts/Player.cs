using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	private bool _isFacingRight;
	private CharacterController2D _controller;
	private float _normalizedHorizontalSpeed;

	public float MaxSpeed = 8;
	public float SpeedAccelerationOnGround = 10f;
	public float SpeedAccelerationInAir = 5f;

	public bool IsDead { get; private set; }

    private CrateController2D pushingCrate;

    public bool ControlsFrozen = false;
    public bool PositionFrozen = false;

    public GameObject PlayerBody;

    public AudioClip JumpAudio;

    private bool isGrounded = true;

	public void Awake()
	{
		_controller = GetComponent<CharacterController2D> ();
		_isFacingRight = transform.localScale.x > 0; 
        //assuming right is default
	}

	public void Update()
	{
        if (!IsDead)
        {
            
            HandleInput();
            _controller.MotionFrozen = PositionFrozen;
        }
		
        if(_controller.State.IsGrounded && !isGrounded){
            if (GameObject.FindGameObjectWithTag("AudioSource")) {
                GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioController>().PlayPlayerFX(1);
            }

        }
        isGrounded = _controller.State.IsGrounded;

		var movementFactor = _controller.State.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;

		// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
		if (IsDead)
			_controller.SetHorizontalForce (new Vector2(0,0));
		else
			_controller.SetHorizontalForce (new Vector2(Mathf.Lerp(_controller.Velocity.x, _normalizedHorizontalSpeed * MaxSpeed, Time.deltaTime * movementFactor), 0));
	}
    public float GetHorzontalDisplacement(){
        return _controller.Velocity.x * Time.deltaTime;
    }
	public void Kill(){
		_controller.HandleCollisions = false;
		GetComponent<Collider2D>().enabled = false;
		IsDead = true;

		_controller.SetForce(new Vector2(0, 20f));
	}

	public void RespawnAt(Transform spawnPoint){
		if (!_isFacingRight)
			Flip ();

		IsDead = false;
		_controller.HandleCollisions = true;
		GetComponent<Collider2D>().enabled = true;

		transform.position = spawnPoint.position;
	}

	private void HandleInput(){
        if (!ControlsFrozen)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {

                _normalizedHorizontalSpeed = 1;
                if (!_isFacingRight)
                {
                    Flip();
                }

            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                _normalizedHorizontalSpeed = -1;
                if (_isFacingRight)
                {
                    Flip();
                }
            }
            else
            {
                _normalizedHorizontalSpeed = 0;
            }

            if (_controller.CanJump && Input.GetKeyDown(KeyCode.Space))
            {
                _controller.Jump();

                //GetComponent<AudioSource>().clip = JumpAudio;
                //GetComponent<AudioSource>().Play();
                GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioController>().PlayPlayerFX(0);
            }
        }
        else
        {
            _normalizedHorizontalSpeed = 0;
        }
        
        
	}

	private void Flip(){
        PlayerBody.transform.localScale = new Vector3 (-PlayerBody.transform.localScale.x, PlayerBody.transform.localScale.y, PlayerBody.transform.localScale.z);
        _isFacingRight = PlayerBody.transform.localScale.x > 0;
	}

    private void HandlePlatformRotation()
    {

    }
}
