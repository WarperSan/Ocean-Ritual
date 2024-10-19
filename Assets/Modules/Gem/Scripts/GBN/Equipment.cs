using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;



public interface Equipment : Forgeable
{
    string Name { get; }
    componentGBN componentGBN { get; }
   

    public void UpdateStat();
   

}
