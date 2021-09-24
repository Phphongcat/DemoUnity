using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    [SerializeField] private float speed;

    public bool SavePoint;
    public int MaxHealth{get; private set;}
    private bool isJumpOrFall;
    private bool isGround;
    private bool isLadder;
    private bool isDead;
    private int score;

    public LayerMask GroundLayer;
    public LayerMask whatIsLadder;
    public AnimationStates animationStates;

    public void SetDefaultAllVariables()
    {
        SavePoint = false;
        Score(0);

        IsDead(false);
        IsLadder(false);
        IsGround(false);
        
        IsJumpOrFall(true);
    }

    public void Speed(float inSpeed) {speed = inSpeed; }
    public float Speed(){ return speed; }

    public void Score(int inScore) { score = inScore; }
    public int Score() { return score; }

    public void IsGround(bool enable) { isGround = enable; }
    public bool IsGround() { return isGround; }

    public void IsJumpOrFall(bool enable){isJumpOrFall = enable;}
    public bool IsJumpOrFall() { return isJumpOrFall; }

    public void IsLadder(bool enable) { isLadder = enable; }
    public bool IsLadder() { return isLadder; }

    public void IsDead(bool isDeaded) { isDead = isDeaded; }
    public bool IsDead() { return isDead; }
}

[System.Serializable]
public class AnimationStates
{
    public string PLAYER_IDLE;
    public string PLAYER_RUN;
    public string PLAYER_JUMP;
    public string PLAYER_FALL;
    public string PLAYER_CLIMB;
    public string PLAYER_DEAD;
}
