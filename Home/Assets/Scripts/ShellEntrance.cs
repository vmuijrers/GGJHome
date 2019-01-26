using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ShellEntrance : MonoBehaviour
{
    public CrabMove attachedCrab;
    private Shell shell;

    public bool isCrabAttached() {
        return attachedCrab != null;
    }

    public void Init(Shell shell) {
        this.shell = shell;
    }

    public void AttachCrab(CrabMove crab) {
        attachedCrab = crab;
    }

    public void DetachCrab(CrabMove crab) {
        attachedCrab = null;
    }
}