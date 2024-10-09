using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlacksmithGemSlot : MonoBehaviour
{
    [SerializeField] GemData gem;
    [SerializeField] Image sprite;
    [SerializeField] Graphic background;

    public void ReceiveGem(GemData gem)
    {
        this.gem = gem;
        sprite.sprite = gem.sprite;
    }
}
