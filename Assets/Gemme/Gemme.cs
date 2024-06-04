using System;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

[Serializable]
public class Gemme 
{

    [SerializeField]
    public FormeBool forme = new(new bool[,]
   {
        { false, false, true },
        { true, true, true },
        { false, false, true }
   }, 3, 3);
    public string GemmeColorsName;
    [SerializeField] public int LVL = 0;
    [SerializeField] public List<TypeQuantite<TypeArme>> typeArme;
    [SerializeField] public List<TypeQuantite<TypeBateau>> typeBoat;
    [SerializeField] public List<TypeQuantite<TypeFilet>> typeFilet;
    public List<TypeQuantite<TEnum>> GetListType<TEnum>()
    {
        if (typeof(TEnum) == typeof(TypeArme))
        {
            return typeArme as List<TypeQuantite<TEnum>>;
        }
        else if (typeof(TEnum) == typeof(TypeBateau))
        {
            return typeBoat as List<TypeQuantite<TEnum>>;
        }
        else if (typeof(TEnum) == typeof(TypeFilet))
        {
            return typeFilet as List<TypeQuantite<TEnum>>;
        }
        else
        {
            
            Debug.LogError("Type non géré.");
            return null;
        }
    }
}
    