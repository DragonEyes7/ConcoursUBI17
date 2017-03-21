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

    
    public void TogglePauseEffect()
    {
        NoiseAndScratchEffect.enabled = !NoiseAndScratchEffect.enabled;
        NoiseAndGrainEffect.enabled = !NoiseAndGrainEffect.enabled;
        VortexEffect1.enabled = !VortexEffect1.enabled;
        VortexEffect2.enabled = !VortexEffect2.enabled;
        VignetteEffect.enabled = !VignetteEffect.enabled;
    }
}
