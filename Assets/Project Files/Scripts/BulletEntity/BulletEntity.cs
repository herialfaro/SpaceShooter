using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletEntity : MonoBehaviour, IDamageable
{
    #region Fields
    [SerializeField] private float f_BulletDamage = 1;
    [SerializeField] private float f_LifeTimeCountdown = 1;

    [SerializeField] private AudioClip _BulletAudioClip;
    #endregion

    #region Monobehaviour Methods
    private void Update()
    {
        if (f_LifeTimeCountdown > 0)
            f_LifeTimeCountdown -= Time.deltaTime;

        CheckCountdown();
    }
    #endregion

    #region Private Methods
    private void CheckCountdown()
    {
        if (f_LifeTimeCountdown > 0)
            return;

        Remove();
    }
    #endregion

    #region Public Methods
    //APPLIES DAMAGE TO ITSELF
    public void DamageItself(float f_Damage) { }
    //APPLIES DAMAGE TO OTHER ENTITIES
    public float DamageOther() 
    {
        SpaceShooterManager.Instance.PlaySFXClip(_BulletAudioClip);
        Remove();
        return f_BulletDamage;
    }
    //DESTROY SHOOT ENTITY
    public void Remove() { Destroy(this.gameObject); }
    #endregion
}
