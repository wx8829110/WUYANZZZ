﻿#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using LeagueSharp.Common.Data;
using ItemData = LeagueSharp.Common.Data.ItemData;

#endregion

namespace ChewyMoonsShaco
{
    internal class ChewyMoonShaco
    {
        public static Spell Q;
        public static Spell W;
        public static Spell E;
        public static Menu Menu;
        public static Orbwalking.Orbwalker Orbwalker;
        public static List<Spell> SpellList;
        public static Items.Item Tiamat;
        public static Items.Item Hydra;

        public static void OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.BaseSkinName != "Shaco")
            {
                return;
            }

            Q = new Spell(SpellSlot.Q, 400);
            W = new Spell(SpellSlot.W, 425);
            E = new Spell(SpellSlot.E, 625);

            SpellList = new List<Spell> { Q, E, W };

            CreateMenu();
            Illuminati.Init();

            Tiamat = ItemData.Tiamat_Melee_Only.GetItem();
            Hydra = ItemData.Ravenous_Hydra_Melee_Only.GetItem();

            Game.OnUpdate += GameOnOnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Orbwalking.AfterAttack += OrbwalkingOnAfterAttack;

            Game.PrintChat(
                "<font color=\"#6699ff\"><b>ChewyMoon's Shaco:</b></font> <font color=\"#FFFFFF\">" + "loaded!" +
                "</font>");
        }

        private static void OrbwalkingOnAfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (!unit.IsMe)
            {
                return;
            }

            if (!(target is Obj_AI_Hero))
            {
                return;
            }

            if (!target.IsValidTarget())
            {
                return;
            }

            if (Hydra.IsReady())
            {
                Hydra.Cast();
            }
            else if (Tiamat.IsReady())
            {
                Tiamat.Cast();
            }
        }

        private static void CreateMenu()
        {
            (Menu = new Menu("恶魔小丑-萨科", "cmShaco", true)).AddToMainMenu();

            // Target Selector
            var tsMenu = new Menu("Target Selector", "cmShacoTS");
            TargetSelector.AddToMenu(tsMenu);
            Menu.AddSubMenu(tsMenu);

            // Orbwalking
            var orbwalkingMenu = new Menu("Orbwalking", "cmShacoOrbwalkin");
            Orbwalker = new Orbwalking.Orbwalker(orbwalkingMenu);
            Menu.AddSubMenu(orbwalkingMenu);

            // Combo
            var comboMenu = new Menu("Combo", "cmShacoCombo");
            comboMenu.AddItem(new MenuItem("useQ", "Use Q").SetValue(true));
            comboMenu.AddItem(new MenuItem("useW", "Use W").SetValue(true));
            comboMenu.AddItem(new MenuItem("useE", "Use E").SetValue(true));
            comboMenu.AddItem(new MenuItem("useItems", "Use items").SetValue(true));
            Menu.AddSubMenu(comboMenu);

            // Harass
            var harassMenu = new Menu("Harass", "cmShacoHarass");
            harassMenu.AddItem(new MenuItem("useEHarass", "Use E").SetValue(true));
            Menu.AddSubMenu(harassMenu);

            // Ks
            var ksMenu = new Menu("KS", "cmShacoKS");
            ksMenu.AddItem(new MenuItem("ksE", "Use E").SetValue(true));
            Menu.AddSubMenu(ksMenu);

            // ILLUMINATI
            var illuminatiMenu = new Menu("Illuminati", "cmShacoTriangleIlluminatiSp00ky");
            illuminatiMenu.AddItem(new MenuItem("PlaceBox", "Place Box").SetValue(new KeyBind(73, KeyBindType.Press)));
            illuminatiMenu.AddItem(
                new MenuItem("RepairTriangle", "Repair Triangle & Auto Form Triangle").SetValue(true));
            illuminatiMenu.AddItem(new MenuItem("BoxDistance", "Box Distance").SetValue(new Slider(600, 101, 1200)));

            illuminatiMenu.Item("BoxDistance").ValueChanged +=
                delegate(object sender, OnValueChangeEventArgs args)
                {
                    Illuminati.TriangleLegDistance = args.GetNewValue<Slider>().Value;
                };

            Menu.AddSubMenu(illuminatiMenu);

            // Drawing
            var drawingMenu = new Menu("Drawings", "cmShacoDrawing");
            drawingMenu.AddItem(new MenuItem("drawQ", "Draw Q").SetValue(true));
            drawingMenu.AddItem(new MenuItem("drawQPos", "Draw Q Pos").SetValue(true));
            drawingMenu.AddItem(new MenuItem("drawW", "Draw W").SetValue(true));
            drawingMenu.AddItem(new MenuItem("drawE", "Draw E").SetValue(true));
            Menu.AddSubMenu(drawingMenu);

            // Misc
            var miscMenu = new Menu("Misc", "cmShacoMisc");
            miscMenu.AddItem(new MenuItem("usePackets", "Use packets").SetValue(true));
            miscMenu.AddItem(new MenuItem("stuff", "Let me know of any"));
            miscMenu.AddItem(new MenuItem("stuff2", "other misc features you want"));
            miscMenu.AddItem(new MenuItem("stuff3", "on the thread or IRC"));
            Menu.AddSubMenu(miscMenu);
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            var qCircle = Menu.Item("drawQ").GetValue<bool>();
            var wCircle = Menu.Item("drawW").GetValue<bool>();
            var eCircle = Menu.Item("drawE").GetValue<bool>();
            var qPosCircle = Menu.Item("drawQPos").GetValue<bool>();

            var pos = ObjectManager.Player.Position;

            if (qCircle)
            {
                Render.Circle.DrawCircle(pos, Q.Range, Q.IsReady() ? Color.Aqua : Color.Red);
            }

            if (wCircle)
            {
                Render.Circle.DrawCircle(pos, W.Range, W.IsReady() ? Color.Aqua : Color.Red);
            }

            if (eCircle)
            {
                Render.Circle.DrawCircle(pos, E.Range, E.IsReady() ? Color.Aqua : Color.Red);
            }

            if (!qPosCircle)
            {
                return;
            }

            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.IsValidTarget()))
            {
                Drawing.DrawLine(
                    Drawing.WorldToScreen(enemy.Position), Drawing.WorldToScreen(ShacoUtil.GetQPos(enemy, false)), 2,
                    Color.Aquamarine);
            }
        }

        private static void GameOnOnGameUpdate(EventArgs args)
        {
            if (Menu.Item("ksE").GetValue<bool>())
            {
                KillSecure();
            }

            if (Menu.Item("PlaceBox").IsActive())
            {
                Illuminati.PlaceInitialBox();
            }

            switch (Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Combo();
                    break;

                case Orbwalking.OrbwalkingMode.Mixed:
                    Harass();
                    break;
            }
        }

        private static void KillSecure()
        {
            if (!E.IsReady())
            {
                return;
            }

            foreach (var target in
                ObjectManager.Get<Obj_AI_Hero>()
                    .Where(x => x.IsEnemy)
                    .Where(x => !x.IsDead)
                    .Where(x => x.Distance(ObjectManager.Player) <= E.Range)
                    .Where(target => ObjectManager.Player.GetSpellDamage(target, SpellSlot.E) > target.Health))
            {
                E.CastOnUnit(target, Menu.Item("usePackets").GetValue<bool>());
                return;
            }
        }

        private static void Combo()
        {
            var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);

            var useQ = Menu.Item("useQ").GetValue<bool>();
            var useW = Menu.Item("useW").GetValue<bool>();
            var useE = Menu.Item("useE").GetValue<bool>();
            var packets = Menu.Item("usePackets").GetValue<bool>();

            foreach (var spell in SpellList.Where(x => x.IsReady()))
            {
                if (spell.Slot == SpellSlot.Q && useQ)
                {
                    if (!target.IsValidTarget(Q.Range))
                    {
                        continue;
                    }

                    var pos = ShacoUtil.GetQPos(target, true);
                    Q.Cast(pos, packets);
                }

                if (spell.Slot == SpellSlot.W && useW)
                {
                    //TODO: Make W based on waypoints
                    if (!target.IsValidTarget(W.Range))
                    {
                        continue;
                    }

                    var pos = ShacoUtil.GetQPos(target, true, 100);
                    W.Cast(pos, packets);
                }

                if (spell.Slot != SpellSlot.E || !useE)
                {
                    continue;
                }
                if (!target.IsValidTarget(E.Range))
                {
                    continue;
                }

                E.CastOnUnit(target);
            }
        }

        private static void Harass()
        {
            var useE = Menu.Item("useEHarass").GetValue<bool>();
            var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Magical);

            if (!target.IsValidTarget(E.Range))
            {
                return;
            }

            if (useE && E.IsReady())
            {
                E.CastOnUnit(target);
            }
        }
    }
}