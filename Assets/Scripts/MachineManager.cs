using System.Collections.Generic;
using UnityEngine;

public class MachineManager : MonoBehaviour
{
    [System.Serializable]
    public class MachineBinding
    {
        public string machineId;
        public TimelineLoop timeline;

        public ChangeMaterial bombilloVerde;
        public ChangeMaterial bombilloRojo;
        public ChangeMaterial bombilloAzul;

        public ChangeMaterial startButton;
        public ChangeMaterial stopButton;

        public GameObject menuInicial;
        public GameObject startButtonUi;
        public GameObject[] otrosMenus;

        public bool manualOverride = false;
        public float overrideTimer = 0f;
    }

    public List<MachineBinding> machines;
    public float factorEscala = 20f;
    public float overrideTimeout = 15f;

    void Update()
    {
        if (MachineReader.Machines == null) return;

        foreach (var binding in machines)
        {
            foreach (var machine in MachineReader.Machines)
            {
                if (machine.TryGetValue("idMaquina", out object id) && id.ToString() == binding.machineId)
                {
                    string estadoMaquina = machine["estadoMaquina"].ToString();

                    if (binding.manualOverride)
                    {
                        binding.overrideTimer += Time.deltaTime;
                        if (binding.overrideTimer >= overrideTimeout)
                        {
                            binding.manualOverride = false;
                            binding.overrideTimer = 0f;
                        }
                    }

                    if (!binding.manualOverride)
                    {
                        if (estadoMaquina == "1")
                        {
                            binding.timeline.Encender();
                            binding.timeline.SetActivo(true);

                            binding.bombilloVerde.Prender();
                            binding.bombilloRojo.Apagar();
                            binding.bombilloAzul.Apagar();

                            binding.startButton.Prender();
                            binding.stopButton.Apagar();
                            binding.menuInicial.SetActive(true);
                            binding.startButtonUi.SetActive(false);
                            foreach (var menu in binding.otrosMenus)
                                menu.SetActive(false);
                        }
                        else if (estadoMaquina == "2")
                        {
                            binding.timeline.Apagar();
                            binding.timeline.SetActivo(false);

                            binding.bombilloAzul.Prender();
                            binding.bombilloVerde.Apagar();
                            binding.bombilloRojo.Apagar();

                            binding.startButtonUi.SetActive(true);
                            binding.startButton.Apagar();
                            binding.stopButton.Prender();
                            binding.menuInicial.SetActive(false);
                            foreach (var menu in binding.otrosMenus)
                                menu.SetActive(false);
                        }
                        else
                        {
                            binding.timeline.Apagar();
                            binding.timeline.SetActivo(false);

                            binding.bombilloRojo.Prender();
                            binding.bombilloVerde.Apagar();
                            binding.bombilloAzul.Apagar();

                            binding.startButtonUi.SetActive(true);
                            binding.startButton.Apagar();
                            binding.stopButton.Prender();
                            binding.menuInicial.SetActive(false);
                            foreach (var menu in binding.otrosMenus)
                                menu.SetActive(false);
                        }

                        if (machine.TryGetValue("paquetesMaquinaMinuto", out object ppmObj))
                        {
                            if (float.TryParse(ppmObj.ToString(), out float ppm))
                            {
                                binding.timeline.velocidad = Mathf.Clamp(2f + (ppm - 50f) / 50f, 2f, 3f);
                            }
                        }
                    }

                    break;
                }
            }
        }
    }

    public void EncenderManual(string machineId)
    {
        var binding = machines.Find(m => m.machineId == machineId);
        if (binding == null) return;

        binding.manualOverride = true;
        binding.overrideTimer = 0f;

        binding.bombilloVerde.Apagar();
        binding.bombilloRojo.Apagar();
        binding.bombilloAzul.Apagar();
        binding.startButton.Apagar();
        binding.stopButton.Apagar();

        binding.timeline.Encender();
        binding.timeline.SetActivo(true);
        binding.bombilloVerde.Prender();
        binding.startButton.Prender();
        binding.stopButton.Apagar();
        binding.menuInicial.SetActive(true);
        binding.startButtonUi.SetActive(false);
        foreach (var menu in binding.otrosMenus)
            menu.SetActive(false);
    }

    public void ApagarManual(string machineId)
    {
        var binding = machines.Find(m => m.machineId == machineId);
        if (binding == null) return;

        binding.manualOverride = true;
        binding.overrideTimer = 0f;

        binding.bombilloVerde.Apagar();
        binding.bombilloRojo.Apagar();
        binding.bombilloAzul.Apagar();
        binding.startButton.Apagar();
        binding.stopButton.Apagar();

        binding.timeline.Apagar();
        binding.timeline.SetActivo(false);
        binding.bombilloRojo.Prender();
        binding.startButtonUi.SetActive(true);
        binding.startButton.Apagar();
        binding.stopButton.Prender();
        binding.menuInicial.SetActive(false);
        foreach (var menu in binding.otrosMenus)
            menu.SetActive(false);
    }

    public void ResetOverrideTimer(string machineId)
    {
        var binding = machines.Find(m => m.machineId == machineId);
        if (binding != null)
        {
            binding.overrideTimer = 0f;
        }
    }
}
