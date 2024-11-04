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
        PelletShapes = new List<string> { "Default", "Hexagon", "Square", "Triangle" };
        Indices = new List<int> { 0, 0, 0 };
    }
    public void SetAddons(List<Addon> addons)
    {
        Debug.Log(addons.Count);
        foreach(var addon in addons)
        {
            var currentAddon = Addons.Find(a => a.GetType() == addon.GetType());
            currentAddon.SetValueFromString(addon.GetValue());
            int index = Addons.IndexOf(currentAddon);
            Debug.Log(index);
            MainThreadDispatcher.Instance().Enqueue(() =>
            {
                AddonTexts[index].text = addon.GetValue();
            });
        }
    }

    public void Increase(int whichField)
    {
        Indices[whichField] = (Indices[whichField] + 1) % 4;
        string newValue;
        if (whichField == 0) newValue = PelletColors[Indices[whichField]].ToString();
        else if (whichField == 1) newValue = PelletShapes[Indices[whichField]].ToString();
        else newValue = WallColors[Indices[whichField]].ToString();
        Debug.Log("Setting new value for " + whichField + ". The value: " + newValue);
        Addons[whichField].SetValueFromString(newValue);
        GameObject.Find("SignalR").GetComponent<SignalRConnector>().SetAddonInServer(Addons[whichField]);
        
    }
    public void Decrease(int whichField)
    {
        Indices[whichField] = (Indices[whichField] - 1) % 4;
        string newValue;
        if (whichField == 0) newValue = PelletColors[Indices[whichField]].ToString();
        else if (whichField == 1) newValue = PelletShapes[Indices[whichField]].ToString();
        else newValue = WallColors[Indices[whichField]].ToString();
        Addons[whichField].SetValueFromString(newValue);
        GameObject.Find("SignalR").GetComponent<SignalRConnector>().SetAddonInServer(Addons[whichField]);
    }
}
