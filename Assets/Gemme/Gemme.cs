using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

public class Gemme : MonoBehaviour
{


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
    //public TypeDeSocle TypeDeSocle
    //{
    //    get { return typeDeSocle; }
    //    set { typeDeSocle = value; }
    //}

    //public TypeArme TypeArme
    //{
    //    get { return typeArme; }
    //    set { typeArme = value; }
    //}

    //public TypeBateau TypeBateau
    //{
    //    get { return typeBateau; }
    //    set { typeBateau = value; }
    //}

    //public TypeFilet TypeFilet
    //{
    //    get { return typeFilet; }
    //    set { typeFilet = value; }
    //}
