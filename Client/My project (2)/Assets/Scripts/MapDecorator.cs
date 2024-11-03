using SharedLibs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MapDecorator : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> AddonTexts;
    private List<Addon> Addons;
    private List<string> WallColors;
    private List<string> PelletColors;
    private List<string> PelletShapes;
    private List<int> Indices;

    private void Start()
    {
        Addons = new List<Addon>
        {
            new MapPelletColor(),
            new MapPelletShape(),
            new MapWall()
        };
        Addons.ForEach(x => x.SetValueFromString("Default"));
        WallColors = new List<string> { "Default", "Red", "Green", "Yellow" };
        PelletColors = new List<string> { "Default", "Red", "Green", "White" };
        PelletShapes = new List<string> { "Default", "Red", "Green", "Yellow" };
        Indices = new List<int> { 0, 0, 0 };
    }
    public void SetAddons(List<Addon> addons)
    {
        foreach(var addon in addons)
        {
            Addons.Find(a => a.GetType() == addon.GetType()).SetValueFromString(addon.GetValue());
            MainThreadDispatcher.Instance().Enqueue(() =>
            {
                AddonTexts[Addons.IndexOf(addon)].text = addon.GetValue();
            });
        }
    }

    public void Increase(int whichField)
    {
        Indices[whichField] = (Indices[whichField] + 1) % 4;
        string newValue;
        if (whichField == 0) newValue = PelletColors[Indices[whichField]];
        else if (whichField == 1) newValue = PelletShapes[Indices[whichField]];
        else newValue = WallColors[Indices[whichField]];
        Addons[whichField].SetValueFromString(newValue);
        GameObject.Find("SignalR").GetComponent<SignalRConnector>().SetAddonInServer(Addons[whichField]);
        
    }
    public void Decrease(int whichField)
    {
        Indices[whichField] = (Indices[whichField] - 1) % 4;
        string newValue;
        if (whichField == 0) newValue = PelletColors[Indices[whichField]];
        else if (whichField == 1) newValue = PelletShapes[Indices[whichField]];
        else newValue = WallColors[Indices[whichField]];
        Addons[whichField].SetValueFromString(newValue);
        GameObject.Find("SignalR").GetComponent<SignalRConnector>().SetAddonInServer(Addons[whichField]);
    }
}
