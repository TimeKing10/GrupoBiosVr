using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

public class MachineSound : MonoBehaviour
{
    public AudioSource audioSource; // arrastra un AudioSource en el inspector

    void Update()
    {
        if (MachineReader.Machines == null || MachineReader.Machines.Count == 0)
            return;

        // Revisa si alguna máquina está encendida (estadoRed == "online")
        bool algunaPrendida = MachineReader.Machines.Any(m =>
            m.ContainsKey("estadoRed") && m["estadoRed"].ToString() == "online"
        );

        if (algunaPrendida)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }
}
