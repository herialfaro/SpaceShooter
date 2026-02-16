using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(InputManager))]
public class PlayableEntity : ShootEntity
{
    #region Fields
    private InputManager _inputManager;
    #endregion

    #region Monobehaviour Methods
    protected override void Start()
    {
        base.Start();

        _inputManager = GetComponent<InputManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (_inputManager.b_Shoot)
            Shoot();
    }

    private void FixedUpdate()
    {
        if (_inputManager.v_Move.magnitude > 0)
            Move();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);


    }
    #endregion

    #region Private Methods
    public void Move()
    {
        _rb.MovePosition((Vector2)transform.position + _inputManager.v_Move * _data.f_Speed * Time.fixedDeltaTime);
    }
    #endregion

    #region Public Methods
    public override void ReturnToPool()
    {
        base.ReturnToPool();

        SpaceShooterManager.Instance.PlayerWasDefeated();
        enabled = false;
    }
    #endregion
}
