using Hakoniwa.Core;
using Hakoniwa.Core.Simulation;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Hakoniwa.GUI
{
    public class SimStart : MonoBehaviour
    {
        Text my_text;
        Button my_btn;
        private enum SimCommandStatus
        {
            Start = 0,
            Stop = 2,
            Reset = 3,
        }
        private SimCommandStatus cmd_status = SimCommandStatus.Start;

        void Start()
        {
            var obj = GameObject.Find("StartButton");
            my_btn = obj.GetComponentInChildren<Button>();
            my_text = obj.GetComponentInChildren<Text>();
            my_btn.interactable = false;
        }
        void Update()
        {
            SimulationController simulator = SimulationController.Get();
            var state = simulator.GetState();
            if (cmd_status == SimCommandStatus.Stop)
            {
                if (state == SimulationState.Running)
                {
                    my_btn.interactable = true;
                }
            }
            else if (cmd_status == SimCommandStatus.Reset)
            {
                if (state == SimulationState.Stopped)
                {
                    my_btn.interactable = true;
                }
            }
            else if (cmd_status == SimCommandStatus.Start)
            {
                int count = simulator.asset_mgr.RefOutsideAssetList().Count;
                if ((count > 0) && (state == SimulationState.Stopped))
                {
                    my_btn.interactable = true;
                }
            }
        }
        public void OnButtonClick()
        {
            SimulationController simulator = SimulationController.Get();
            switch (cmd_status)
            {
                case SimCommandStatus.Stop:
                    simulator.Stop();
                    cmd_status = SimCommandStatus.Reset;
                    my_text.text = "リセット";
                    my_btn.interactable = false; 
                    break;
                case SimCommandStatus.Reset:
                    simulator.Reset();
                    cmd_status = SimCommandStatus.Start;
                    my_text.text = "開始";
                    my_btn.interactable = false;
                    break;
                case SimCommandStatus.Start:
                    simulator.Start();
                    cmd_status = SimCommandStatus.Stop;
                    my_text.text = "停止";
                    my_btn.interactable = false;
                    break;
                default:
                    break;
            }
        }
    }
}
