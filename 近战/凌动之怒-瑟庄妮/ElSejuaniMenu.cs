﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using System.Drawing;


namespace ElSejuani
{

    public class ElSejuaniMenu
    {
        public static Menu _menu;

        public static void Initialize()
        {
            _menu = new Menu("凌动之怒-瑟庄妮", "menu", true);

            //ElSejuani.Orbwalker
            var orbwalkerMenu = new Menu("Orbwalker", "orbwalker");
            Sejuani.Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);

            _menu.AddSubMenu(orbwalkerMenu);

            //ElSejuani.TargetSelector
            var targetSelector = new Menu("Target Selector", "TargetSelector");
            TargetSelector.AddToMenu(targetSelector);

            _menu.AddSubMenu(targetSelector);

            var cMenu = new Menu("Combo", "Combo");
            cMenu.AddItem(new MenuItem("ElSejuani.Combo.Q", "Use Q").SetValue(true));
            cMenu.AddItem(new MenuItem("ElSejuani.Combo.W", "Use W").SetValue(true));
            cMenu.AddItem(new MenuItem("ElSejuani.Combo.E", "Use E").SetValue(true));
            cMenu.AddItem(new MenuItem("ElSejuani.Combo.R", "Use R").SetValue(true));
            cMenu.AddItem(new MenuItem("ElSejuani.Combo.Ignite", "Use Ignite").SetValue(true));
            cMenu.AddItem(new MenuItem("ElSejuani.ssssssssssss", ""));
            cMenu.AddItem(new MenuItem("ElSejuani.Combo.R.Count", "Minimum targets for R >=").SetValue(new Slider(2, 1, 5)));
            cMenu.AddItem(new MenuItem("ElSejuani.Combo.E.Count", "Minimum targets for E >=").SetValue(new Slider(1, 1, 5)));
            cMenu.AddItem(new MenuItem("ComboActive", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));

            _menu.AddSubMenu(cMenu);

            var hMenu = new Menu("Harass", "Harass");
            hMenu.AddItem(new MenuItem("ElSejuani.Harass.Q", "Use Q").SetValue(true));
            hMenu.AddItem(new MenuItem("ElSejuani.Harass.W", "Use Q").SetValue(true));
            hMenu.AddItem(new MenuItem("ElSejuani.Harass.E", "Use E").SetValue(true));
            hMenu.AddItem(new MenuItem("ElSejuani.harass.mana", "Minimum mana for harass >=")).SetValue(new Slider(55));

            _menu.AddSubMenu(hMenu);

            var lMenu = new Menu("Clear", "Clear");
            lMenu.AddItem(new MenuItem("ElSejuani.Clear.Q", "Use Q").SetValue(true));
            lMenu.AddItem(new MenuItem("ElSejuani.Clear.W", "Use W").SetValue(true));
            lMenu.AddItem(new MenuItem("ElSejuani.Clear.E", "Use E").SetValue(true));
            lMenu.AddItem(new MenuItem("useEFarmddssd", ""));
            lMenu.AddItem(new MenuItem("ElSejuani.Clear.Q.Count", "Minimum targets for Q >=").SetValue(new Slider(1, 1, 5)));
            lMenu.AddItem(new MenuItem("minmanaclear", "Minimum mana to clear >=")).SetValue(new Slider(55));

            _menu.AddSubMenu(lMenu);

            //ElSejuani.Interupt
            var interuptMenu = new Menu("Interupt settings", "interuptsettings");
            interuptMenu.AddItem(new MenuItem("ElSejuani.Interupt.Q", "Use Q").SetValue(true));
            interuptMenu.AddItem(new MenuItem("ElSejuani.Interupt.R", "Use R").SetValue(false));

            _menu.AddSubMenu(interuptMenu);

            //ElCorki.Misc
            var miscMenu = new Menu("Misc", "Misc");
            miscMenu.AddItem(new MenuItem("ElSejuani.Draw.off", "Turn drawings off").SetValue(false));
            miscMenu.AddItem(new MenuItem("ElSejuani.Draw.Q", "Draw Q").SetValue(new Circle()));
            miscMenu.AddItem(new MenuItem("ElSejuani.Draw.W", "Draw W").SetValue(new Circle()));
            miscMenu.AddItem(new MenuItem("ElSejuani.Draw.E", "Draw E").SetValue(new Circle()));
            miscMenu.AddItem(new MenuItem("ElSejuani.Draw.R", "Draw R").SetValue(new Circle()));
            miscMenu.AddItem(new MenuItem("useEdaadaDFafsdsgdrmddsddsasfsasdsdsaadsd", ""));
            miscMenu.AddItem(new MenuItem("ElSejuani.hitChance", "Hitchance Q").SetValue(new StringList(new[] { "Low", "Medium", "High", "Very High" }, 3)));

            _menu.AddSubMenu(miscMenu);

            //Here comes the moneyyy, money, money, moneyyyy
            var credits = new Menu("Credits", "jQuery");
            credits.AddItem(new MenuItem("ElCorki.Paypal", "if you would like to donate via paypal:"));
            credits.AddItem(new MenuItem("ElCorki.Email", "info@zavox.nl"));
            _menu.AddSubMenu(credits);

            _menu.AddItem(new MenuItem("422442fsaafs4242f", ""));
            _menu.AddItem(new MenuItem("422442fsaafsf", "Version: 1.0.0.1"));
            _menu.AddItem(new MenuItem("fsasfafsfsafsa", "Made By jQuery"));

            _menu.AddToMainMenu();

            Console.WriteLine("Menu Loaded");
        }
    }
}