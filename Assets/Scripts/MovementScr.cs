using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScr : MonoBehaviour
{
    public int startingChar = 1;
    public float moveSpeed = 1000;
    public float airFriction = 2;

    public float normalForce = 400;
    public float jumpForce = 200;
    public float sideMultiplier = 2.4f;
    public float sideJumpFallReduction = 0.5f;
    public float airMultiplier = 1.2f;
    public float airJumpMin = 1;
    public float affectGravity = 120f;

    public static bool isFinished = false;
    private bool canControl { get { return !PauseScr.isPause && !isFinished && !CamScr.isFreecaming; } }

    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private float groundCheckDist = 0.8f;
    [SerializeField]
    private float onGroundMaxSpeed = 1f;

    [SerializeField]
    private float errorTime = 0.1f;
    private float preJump;
    private Vector2 lastGroundVector;
    private float postJump;
    private Collider2D moveCollider;
    private Rigidbody2D rig;
    private float lastJump;
    private float lastWallJump;

    [SerializeField]
    private bool canWallJump = true;
    [SerializeField]
    private bool canTripleJump;
    private int numJump = 0;
    [SerializeField]
    private bool canDash;

    [SerializeField]
    private float dashSpeed = 9;
    [SerializeField]
    private float minDashExtraSpeed = 3;
    private bool dashed;
    private float lastDash;

    [HideInInspector]
    public int whichCharIsOn = 1;
    public SpriteRenderer spriteRenderer;
    public Sprite[] characterSprites;
    public bool[] characterAccesses = { true, true, true, false };

    private void Awake()
    {
        GameMaster.checkpoints = new List<CheckpointScr>();
        moveCollider = GetComponent<Collider2D>();
        rig = GetComponent<Rigidbody2D>();
        switchToCharacter(startingChar);
        isFinished = false;
        Vector3 checkpointPos = GameMaster.GetPlayerSpawn();
        if (checkpointPos != Vector3.zero)
        {
            transform.position = checkpointPos;
            rig.gravityScale = GameMaster.GetGravity();
            characterAccesses = GameMaster.GetAccesses();
            switchToCharacter(GameMaster.GetChar());
        }
        //Time.timeScale = 0.1f;
    }
    private void Update()
    {
        if (canControl)
        {
            Vector2 groundVector = GetGroundVector();
            bool onGround = (groundVector.magnitude != 0) && rig.velocity.y < onGroundMaxSpeed;
            if (onGround)
            { 
                switchChar();
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                // jump
                if (onGround)
                {
                    GroundJump(groundVector);
                }
                else
                {
                    // wall jump
                    Vector2 sideVector = GetSideVector();
                    if (canWallJump && sideVector.magnitude != 0)
                    {
                        if (lastWallJump < Time.time)
                        {
                            SideJump(sideVector);
                        }
                    }
                    // cayote jump
                    else if (postJump > Time.time - errorTime)
                    {
                        GroundJump(lastGroundVector);
                    }
                    // triplejump
                    else if (canTripleJump && numJump < 3)
                    {
                        AirJump();
                    }
                    // setup prejump
                    else
                    {
                        preJump = Time.time + errorTime;
                    }
                }
            }
            else if (onGround)
            {
                // prejump
                if (preJump > Time.time)
                {
                    GroundJump(groundVector);
                }
                // collect onground data
                else
                {
                    if (lastJump < Time.time)
                    {
                        lastGroundVector = groundVector;
                        postJump = Time.time;
                        numJump = 0;
                    }
                }
            } 
            else
            {
                if (preJump > Time.time)
                {// wall jump
                    Vector2 sideVector = GetSideVector();
                    if (canWallJump && sideVector.magnitude != 0)
                    {
                        SideJump(sideVector);
                    }
                }
            }
            if (canTripleJump)
            {
                rig.AddForce(Vector2.up * Input.GetAxis("Vertical") * affectGravity * rig.gravityScale * Time.deltaTime);
            }
            if (Input.GetKeyDown(KeyCode.K) && canDash && !dashed)
            {
                DoDash();
                dashed = true;
            }
            else if (onGround)
            {
                if (lastDash < Time.time)
                {
                    dashed = false;
                }
            }
        }
    }

    private void DoDash()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 dashDir = !(h == 0 && v == 0) ? new Vector2(h, v).normalized :
            (rig.velocity.x >= 0) ? Vector2.right : Vector2.left;
        if (dashDir.y > 0)
        {
            lastDash = Time.time + errorTime;
            preJump = -1f;
            postJump = -1f;
        }
        if (rig.velocity.magnitude != 0)
        {
            float proj = rig.velocity.x * dashDir.x + rig.velocity.y * dashDir.y;
            proj = Mathf.Max(proj, minDashExtraSpeed);
            rig.velocity = dashDir * (proj + dashSpeed);
        }
        else
        {
            rig.velocity = dashDir * (minDashExtraSpeed + dashSpeed);
        }
        AudioManagerScr.PlaySound("Dash");
    }

    void FixedUpdate()
    {
        if (canControl)
        {
            float h = Input.GetAxisRaw("Horizontal");
            if (h != 0)
            {
                rig.AddForce(new Vector2(h * moveSpeed * Time.fixedDeltaTime * (rig.gravityScale + 2f), 0));
                rig.AddForce(new Vector2(rig.velocity.x * -airFriction * (rig.gravityScale + 2f), 0));
            }
        }
    }

    private void GroundJump(Vector2 groundVector)
    {
        if (lastJump < Time.time)
        {
            Vector2 JumpVector = groundVector * normalForce;
            JumpVector += new Vector2(0, jumpForce);
            rig.AddForce(JumpVector);
            preJump = -1f;
            postJump = -1f;
            lastJump = Time.time + errorTime;
            numJump = 1;
        }
        AudioManagerScr.PlaySound("Jump");
    }
    private void SideJump(Vector2 sideVector)
    {
        Vector2 JumpVector = sideVector * normalForce;
        JumpVector += new Vector2(0, jumpForce * sideMultiplier);
        rig.AddForce(JumpVector);
        if (rig.velocity.y < 0)
        {
            rig.velocity = new Vector2(rig.velocity.x, rig.velocity.y * sideJumpFallReduction);
        }
        preJump = -1f;
        postJump = -1f;
        lastWallJump = Time.time + errorTime;
        AudioManagerScr.PlaySound("WallJump");
    }

    private void AirJump()
    {
        Vector2 JumpVector = new Vector2(0, jumpForce * airMultiplier);
        rig.AddForce(JumpVector);
        rig.velocity = new Vector2(rig.velocity.x, Mathf.Max(rig.velocity.y, airJumpMin));
        preJump = -1f;
        postJump = -1f;
        numJump++;
        AudioManagerScr.PlaySound("AirJump");
    }

    /// <summary>
    /// Gets the normal vector for the ground
    /// </summary>
    /// <returns>normal vector, or (0, 0) if not on grond</returns>
    Vector2 GetGroundVector()
    {
        if (moveCollider.IsTouchingLayers(groundLayer))
        { 
            Bounds bounds = moveCollider.bounds;
            Vector2 v = new Vector2(bounds.center.x, bounds.center.y);
            RaycastHit2D hit = Physics2D.Raycast(v, Vector2.down, bounds.size.y * groundCheckDist, groundLayer);
            if (hit)
            {
                Debug.DrawRay(hit.point, hit.normal, Color.green);
                return hit.normal;
            }
        }
        return new Vector2();
    }

    Vector2 GetSideVector()
    {
        if (moveCollider.IsTouchingLayers(groundLayer))
        {
            Bounds bounds = moveCollider.bounds;
            Vector2 v = new Vector2(bounds.center.x, bounds.center.y);
            RaycastHit2D hit = Physics2D.Raycast(v, Vector2.left, bounds.size.y * groundCheckDist, groundLayer);
            RaycastHit2D hit2 = Physics2D.Raycast(v, Vector2.right, bounds.size.y * groundCheckDist, groundLayer);
            if (hit && hit2)
            {
                Debug.DrawRay(bounds.min, Vector2.up, Color.green);
                return Vector2.up;
            }
            if (hit)
            {
                Debug.DrawRay(hit.point, hit.normal, Color.green);
                return hit.normal;
            }
            if (hit2)
            {
                Debug.DrawRay(hit2.point, hit2.normal, Color.green);
                return hit2.normal;
            }
        }
        return new Vector2();
    }

    void switchChar()
    {
        if (Input.GetKeyDown("4") && whichCharIsOn != 4 && characterAccesses[3])
        {
            switchToCharacter(4);
        }
        else if (Input.GetKeyDown("3") && whichCharIsOn != 3 && characterAccesses[2])
        {
            switchToCharacter(3);
        }
        else if (Input.GetKeyDown("2") && whichCharIsOn != 2 && characterAccesses[1])
        {
            switchToCharacter(2);
        }
        else if(Input.GetKeyDown("1") && whichCharIsOn != 1 && characterAccesses[0])
        {
            switchToCharacter(1);
        }
    }
    private static bool[][] charAbilities = { 
        new bool[] { true, false, false },
        new bool[] { false, true, false },
        new bool[] { false, false, true },
        new bool[] { true, true, true } };
    public void switchToCharacter(int ind)
    {
        whichCharIsOn = ind;
        spriteRenderer.sprite = characterSprites[ind - 1];
        canWallJump = charAbilities[ind - 1][0];
        canTripleJump = charAbilities[ind - 1][1];
        canDash = charAbilities[ind - 1][2];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {   if (!isFinished)
        {
            if (collision.gameObject.CompareTag("Finish"))
            {
                LevelLoaderScr.Instance.LoadNextLevel();
                AudioManagerScr.PlaySound("Finish");
                Finish();
            }
            else if (collision.gameObject.CompareTag("FinalFinish"))
            {
                LevelLoaderScr.Instance.LoadFinalFinish();
                AudioManagerScr.StopSound("Theme1");
                AudioManagerScr.PlaySound("FinalFinish");
                Finish();
            }
        }
    }
    private void Finish()
    {
        isFinished = true;

    }
}
