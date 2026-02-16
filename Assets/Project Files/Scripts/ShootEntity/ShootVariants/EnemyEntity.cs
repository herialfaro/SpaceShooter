using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class EnemyEntity : ShootEntity
{
    #region Fields
    public Vector2 v_Move;
    #endregion

    #region Monobehaviour Methods
    protected override void OnEnable()
    {
        base.OnEnable();

        v_Move = Vector2.zero;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        Shoot();

        Reposition();
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
    #endregion

    #region Protected Methods
    protected override void Reposition()
    {
        base.Reposition();

        //FIND LIMITS OF SCREEN AND RETURN TO POOL IF EXITS SCREEN
        if (this.transform.position.x > Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect)
            ReturnToPool();

        if (this.transform.position.x < Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect)
            ReturnToPool();

        if (this.transform.position.y > Camera.main.transform.position.y + Camera.main.orthographicSize)
            ReturnToPool();

        if (this.transform.position.y < Camera.main.transform.position.y - Camera.main.orthographicSize)
            ReturnToPool();
    }
    #endregion

    #region Public Methods
    private void Move()
    {
        _rb.MovePosition((Vector2)transform.position + v_Move * _data.f_Speed * Time.fixedDeltaTime);
    }

    public override void ReturnToPool()
    {
        base.ReturnToPool();
        this.gameObject.SetActive(false);
        SpaceShooterManager.Instance.AddScore(_data.i_EntityPoints);
    }
    #endregion
}
