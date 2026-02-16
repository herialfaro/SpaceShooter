using UnityEngine;

public interface IDamageable
{
    void DamageItself(float f_Damage);
    float DamageOther();
    void Remove();
}
