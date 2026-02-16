using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class ShootEntity: MonoBehaviour, IShootEntity, IDamageable
{
    //PARENT CLASS FOR ALL SHOOT ENTITIES IN THE GAME

    #region Fields
    //ACCESS WILL BE REQUIRED BY CHILDREN
    protected Rigidbody2D _rb;
    protected float f_CurrentBulletForce;
    protected float f_RemainingHitPoints;
    protected float f_CurrentBulletCooldown;
    protected bool b_TripleBullets;
    protected BulletType e_CurrentBulletType;

    //INSPECTOR FIELDS
    [SerializeField] protected ShootEntityData _data;
    [SerializeField] private Transform _BulletOrigin;
    #endregion

    #region Monobehaviour Methods
    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {
        _rb.velocity = Vector3.zero;
    }

    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if (f_CurrentBulletCooldown > 0)
            f_CurrentBulletCooldown -= Time.deltaTime;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //DETECTS COLLISION FOR DAMAGE CALCULATION
        IDamageable other = collision.GetComponent<IDamageable>();
        if(other != null)
        {
            //EXCHANGES DAMAGE WITH OTHER ENTITY
            this.DamageItself(other.DamageOther());
        }
    }
    #endregion

    #region Public Methods
    //SHOOT ACTION
    public virtual void Shoot()
    {
        if (f_CurrentBulletCooldown > 0)
            return;

        GameObject bulletBluePrint = SpaceShooterManager.Instance.bulletTypes[(int)e_CurrentBulletType];
        if (bulletBluePrint == null)
            return;

        if(b_TripleBullets)
        {
            //SHOOT THREE BULLETS AT ONCE
        }
        else
        {
            //SHOOT ONE BULLET AT ONCE
            GameObject bullet_01 = Instantiate(bulletBluePrint, _BulletOrigin.position, this.transform.rotation);
            SpaceShooterManager.Instance.AddEntity(bullet_01);
            bullet_01.GetComponent<Rigidbody2D>().AddForce(this.transform.up * this.f_CurrentBulletForce, ForceMode2D.Impulse);
        }

        //RESET BULLET COOLDOWN TIMER
        f_CurrentBulletCooldown = _data.f_BulletCooldownTime;
    }
    //APPLIES DAMAGE TO ITSELF
    public virtual void DamageItself(float f_Damage) 
    {
        f_RemainingHitPoints -= f_Damage;

        if (f_RemainingHitPoints < 0)
            ReturnToPool();
    }
    //APPLIES DAMAGE TO OTHER ENTITIES
    public virtual float DamageOther() { return _data.f_CollisionDamagePoints; }
    //RETURN SHOOT ENTITY TO ENTITY POOL
    public virtual void ReturnToPool()
    {
        this.transform.position = SpaceShooterManager.Instance._EntityReturnPool.position;
        //this.gameObject.SetActive(false);
    }
    //DESTROY SHOOT ENTITY
    public virtual void Remove() { Destroy(this.gameObject); }

    //GET VALUES FROM ENTITY SCRIPTABLE OBJECT
    public void InitializeDataValues()
    {
        this.f_RemainingHitPoints = _data.f_HitPoints;
        this.f_CurrentBulletCooldown = _data.f_BulletCooldownTime;
        this.f_CurrentBulletForce = _data.f_BulletForce;
        this.b_TripleBullets = _data.b_TripleBullets;
        this.e_CurrentBulletType = _data.e_InitialBulletType;
    }
    #endregion
}
