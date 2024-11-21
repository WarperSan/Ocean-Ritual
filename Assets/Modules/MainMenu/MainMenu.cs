using DhafinFawwaz.AnimationUILib;
using ExtensionsModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private AnimationUI transitionAnimation;

    public void PlayButton() => this.StartCoroutine(this.TransitionToScene("Game"));

    public void CreditsButton() => this.StartCoroutine(this.TransitionToScene("Credits"));

    public void ExitButton() => Application.Quit();

    private IEnumerator TransitionToScene(string sceneName)
    {
        yield return transitionAnimation.PlayAnimation();

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Wait until loaded
        while (!operation.isDone)
            yield return null;
    }
}
