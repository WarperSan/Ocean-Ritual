using EntityModule;
using UnityEngine;

public class BalleLF : Projectile
{
    #region Propri�t�s

    float distanceTravel = 0;

    #endregion

    #region Properties

    [Header("Properties")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float range = 50;

    /// <summary>
    /// Sets the properties for this projectile
    /// </summary>
    public void SetProperties(float speed, float range)
    {
        this.speed = speed;
        this.range = range;
    }

    #endregion

    #region Growing

    [Header("Growing")]
    [SerializeField]
    private bool isGrowing = true;

    [SerializeField]
    private float growingSpeed = 1.5f;

    [SerializeField]
    private Transform growingTarget;

    private void Grow(float elapsed)
    {
        if (growingTarget == null)
            return;

        // Grossit l'enfant en utilisant la valeur de ValeurGrossissement
        float scaleIncrement = this.growingSpeed * this.speed * elapsed;
        growingTarget.localScale += new Vector3(0, scaleIncrement, scaleIncrement);
    }

    #endregion

    #region Projectile

    /// <inheritdoc/>
    protected override void OnMove(float elapsed)
    {
        // Calculer la distance que la balle a parcourue depuis la derni�re frame
        float distanceThisFrame = this.speed * elapsed;

        // Ajouter cette distance � la distance totale parcourue
        this.distanceTravel += distanceThisFrame;

        // Si la distance parcourue d�passe la port�e, d�sactiver la balle
        if (this.distanceTravel >= this.range)
        {
            this.distanceTravel = 0;
            this.gameObject.SetActive(false);
        }
        else
        {
            // D�placer la balle vers l'avant
            this.transform.Translate(Vector3.forward * distanceThisFrame);
        }
    }

    /// <inheritdoc/>
    protected override void OnReset()
    {
        // Reset growning
        if (growingTarget != null)
            growingTarget.localScale = new Vector3(0.1f, 1f, 1f);

        this.distanceTravel = 0;
    }

    /// <inheritdoc/>
    protected override void OnUpdate(float elapsed)
    {
        if (this.isGrowing)
            this.Grow(elapsed);
    }

    #endregion
}
