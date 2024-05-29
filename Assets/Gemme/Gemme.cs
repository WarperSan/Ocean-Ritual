using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

public class Gemme : MonoBehaviour
{
    [SerializeField] public int width;
    [SerializeField] public int height;
    [SerializeField]
    private FormeBool forme = new(new bool[,]
   {
        { false, false, true },
        { true, true, true },
        { false, false, true }
   });
    public void InitializeForme(int width, int height)
    {
        forme.InitializeIfNeeded(width, height);
    }

    [SerializeField] private List<TypeQuantite<TypeArme>> typeArme;
    [SerializeField] private List<TypeQuantite<TypeBateau>> typeBoat;
    [SerializeField] private List<TypeQuantite<TypeFilet>> typeFilet;
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
    