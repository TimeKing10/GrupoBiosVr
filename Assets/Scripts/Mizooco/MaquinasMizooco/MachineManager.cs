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

        public GenerarAlimento generador;

        public bool manualOverride = false;
        public float overrideTimer = 0f;

        [HideInInspector] public string previousProducto = "";
    }

    public List<MachineBinding> machines;
    public float factorEscala = 20f;
    public float overrideTimeout = 15f;

    void Awake()
    {
        AutoAssignGeneradoresByMachineId();
    }

    void Start()
    {
        LogGeneradoresAsignados();
    }

    void AutoAssignGeneradoresByMachineId()
    {
        var gens = FindObjectsOfType<GenerarAlimento>();
        if (gens == null || gens.Length == 0) return;

        foreach (var binding in machines)
        {
            if (binding == null) continue;
            if (binding.generador != null) continue;

            GenerarAlimento match = null;
            foreach (var g in gens)
            {
                if (!string.IsNullOrEmpty(g.machineId) && g.machineId == binding.machineId)
                {
                    match = g;
                    break;
                }
            }

            if (match == null)
            {
                foreach (var g in gens)
                {
                    if (g.gameObject.name.Contains(binding.machineId))
                    {
                        match = g;
                        break;
                    }
                }
            }

            if (match != null)
            {
                binding.generador = match;
                
            }
            else
            {
                
            }
        }
    }

    void LogGeneradoresAsignados()
    {
        foreach (var b in machines)
        {
            string genName = b.generador != null ? b.generador.gameObject.name : "NONE";
            
        }
    }

    void Update()
    {
        
        
        if (MachineReader.Machines == null) return;

        foreach (var binding in machines)
        {
            
            foreach (var machine in MachineReader.Machines)
            {
                if (machine.TryGetValue("idMaquina", out object id) && id.ToString() == binding.machineId)
                {
                    string estadoMaquina = machine.GetValueOrDefault("estadoMaquina", "").ToString();

                    string productoDescripcion = "";
                    if (machine.TryGetValue("productoDescripcion", out object prodObj))
                        productoDescripcion = prodObj?.ToString() ?? "";

                    if (binding.generador != null && !binding.manualOverride)
                    {
                        if (!string.IsNullOrEmpty(productoDescripcion) && productoDescripcion != binding.previousProducto)
                        {

                            binding.generador.SeleccionarPorDescripcion(productoDescripcion);
                            binding.previousProducto = productoDescripcion;

                        }
                    }

                    if (binding.manualOverride)
                    {
                        binding.overrideTimer += Time.deltaTime;
                        if (binding.overrideTimer >= overrideTimeout)
                        {
                            binding.manualOverride = false;
                            binding.overrideTimer = 0f;

                            // 🔄 Forzar que el generador vuelva al JSON
                            if (binding.generador != null)
                            {
                                binding.generador.ActualizarDesdeJSON();
                            }

                            Debug.Log($"⏱ Override terminado → {binding.machineId} vuelve al JSON");
                        }
                    }

                    if (!binding.manualOverride)
                    {
                        if (estadoMaquina == "1")
                        {
                            binding.timeline?.Encender();
                            binding.timeline?.SetActivo(true);

                            binding.bombilloVerde?.Prender();
                            binding.bombilloRojo?.Apagar();
                            binding.bombilloAzul?.Apagar();

                            binding.startButton?.Prender();
                            binding.stopButton?.Apagar();
                            if (binding.menuInicial != null) binding.menuInicial.SetActive(true);
                            if (binding.startButtonUi != null) binding.startButtonUi.SetActive(false);
                            foreach (var menu in binding.otrosMenus) if (menu != null) menu.SetActive(false);
                        }
                        else if (estadoMaquina == "2")
                        {
                            binding.timeline?.Apagar();
                            binding.timeline?.SetActivo(false);

                            binding.bombilloAzul?.Prender();
                            binding.bombilloVerde?.Apagar();
                            binding.bombilloRojo?.Apagar();

                            if (binding.startButtonUi != null) binding.startButtonUi.SetActive(true);
                            binding.startButton?.Apagar();
                            binding.stopButton?.Prender();
                            if (binding.menuInicial != null) binding.menuInicial.SetActive(false);
                            foreach (var menu in binding.otrosMenus) if (menu != null) menu.SetActive(false);
                        }
                        else
                        {
                            binding.timeline?.Apagar();
                            binding.timeline?.SetActivo(false);

                            binding.bombilloRojo?.Prender();
                            binding.bombilloVerde?.Apagar();
                            binding.bombilloAzul?.Apagar();

                            if (binding.startButtonUi != null) binding.startButtonUi.SetActive(true);
                            binding.startButton?.Apagar();
                            binding.stopButton?.Prender();
                            if (binding.menuInicial != null) binding.menuInicial.SetActive(false);
                            foreach (var menu in binding.otrosMenus) if (menu != null) menu.SetActive(false);
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

        binding.bombilloVerde?.Apagar();
        binding.bombilloRojo?.Apagar();
        binding.bombilloAzul?.Apagar();
        binding.startButton?.Apagar();
        binding.stopButton?.Apagar();

        binding.timeline?.Encender();
        binding.timeline?.SetActivo(true);
        binding.bombilloVerde?.Prender();
        binding.startButton?.Prender();
        binding.stopButton?.Apagar();
        if (binding.menuInicial != null) binding.menuInicial.SetActive(true);
        if (binding.startButtonUi != null) binding.startButtonUi.SetActive(false);
        foreach (var menu in binding.otrosMenus) if (menu != null) menu.SetActive(false);
    }

    public void ApagarManual(string machineId)
    {
        var binding = machines.Find(m => m.machineId == machineId);
        if (binding == null) return;

        binding.manualOverride = true;
        binding.overrideTimer = 0f;

        binding.bombilloVerde?.Apagar();
        binding.bombilloRojo?.Apagar();
        binding.bombilloAzul?.Apagar();
        binding.startButton?.Apagar();
        binding.stopButton?.Apagar();

        binding.timeline?.Apagar();
        binding.timeline?.SetActivo(false);
        binding.bombilloRojo?.Prender();
        if (binding.startButtonUi != null) binding.startButtonUi.SetActive(true);
        binding.startButton?.Apagar();
        binding.stopButton?.Prender();
        if (binding.menuInicial != null) binding.menuInicial.SetActive(false);
        foreach (var menu in binding.otrosMenus) if (menu != null) menu.SetActive(false);
    }

    public void ResetOverrideTimer(string machineId)
{
    var binding = machines.Find(m => m.machineId == machineId);

    if (binding != null)
    {
        binding.manualOverride = true;
        binding.overrideTimer = 0f;
        
    }
    else
    {
        
        foreach (var b in machines)
            Debug.Log($"- {b.machineId}");
    }
}

    public void RegisterGenerador(GenerarAlimento gen)
    {
        if (gen == null || string.IsNullOrEmpty(gen.machineId)) return;
        var binding = machines.Find(m => m.machineId == gen.machineId);
        if (binding != null)
        {
            binding.generador = gen;
    
        }
    }
}
