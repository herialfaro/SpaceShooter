using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(InputManager))]
public class PlayableEntity : ShootEntity
{
    #region Fields
    private InputManager _inputManager;
    #endregion

    #region Monobehaviour Methods
    protected override void OnEnable()
    {
        base.OnEnable();

        f_CurrentBulletCooldown = 0;
    }

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

        Reposition();
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
    private void Move()
    {
        _rb.MovePosition((Vector2)transform.position + _inputManager.v_Move * _data.f_Speed * Time.fixedDeltaTime);
    }
    #endregion

    #region Protected Methods
    protected override void Reposition()
    {
        base.Reposition();

        //FIND LIMITS OF SCREEN AND REPOSITION PLAYER
        if (this.transform.position.x > Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect)
            this.transform.position = new Vector3(Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect, this.transform.position.y, 0.5f);

        if (this.transform.position.x < Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect)
            this.transform.position = new Vector3(Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect, this.transform.position.y, 0.5f);

        if (this.transform.position.y > Camera.main.transform.position.y + Camera.main.orthographicSize)
            this.transform.position = new Vector3(this.transform.position.x, Camera.main.transform.position.y + Camera.main.orthographicSize, 0.5f);

        if (this.transform.position.y < Camera.main.transform.position.y - Camera.main.orthographicSize)
            this.transform.position = new Vector3(this.transform.position.x, Camera.main.transform.position.y - Camera.main.orthographicSize, 0.5f);
    }
    #endregion

    #region Public Methods
    public override void ReturnToPool()
    {
        base.ReturnToPool();

        SpaceShooterManager.Instance.PlayerLostLife();
        enabled = false;
    }
    #endregion
}
