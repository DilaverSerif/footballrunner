using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;
using Lean.Touch;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private float normalySpeed, maxSpeed;
    private SplineFollower _splineFollower;
    private Rigidbody[] _rigidbodies;
    private Collider[] _colliders;
    private Collider baseCollider;
    private Rigidbody _rigidbody;

    private Animator _animator;

    [SerializeField] private Transform spine;
    public static Action FinishEvent;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        baseCollider = GetComponent<Collider>();
        _splineFollower = GetComponent<SplineFollower>();
        _rigidbody = GetComponent<Rigidbody>();
        
        
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();

        RagdollSet(false);
    }

    private void Start()
    {
        _splineFollower.followSpeed = normalySpeed;
    }

    private void OnEnable()
    {
        LeanTouch.OnFingerDown += MoveLeft;
        LeanTouch.OnFingerUp += MoveRight;
    }
    
    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= MoveLeft;
        LeanTouch.OnFingerUp -= MoveRight;
    }

    private void RagdollSet(bool active)
    {
        foreach (var collider in _colliders)
        {
            collider.enabled = active;
        }
        
        foreach (var body in _rigidbodies)
        {
            body.isKinematic = !active;
        }
        
        baseCollider.enabled = true;
        _rigidbody.isKinematic = true;
    }

    private void MoveLeft(LeanFinger obj)
    {
        LeftAnim();
    }

    private void LeftAnim()
    {
        var pos = Vector2.zero;
        Sequence toLeft = DOTween.Sequence();
        DOTween.Kill("Move");

        pos.x = -2.5f;

        toLeft.Join(DOTween.To(() => _splineFollower.followSpeed, x => _splineFollower.followSpeed = x,
            maxSpeed, 0.75f));

        toLeft.Join(DOTween.To(() => _splineFollower.motion.offset, x =>
                _splineFollower.motion.offset = x, pos, 0.75f
        ));
        toLeft.Join(DOTween.To(() => _splineFollower.motion.rotationOffset, 
            x => _splineFollower.motion.rotationOffset = x, 
            new Vector3(0,-15,9),  0.35f
        ).OnComplete(
            () =>
            {
                DOTween.To(() => _splineFollower.motion.rotationOffset,
                    x => _splineFollower.motion.rotationOffset = x,
                    Vector3.zero, 0.3f);
            }
            ));

        toLeft.SetId("Move");
    }
    private void MoveRight(LeanFinger obj)
    {
        RightAnim();
    }

    private void RightAnim()
    {
        var pos = Vector2.zero;
        Sequence toRight = DOTween.Sequence();
        DOTween.Kill("Move");

        toRight.Join(DOTween.To(() => _splineFollower.followSpeed, 
            x => _splineFollower.followSpeed = x, normalySpeed, 0.75f));

        toRight.Join(DOTween.To(() => _splineFollower.motion.offset, 
            x => _splineFollower.motion.offset = x, pos, 0.5f
        ));
        
        toRight.Join(DOTween.To(() => _splineFollower.motion.rotationOffset, 
            x => _splineFollower.motion.rotationOffset = x, 
            new Vector3(0,15,-9), 0.35f
        ).OnComplete(
            () =>
            {
                DOTween.To(() => _splineFollower.motion.rotationOffset,
                    x => _splineFollower.motion.rotationOffset = x,
                    Vector3.zero, 0.3f);
            }
        ));
        
        toRight.SetId("Move");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            Menu.FinishGame.Invoke(false);
            _animator.enabled = false;
            _splineFollower.follow = false;
            RagdollSet(true);
            spine.GetComponent<Rigidbody>().velocity = new Vector3(0,1,-1) * 10;
        }
        if (other.gameObject.CompareTag("Finish"))
        {
            Menu.FinishGame.Invoke(true);

            FinishEvent.Invoke();
            StartCoroutine(FinishAnim());
        }
    }

    IEnumerator FinishAnim()
    {
        _animator.SetInteger("finish",Random.Range(1,4));

        yield return new WaitForSeconds(.5f);
        
        _splineFollower.follow = false;
    }
    
}
