using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private bool isDead;
    [SerializeField] private float timeTodead;
    [SerializeField] private Transform transformGroundCheck;

    private Rigidbody2D rb;
    private Animator amin;
    private Collider2D col2D;
    public CharacterStats characterStats;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        amin = gameObject.GetComponent<Animator>();
        col2D = gameObject.GetComponent<Collider2D>();
    }

    void Awake()
    {
        isDead = false;
        characterStats.SetDefaultAllVariables();
    }

    private void Movement()
    {
        if(!isDead){
            Run();
            Jump();
        }
    }

    void Run()
    {
        transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * characterStats.Speed() * Time.deltaTime, 0f , 0f));
        if(Input.GetAxisRaw("Horizontal") > 0)
        {
            FlipCharacter(1);
        }
        else if(Input.GetAxisRaw("Horizontal") < 0)
        {
            FlipCharacter(-1);
        }
        
    }

    void Jump()
    {
        Climb();
        if(Input.GetKeyDown(KeyCode.Space) && characterStats.IsGround() && IsVelocityDontChange() && !IsClimb()){
            rb.AddForce(Vector2.up * characterStats.Speed(), ForceMode2D.Impulse);
        }
    }

    private bool IsClimb(){
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, 
                                                    characterStats.Speed(), characterStats.whatIsLadder);
        if(hitInfo.collider != null)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                characterStats.IsLadder(true);
            }
        }
        else
        {
            characterStats.IsLadder(false);
        }

        return characterStats.IsLadder();
    }

    private void Climb()
    {
        if(IsClimb() && Input.GetKey(KeyCode.Space)){
            rb.velocity = new Vector2(rb.velocity.x, 2f);
            rb.gravityScale = 0;
        }else{
            if(rb.gravityScale == 0){
                rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
                rb.gravityScale = 1;
            }
        }
    }

    private void FlipCharacter(int state)
    {
        Vector3 trans = transform.localScale;
        trans.x = state;
        transform.localScale = trans;
    }

    public void ChangeStateAnimation(string state)
    {
        amin.Play(state);
    }

    public void UpdateAnimationClip()
    {
        if(!isDead){
            if(characterStats.IsGround() && !characterStats.IsLadder() && IsVelocityDontChange()){
                if(Input.GetAxisRaw("Horizontal") != 0){
                    ChangeStateAnimation(characterStats.animationStates.PLAYER_RUN);
                }else{
                    ChangeStateAnimation(characterStats.animationStates.PLAYER_IDLE);
                }
            }else if(characterStats.IsLadder()){
                ChangeStateAnimation(characterStats.animationStates.PLAYER_CLIMB);
            }else if(rb.velocity.y != 0){
                if(rb.velocity.y > 0){
                    ChangeStateAnimation(characterStats.animationStates.PLAYER_JUMP);
                }else{
                    ChangeStateAnimation(characterStats.animationStates.PLAYER_FALL);
                }
            }
        }
    }

    void BlockMovement()
    {
        float dist = (transform.position - Camera.main.transform.position).z;

        float leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
        float rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;

        Vector3 playerSize = GetComponent<Renderer>().bounds.size / 2;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, leftBorder + playerSize.x / 2, rightBorder - playerSize.x / 2),
            transform.position.y, transform.position.z
        );
    }

    public bool IsVelocityDontChange(){
        return rb.velocity.y > -0.01f && rb.velocity.y < 0.01f;
    }

    public bool GroundCheck()
    {
        return Physics2D.OverlapCircle(transformGroundCheck.position, 0.2f, characterStats.GroundLayer);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "ground"){
            if(characterStats.IsLadder()){
                characterStats.IsLadder(false);
            }
        }else if(collision.gameObject.tag == "deadZone"){
            StartCoroutine(RepawnPlayer());
        }else if(collision.gameObject.tag == "enemy"){
            StartCoroutine(RepawnPlayer());
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "gem"){
            characterStats.Score(characterStats.Score() + 1);
        }

        if(collider.gameObject.tag == "savePoint"){
            characterStats.SavePoint = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "ladder" && characterStats.IsLadder()){
            transform.position = new Vector3(other.gameObject.transform.position.x, 
                                                transform.position.y, transform.position.z);
        }
    }

    IEnumerator RepawnPlayer()
    {
        isDead = true;
        ChangeStateAnimation(characterStats.animationStates.PLAYER_DEAD);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(timeTodead);
        characterStats.IsDead(isDead);
    }

    void Update()
    {
        Movement();
        BlockMovement();
    }

    void FixedUpdate()
    {
        characterStats.IsGround(GroundCheck());
        UpdateAnimationClip();
    }
}