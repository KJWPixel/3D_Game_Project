using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStrategy 
{
    public bool SkillPlay();  
}

//public class Strategy : IStrategy
//{
//    public bool SkillPlay()
//    {
//        return true;
//    }
//}

public abstract class Strategy
{
    public abstract void Skill();
}

public class Nearing : Strategy
{
    public override void Skill()
    {

    }
}
