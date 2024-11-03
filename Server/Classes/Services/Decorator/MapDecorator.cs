﻿using Server.GameWorld;
using SharedLibs;

namespace Server.Classes.Services.Decorator
{
    public class MapDecorator
    {
        List<Addon> Addons;
        public MapDecorator(List<Addon> addons)
        {
            Addons = addons;
        }
        public MapDecorator() 
        { 
            Addons = new List<Addon>();
        }
        public List<Addon> GetAllAddons()
        {
            Console.WriteLine(Addons.Count);
            return Addons;
        }
        public void Remove(Addon addon)
        {
            Addons.Remove(addon);
        }
        public void RemoveAll()
        {
            Addons = new List<Addon>();
        }
        public void UpdateAddon(Addon toUpdate)
        {
            Console.WriteLine("Currently in decorator");
            if (Addons.Contains(toUpdate)) Addons.Find(a => a.GetType() == toUpdate.GetType()).SetValueFromString(toUpdate.GetValue());
            else Addons.Add(toUpdate);
            Console.WriteLine("Added addon");
        }

    }
}
