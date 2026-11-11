using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class VFXLauncher : MonoBehaviour
{
    [Serializable]
    class VFX
    {
        public VFXID ID;
        public List<ParticleSystem> ParticleSystems;
        public List<VisualEffect> VisualEffects;

    }

    [SerializeField] List<VFX> _effects;

    public void StartEffect(VFXID id)
    {
        
        var found = _effects.FirstOrDefault(i => i.ID == id);

        if (found == null)
        {
            return;
        }
        foreach (var ps in found.ParticleSystems)
        {
            ps.Play();
        }
        foreach (var vfx in found.VisualEffects)
        {
            vfx.Play();
            
        }
    }

    public void EndEffect(VFXID id)
    {
        var found = _effects.FirstOrDefault(i => i.ID == id);

        if (found == null)
        {
            return;
        }
        foreach (var ps in found.ParticleSystems)
        {
            ps.Stop();
        }
        foreach (var vfx in found.VisualEffects)
        {
            vfx.Stop();

        }
    }
}