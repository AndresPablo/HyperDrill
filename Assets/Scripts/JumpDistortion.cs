using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;

public class JumpDistortion : MonoBehaviour
{
    [SerializeField] Volume volume;
    [SerializeField] float jumpDistortionAmount = .2f;

    void Start()
    {
        Player.OnPlayerMoves += Distorsionar;
        Player.OnPlayerStop += Restaurar;
    }

    void Distorsionar()
    {
        if(volume.profile.TryGet<LensDistortion>(out var lens))
        {
            lens.intensity.overrideState = true;
            lens.intensity.value = jumpDistortionAmount;
        }
    }

    void Restaurar()
    {
        if(volume.profile.TryGet<LensDistortion>(out var lens))
        {
            lens.intensity.overrideState = false;
            lens.intensity.value = 0;
        }
    }
}
