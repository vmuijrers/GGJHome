using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ShellEntrance : MonoBehaviour
{
    public List<Crab> attachedCrabs = new List<Crab>();
    public Shell shell;
    public int maxCrabs = 2;
    public void Init(Shell shell) {
        this.shell = shell;
    }

    public void Update() {
        attachedCrabs.ForEach(x => {
            x.transform.rotation = Quaternion.LookRotation(transform.forward);
            x.transform.position = transform.position;

            }
        );
    }
    public bool AttachCrab(Crab crab) {
        if (!attachedCrabs.Contains(crab) && attachedCrabs.Count < maxCrabs) {
            attachedCrabs.Add(crab);
            shell.UpdateCrabVisuals(attachedCrabs.Count);
            return true;
        }
        return false;
    }

    public void DetachCrab(Crab crab) {
        if (attachedCrabs.Contains(crab)) {
            attachedCrabs.Remove(crab);
            shell.UpdateCrabVisuals(attachedCrabs.Count);
        }
    }
}