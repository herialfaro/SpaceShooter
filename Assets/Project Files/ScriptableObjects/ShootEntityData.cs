using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ShootEntityData", order = 1)]
public class ShootEntityData : ScriptableObject
{
    public float f_Speed = 1;
    public float f_HitPoints = 1;
    public float f_CollisionDamagePoints = 1;
    public float f_BulletCooldownTime = 1;
    public float f_BulletForce = 1;
    public bool b_TripleBullets = false;
    public BulletType e_InitialBulletType = BulletType.none;
}
