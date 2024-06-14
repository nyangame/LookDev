using System.Collections;
using System.Collections.Generic;
using CreateSenbei;
using UnityEngine;

public class ApplicationManager
{
    private static ApplicationManager _instance = new ApplicationManager();
    public static ApplicationManager Instance => _instance;
    private ApplicationManager(){}

    public SenbeiModel SenbeiModel { get; set; }
}
