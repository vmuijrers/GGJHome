using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ShellEntrance : MonoBehaviour
{
    public List<Crab> attachedCrabs = new List<Crab>();
    public Shell shell;

    public void Init(Shell shell) {
        this.shell = shell;
    }

    public void AttachCrab(Crab crab) {
        if (!attachedCrabs.Contains(crab)) {
            attachedCrabs.Add(crab);
            shell.UpdateCrabVisuals(attachedCrabs.Count);
        }
    }

    public void DetachCrab(Crab crab) {
        if (attachedCrabs.Contains(crab)) {
            attachedCrabs.Remove(crab);
            shell.UpdateCrabVisuals(attachedCrabs.Count);
        }
    }
}