using UnityEngine;

namespace DhafinFawwaz.AnimationUILib.Demo
{
    public class AudioObserver : MonoBehaviour
    {
        [SerializeField]
        private AudioManager _audio;

        private void OnEnable()
        {
            ButtonUI.s_onClick += ButtonOnClick;
            ButtonUI.s_onPointerEnter += ButtonEnter;
            ButtonUI.s_onPointerDown += ButtonDown;
            ButtonUI.s_onSelect += ButtonEnter;

            AnimationUI.OnPlaySoundByFile += _audio.PlaySound;
            AnimationUI.OnPlaySoundByIndex += _audio.PlaySound;
        }

        private void OnDisable()
        {
            ButtonUI.s_onClick -= ButtonOnClick;
            ButtonUI.s_onPointerEnter -= ButtonEnter;
            ButtonUI.s_onPointerDown -= ButtonDown;
            ButtonUI.s_onSelect -= ButtonEnter;

            AnimationUI.OnPlaySoundByFile -= _audio.PlaySound;
            AnimationUI.OnPlaySoundByIndex -= _audio.PlaySound;
        }

        private void ButtonEnter()   => _audio.PlaySound(3);
        private void ButtonOnClick() => _audio.PlaySound(2);
        private void ButtonDown()    => _audio.PlaySound(0);
    }
}