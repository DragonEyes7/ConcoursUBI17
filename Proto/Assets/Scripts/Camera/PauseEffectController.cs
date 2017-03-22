using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class PauseEffectController : MonoBehaviour {

    [SerializeField]
    NoiseAndScratches NoiseAndScratchEffect;
    [SerializeField]
    NoiseAndGrain NoiseAndGrainEffect;
    [SerializeField]
    Vortex VortexEffect1;
    [SerializeField]
    Vortex VortexEffect2;
    [SerializeField]
    VignetteAndChromaticAberration VignetteEffect;

    public void ActivatePauseEffect()
    {
        NoiseAndScratchEffect.enabled = true;
        NoiseAndGrainEffect.enabled = true;
        VortexEffect1.enabled = true;
        VortexEffect2.enabled = true;
        VignetteEffect.enabled = true;
    }
    
    public void DeactivatePauseEffect()
    {
        NoiseAndScratchEffect.enabled = false;
        NoiseAndGrainEffect.enabled = false;
        VortexEffect1.enabled = false;
        VortexEffect2.enabled = false;
        VignetteEffect.enabled = false;
    }
}
