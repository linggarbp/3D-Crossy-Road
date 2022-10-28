using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] AudioSource sfxClick;
    [SerializeField] AudioSource sfxDie;
    [SerializeField] TMP_Text scoreText;
    //[SerializeField] ParticleSystem dieParticle;
    [SerializeField, Range(0.01f, 1f)] float moveDuration = 0.2f;
    [SerializeField, Range(0.01f, 1f)] float jumpHeight = 0.5f;

    //private int minzpos;
    //private int extent;
    private float backBoundary;
    private float leftBoundary;
    private float rightBoundary;
    [SerializeField] private int maxTravel;

    public int MaxTravel { get => maxTravel;}
    [SerializeField] private int currentTravel;
    public int CurrentTravel { get => currentTravel;}
    public bool IsDie { get => this.enabled == false; }

    public void SetUp(int minZPos, int extent)
    {
        backBoundary = minZPos - 1;
        leftBoundary = -(extent + 1);
        rightBoundary = extent + 1;
    }

    public void soundClick()
    {
        sfxClick.Play();
    }

    private void Update()
    {
        var moveDir = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveDir += new Vector3(0, 0, 1);
            soundClick();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDir += new Vector3(0, 0, -1);
            soundClick();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDir += new Vector3(1, 0, 0);
            soundClick();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDir += new Vector3(-1, 0, 0);
            soundClick();
        }

        if (moveDir != Vector3.zero && IsJumping() == false)
            Jump(moveDir);
    }

    private void Jump(Vector3 targetDirection)
    {
        //atur rotasi
        Vector3 TargetPosition = transform.position + targetDirection;
        transform.LookAt(TargetPosition);

        //loncat ke atas
        var moveSeq = DOTween.Sequence(transform);
        moveSeq.Append(transform.DOMoveY(jumpHeight, moveDuration/2));
        moveSeq.Append(transform.DOMoveY(0, moveDuration/2));

        if (TargetPosition.z <= backBoundary ||
            TargetPosition.x <= leftBoundary ||
            TargetPosition.x >= rightBoundary)
            return;

        if (Tree.AllPosition.Contains(TargetPosition))
            return;

        //gerak maju/mundur/samping
        transform.DOMoveX(TargetPosition.x, moveDuration);
        transform
            .DOMoveZ(TargetPosition.z, moveDuration)
            .OnComplete(UpdateTravel);
    }

    private void UpdateTravel()
    {
        currentTravel = (int)this.transform.position.z;
        if (currentTravel > maxTravel)
            maxTravel = currentTravel;

        scoreText.text = "Step : " + maxTravel.ToString();
    }

    public bool IsJumping()
    {
        return DOTween.IsTweening(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.enabled == false)
            return;
        if (other.tag == "Car")
        {
            AnimateDie();
            sfxDie.Play();
        }
    }

    private void AnimateDie()
    {
        transform.DOScaleY(0.1f, 0.2f);
        transform.DOScaleX(1, 0.2f);
        transform.DOScaleZ(0.75f, 0.2f);
        this.enabled = false;
        //dieParticle.Play();
    }
}
