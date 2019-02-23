using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//An interface to standardize hitbox attack interfaces
public interface IHitboxResponder {

    void collisionedWith(Collider collider, string move);

}

//An interface to standardize attack hurtbox interfaces
public interface IAttackResponder{

    void getHitBy(moveData move);

}


