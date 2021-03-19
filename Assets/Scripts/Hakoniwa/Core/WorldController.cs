using Hakoniwa.Core.Rpc;
using Hakoniwa.Core.Simulation;
using Hakoniwa.Core.Simulation.Environment;
using Hakoniwa.Core.Utils;
using Hakoniwa.PluggableAsset;
using Hakoniwa.PluggableAsset.Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakoniwa.Core
{
    class UnitySimulator : IInsideWorldSimulatior
    {
        public UnitySimulator()
        {

        }
        public void DoSimulation()
        {
            Physics.Simulate(Time.fixedDeltaTime);
        }
    }

    public class WorldController : MonoBehaviour
    {
        private GameObject root;
        public long maxDelayTime = 20000; /* usec */
        private SimulationController simulator = SimulationController.Get();
        void Start()
        {
            this.root = GameObject.Find("Robot");

            HakoniwaConfig cfg = GameObject.Find("Hakoniwa").GetComponentInChildren<HakoniwaConfig>();
            cfg.Initialize();

            if (cfg.cfg.CoreIpAddr != null)
            {
                Debug.Log("HakoniwaCore START");
                RpcServer.StartServer(cfg.cfg.CoreIpAddr, cfg.cfg.CorePort);
                simulator.RegisterEnvironmentOperation(new UnityEnvironmentOperation());
                simulator.SaveEnvironment();
                simulator.GetLogger().SetFilePath(cfg.cfg.SymTimeMeasureFilePath);
            }
            else
            {
                Debug.LogError("HakoniwaCore NONE");
            }
            AssetConfiguration.Load();
            foreach (Transform child in this.root.transform)
            {
                Debug.Log("child=" + child.name);
                GameObject obj = root.transform.Find(child.name).gameObject;
                IInsideAssetController ctrl = obj.GetComponentInChildren<IInsideAssetController>();
                AssetConfiguration.AddInsideAsset(ctrl);
                simulator.asset_mgr.RegisterInsideAsset(child.name);

                ctrl.Initialize();
            }

            simulator.SetSimulationWorldTime(
                this.maxDelayTime,
                (long)(Time.fixedDeltaTime * 1000000f));
            simulator.SetInsideWorldSimulator(new UnitySimulator());
            Physics.autoSimulation = false;
        }
        void FixedUpdate()
        {
            this.simulator.Execute();
        }
    }
}
