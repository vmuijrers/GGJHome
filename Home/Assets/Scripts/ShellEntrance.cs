using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ShellEntrance : MonoBehaviour
{
    public Crab attachedCrab;
    public Shell shell;

    public bool isCrabAttached() {
        return attachedCrab != null;
    }

    public void Init(Shell shell) {
        this.shell = shell;
    }

    public void AttachCrab(Crab crab) {
        attachedCrab = crab;
    }

    public void DetachCrab(Crab crab) {
        attachedCrab = null;
    }
}