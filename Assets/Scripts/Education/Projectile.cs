using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : IStrategy
{
    public bool SkillPlay()
    {
        return true;
    }    
}

public class SkillProjectile: Projectile
{

}

