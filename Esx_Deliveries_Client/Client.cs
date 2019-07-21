using CitizenFX.Core;
using CitizenFX.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;


namespace Esx_Deliveries_Client
{
    public class Client : BaseScript
    {
        public enum DELIVERY_STATE
        {
            DELIVERY_INACTIVE,
            PLAYER_STARTED_DELIVERY,
            PLAYER_REACHED_VEHICLE_POINT,
            PLAYER_REMOVED_GOODS_FROM_VEHICLE,
            PLAYER_REACHED_DELIVERY_POINT,
            PLAYER_RETURNING_TO_BASE
        };

        public enum DeliveryType { Scooter, Van, Truck, NotSelected };
        public enum SafeDepositReturn { YES, NO };

        public static DELIVERY_STATE m_delivery_state                   = DELIVERY_STATE.DELIVERY_INACTIVE;
        public static CitizenFX.Core.Vehicle m_delivery_vehicle         = null;

        public static List<Tuple<Vector3, Vector3>> m_delivery_route    = new List<Tuple<Vector3, Vector3>>();
        public static List<bool> m_delivery_point_complete              = new List<bool>();

        public static Blip m_active_blip                                = null;
        public static Tuple<Vector3, Vector3> m_active_delivery_point   = null;
        public static DeliveryType m_active_delivery_type               = DeliveryType.NotSelected;

        public static List<Prop> m_player_attatchments                  = new List<Prop>();
        public static List<Prop> m_vehicle_attatchments                 = new List<Prop>();

        public static Vector3 m_baselocation_coords                     = new Vector3(-314.0f, -1035.21f, 30.53f);
        public static Vector3 m_baselocation_scooter                    = new Vector3(-319.17f, -1032.14f, 30.53f);
        public static Vector3 m_baselocation_van                        = new Vector3(-323.17f, -1030.63f, 30.53f);
        public static Vector3 m_baselocation_truck                      = new Vector3(-326.75f, -1029.23f, 30.53f);

        public static Vector3 m_baselocation_return_point               = new Vector3(-271.3f, -1009.66f, 29.87f);
        public static Vector3 m_baselocation_deleter                    = new Vector3(-338.26f, -1023.18f, 30.38f);

        public static float m_baselocation_spawn_heading                = 245.0f;

        /* Skin variables */
        public static Dictionary<int, int> m_drawables                  = new Dictionary<int, int>();
        public static Dictionary<int, int> m_drawableTextures           = new Dictionary<int, int>();

        private static void SavePlayerSkin()
        {
            m_drawables.Clear();
            m_drawableTextures.Clear();

            //Save Ped skin
            for (var i = 0; i < 21; i++)
            {
                int drawable = GetPedDrawableVariation(Game.PlayerPed.Handle, i);
                int textureVariation = GetPedTextureVariation(Game.PlayerPed.Handle, i);
                m_drawables.Add(i, drawable);
                m_drawableTextures.Add(i, textureVariation);
            }


        }

        private static void LoadWorkPlayerSkin(DeliveryType aDeliveryType)
        {
            bool isPedFemale = IsPedModel(Game.PlayerPed.Handle, (uint)GetHashKey("mp_f_freemode_01"));

            var drawables = DeliveryData.OutfitScooter_drawables;
            var drawableTextures = DeliveryData.OutfitScooter_drawableTextures;

            if (isPedFemale)
            {
                drawables = DeliveryData.OutfitScooter_drawables_f;
                drawableTextures = DeliveryData.OutfitScooter_drawableTextures_f;
            }

            if (aDeliveryType != DeliveryType.Scooter)
            {
                drawables = DeliveryData.OutfitVan_drawables;
                drawableTextures = DeliveryData.OutfitVan_drawableTextures;

                if (isPedFemale)
                {
                    drawables = DeliveryData.OutfitVan_drawables_f;
                    drawableTextures = DeliveryData.OutfitVan_drawableTextures_f;
                }

            }

            foreach (var x in drawables)
            {

                SetPedComponentVariation(Game.PlayerPed.Handle, x.Key, x.Value, drawableTextures[x.Key], 1);
            }
        }

        private static void LoadDefaultPlayerSkin()
        {
            var ped = Game.PlayerPed.Handle;
            CitizenFX.Core.Debug.WriteLine(":: Loading skin drawables : " + m_drawables.Count.ToString());
            CitizenFX.Core.Debug.WriteLine(":: Loading skin drawableTextures : " + m_drawableTextures.Count.ToString());

            for (var drawable = 0; drawable < 21; drawable++)
            {
                //Citizen FX.Core.Debug.WriteLine(":: Loading skin : " + m_drawables[drawable].ToString() + " : " + m_drawableTextures[drawable].ToString());
                SetPedComponentVariation(ped, drawable, m_drawables[drawable], m_drawableTextures[drawable], 1);
            }
        }
        


        public Client()
        {
            /* Map blip creation */
            dynamic blip = AddBlipForCoord(m_baselocation_coords.X, m_baselocation_coords.Y, m_baselocation_coords.Z);
            SetBlipSprite(blip, 85);
            SetBlipColour(blip, 5);
            BeginTextCommandSetBlipName("STRING");
            AddTextComponentString("Deliveries Hub");
            EndTextCommandSetBlipName(blip);

            DeliveryData.DecorCode                      = GetConvarInt("esx_deliveries_decorcode", 1450);

            DeliveryData.DELIVERIES_MIN                 = GetConvarInt("esx_deliveries_min", 5);
            DeliveryData.DELIVERIES_MAX                 = GetConvarInt("esx_deliveries_max", 7);

            DeliveryData.DELIVERIES_REWARD_SCOOTER      = GetConvarInt("esx_deliveries_reward_scooter", 750);
            DeliveryData.DELIVERIES_REWARD_VAN          = GetConvarInt("esx_deliveries_reward_van", 1000);
            DeliveryData.DELIVERIES_REWARD_TRUCK        = GetConvarInt("esx_deliveries_reward_truck", 1450);

            DeliveryData.SAFE_DEPOSIT_SCOOTER           = GetConvarInt("esx_deliveries_safe_deposit_scooter", 4000);
            DeliveryData.SAFE_DEPOSIT_VAN               = GetConvarInt("esx_deliveries_safe_deposit_van", 6000);
            DeliveryData.SAFE_DEPOSIT_TRUCK             = GetConvarInt("esx_deliveries_safe_deposit_truck", 8000);

            Tick += HandleInput;
            Tick += HandleLogic;
            Tick += HandleMarkers;

        }

        private async Task HandleInput()
        {
            try
            {
                if (m_delivery_state == DELIVERY_STATE.PLAYER_REMOVED_GOODS_FROM_VEHICLE)
                {
                    Game.DisableControlThisFrame(0, Control.Sprint);
                    DisableControlAction(0, 21, true);
                } else {
                    await Delay(500);
                }
            } catch (Exception e)
            {
                CitizenFX.Core.Debug.WriteLine($"{e.Message} : Exception thrown on HandleInput()");
            }

        }

        private async Task HandleLogic()
        {
            try
            {
                if (m_delivery_state != DELIVERY_STATE.DELIVERY_INACTIVE)
                {

                    /* Check Vehicle and Player health */
                    if (IsPedDeadOrDying(Game.PlayerPed.Handle, true))
                    {
                        FinishDelivery(m_active_delivery_type, SafeDepositReturn.NO);
                        return;
                    }

                    if (GetVehicleEngineHealth(m_delivery_vehicle.Handle) < 20 && m_delivery_vehicle != null)
                    {
                        FinishDelivery(m_active_delivery_type, SafeDepositReturn.NO);
                        return;
                    }

                    var playerPed = Game.PlayerPed;
                    Vector3 pCoords = Game.PlayerPed.Position;


                    /* Moving towards vehicle delivery point logic */
                    if (m_delivery_state == DELIVERY_STATE.PLAYER_STARTED_DELIVERY)
                    {
                        /* Check if the player is still inside the delivery vehicle */
                        if (!isPlayerInsideDeliveryVehicle())
                        {
                            Screen.ShowSubtitle(DeliveryData._U["GET_BACK_IN_VEHICLE"]);
                        }

                        /* Check if player reached the vehicle delivery point */
                        if (World.GetDistance(pCoords, m_active_delivery_point.Item1) < 1.5f)
                        {
                            m_delivery_state = DELIVERY_STATE.PLAYER_REACHED_VEHICLE_POINT;
                            PlaySound(-1, "Menu_Accept", "Phone_SoundSet_Default", false, 0, true);
                        }
                    }

                    /* Make player grab the props for delivery */
                    if (m_delivery_state == DELIVERY_STATE.PLAYER_REACHED_VEHICLE_POINT)
                    {

                    }

                    /* Delivering the goods logic */
                    if (m_delivery_state == DELIVERY_STATE.PLAYER_REMOVED_GOODS_FROM_VEHICLE)
                    {
                        /* Display delivery hint for vans and trucks */
                        if (m_active_delivery_type == DeliveryType.Van || m_active_delivery_type == DeliveryType.Truck)
                        {
                            Screen.ShowSubtitle(DeliveryData._U["DELIVER_INSIDE_SHOP"]);

                            if (m_active_delivery_type == DeliveryType.Van && !IsEntityPlayingAnim(Game.PlayerPed.Handle, "anim@heists@box_carry@", "walk", 3))
                            {
                                ForceCarryAnimation();
                            }

                        }

                        /* Check if player reached the delivery point */
                        if (World.GetDistance(pCoords, m_active_delivery_point.Item2) < 1.5f)
                        {
                            /* Complete payment*/
                            int reward_amount = 0;
                            if (m_active_delivery_type == DeliveryType.Scooter) { reward_amount = DeliveryData.DELIVERIES_REWARD_SCOOTER; Screen.ShowNotification(DeliveryData._U["DELIVERY_POINT_REWARD"] + DeliveryData.DELIVERIES_REWARD_SCOOTER.ToString()); }
                            if (m_active_delivery_type == DeliveryType.Van) { reward_amount = DeliveryData.DELIVERIES_REWARD_VAN; Screen.ShowNotification(DeliveryData._U["DELIVERY_POINT_REWARD"] + DeliveryData.DELIVERIES_REWARD_VAN.ToString()); }
                            if (m_active_delivery_type == DeliveryType.Truck) { reward_amount = DeliveryData.DELIVERIES_REWARD_TRUCK; Screen.ShowNotification(DeliveryData._U["DELIVERY_POINT_REWARD"] + DeliveryData.DELIVERIES_REWARD_TRUCK.ToString()); }

                            TriggerServerEvent("esx_deliveries:AddCashMoney", reward_amount);
                            PlaySound(-1, "Menu_Accept", "Phone_SoundSet_Default", false, 0, true);

                            /* Check if this is the last delivery */
                            if (IsLastDelivery())
                            {
                                RemovePlayerProps();
                                m_active_blip.Delete();
                                CitizenFX.Core.Debug.WriteLine("Player completed all deliveries. Calling ReturnToVehicle");
                                m_active_delivery_point = new Tuple<Vector3, Vector3>(m_baselocation_return_point, Vector3.Zero);
                                ReturnToBase(m_active_delivery_type);
                                Screen.ShowSubtitle(DeliveryData._U["GET_BACK_TO_DELIVERYHUB"]);
                                m_delivery_state = DELIVERY_STATE.PLAYER_RETURNING_TO_BASE;
                                return;
                            }
                            else
                            {
                                RemovePlayerProps();
                                GetNextDeliveryPoint(false);
                                m_delivery_state = DELIVERY_STATE.PLAYER_STARTED_DELIVERY;
                                PlaySound(-1, "Menu_Accept", "Phone_SoundSet_Default", false, 0, true);
                            }

                        }
                    }

                    /* Return to base logic */
                    if (m_delivery_state == DELIVERY_STATE.PLAYER_RETURNING_TO_BASE)
                    {


                    }

                    await Delay(500);
                } else {
                    await Delay(1000);
                }

            } catch (Exception e)
            {
                CitizenFX.Core.Debug.WriteLine($"{e.Message} : Exception thrown on HandleLogic()");
            }

        }

        private void ForceCarryAnimation()
        {
            /* A somewhat fix to the force play door opening animation */
            AnimationFlags flags = AnimationFlags.UpperBodyOnly | AnimationFlags.Loop | AnimationFlags.AllowRotation | AnimationFlags.StayInEndFrame | (AnimationFlags)2;
            Game.PlayerPed.Task.PlayAnimation("anim@heists@box_carry@", "walk", 18.0f, -1, flags);
        }


        private async Task StartDelivery(DeliveryType aDeliveryType)
        {
            SavePlayerSkin();
            LoadWorkPlayerSkin(aDeliveryType);

            RequestModel((uint)GetHashKey("prop_paper_bag_01"));

            await SpawnDeliveryVehicle(aDeliveryType);

            m_delivery_state = DELIVERY_STATE.PLAYER_STARTED_DELIVERY;

            CreateRoute(aDeliveryType);
            GetNextDeliveryPoint(true);
            m_active_delivery_type = aDeliveryType;

            int safe_deposit = DeliveryData.SAFE_DEPOSIT_SCOOTER;

            if (aDeliveryType == DeliveryType.Van) safe_deposit = DeliveryData.SAFE_DEPOSIT_VAN;
            if (aDeliveryType == DeliveryType.Truck) safe_deposit = DeliveryData.SAFE_DEPOSIT_TRUCK;

            TriggerServerEvent("esx_deliveries:RemoveBankMoney", safe_deposit);
            Screen.ShowNotification(DeliveryData._U["SAFE_DEPOSIT_RECEIVED"]);

        }

        private bool isPlayerInsideDeliveryVehicle()
        {
            bool result = true;

            if (!Game.PlayerPed.IsSittingInVehicle() && GetVehiclePedIsIn(Game.PlayerPed.Handle, false) != m_delivery_vehicle.Model.Hash)
            {
                result = false;
            }

            return result;
        }

        private bool IsLastDelivery()
        {
            bool is_last = false;
            var dp1 = m_active_delivery_point.Item2;
            var dp2 = m_delivery_route.Last().Item2;
            if (dp1.X == dp2.X && dp1.Y == dp2.Y && dp1.Z == dp2.Z) is_last = true;
            return is_last;
        }

        private void RemovePlayerProps()
        {
            try
            {
                CitizenFX.Core.Debug.WriteLine(" :: Removing Player props for delivery.");

                for (int i = 0; i < m_player_attatchments.Count; i++)
                {
                    m_player_attatchments[i].Detach();
                    m_player_attatchments[i].Delete();
                }

                ClearPedTasks(Game.PlayerPed.Handle);

                m_player_attatchments.Clear();

            } catch (Exception e)
            {
                CitizenFX.Core.Debug.WriteLine($"{e.Message} : Exception thrown on RemovePlayerProps()");
            }
        }

        private async Task GetPlayerPropsForDelivery(DeliveryType aDeliveryType)
        {

            if (aDeliveryType == DeliveryType.Scooter)
            {
                CitizenFX.Core.Debug.WriteLine(" :: Getting Player props for delivery.");

                if (aDeliveryType == DeliveryType.Scooter)
                {
                    if (!HasModelLoaded((uint)GetHashKey("prop_paper_bag_01")))
                    {
                        RequestModel((uint)GetHashKey("prop_paper_bag_01"));
                        while (!HasModelLoaded((uint)GetHashKey("prop_paper_bag_01")))
                        {
                            await Delay(0);
                        }
                    }

                    Prop prop_ent = null;
                    prop_ent = await World.CreateProp("prop_paper_bag_01", Game.PlayerPed.Position, Vector3.Zero, true, false);

                    AttachEntityToEntity(prop_ent.Handle, Game.PlayerPed.Handle, GetPedBoneIndex(Game.PlayerPed.Handle, 28422), 0.25f, 0.0f, 0.06f, 65.0f, -130.0f, -65.0f, true, true, false, true, 0, true);

                    m_player_attatchments.Add(prop_ent);
                }
            }

            if (aDeliveryType == DeliveryType.Van)
            {

                AnimationFlags flags = AnimationFlags.UpperBodyOnly | AnimationFlags.Loop | AnimationFlags.AllowRotation | AnimationFlags.StayInEndFrame | (AnimationFlags)2;
                Game.PlayerPed.Task.PlayAnimation("anim@heists@box_carry@", "walk", 18.0f, -1, flags);

                Prop prop_ent = null;

                var rs = GetRandomFromRange(0, DeliveryData.VAN_GOODS_PROP_NAMES.Count - 1);

                prop_ent = await World.CreateProp(DeliveryData.VAN_GOODS_PROP_NAMES[rs], GetOffsetFromEntityInWorldCoords(Game.PlayerPed.Handle, 0.0f, 0.0f, -5.0f), Vector3.Zero, false, false);

                AttachEntityToEntity(prop_ent.Handle, Game.PlayerPed.Handle, GetPedBoneIndex(Game.PlayerPed.Handle, 28422), 0.0f, 0.0f, -0.55f, 0.0f, 0.0f, 90.0f, true, false, false, true, 1, true);

                m_player_attatchments.Add(prop_ent);

            }

            if (aDeliveryType == DeliveryType.Truck)
            {
                AnimationFlags flags = AnimationFlags.UpperBodyOnly | AnimationFlags.Loop | AnimationFlags.AllowRotation | AnimationFlags.StayInEndFrame | (AnimationFlags)3;
                Game.PlayerPed.Task.PlayAnimation("anim@heists@box_carry@", "idle", 18.0f, -1, flags);
                Prop prop_ent = null;
                prop_ent = await World.CreateProp("prop_sacktruck_02b", GetOffsetFromEntityInWorldCoords(Game.PlayerPed.Handle, 0.0f, 0.0f, -5.0f), Vector3.Zero, false, false);
                AttachEntityToEntity(prop_ent.Handle, Game.PlayerPed.Handle, GetEntityBoneIndexByName(Game.PlayerPed.Handle, "SKEL_Pelvis"), -0.075f, 0.90f, -0.86f, -20.0f, 0.5f, 181.0f, true, false, false, true, 1, true);
                m_player_attatchments.Add(prop_ent);

            }


        }

        private async Task SpawnDeliveryVehicle(DeliveryType aDeliveryType)
        {
            try
            {

                var rnd = new Random(DateTime.Now.Millisecond);
                int rs = rnd.Next(0, DeliveryData.ParkingSpawns.Count);

                Vector3 spawn_location = new Vector3(DeliveryData.ParkingSpawns[rs].X, DeliveryData.ParkingSpawns[rs].Y, DeliveryData.ParkingSpawns[rs].Y);            

                if (aDeliveryType == DeliveryType.Scooter) m_delivery_vehicle = await World.CreateVehicle(DeliveryData.MODEL_SCOOTER, spawn_location, DeliveryData.ParkingSpawns[rs].W);

                if (aDeliveryType == DeliveryType.Truck)
                {
                    m_delivery_vehicle = await World.CreateVehicle(DeliveryData.MODEL_TRUCK, spawn_location, DeliveryData.ParkingSpawns[rs].W);
                    SetVehicleLivery(m_delivery_vehicle.Handle, 2);
                }

                if (aDeliveryType == DeliveryType.Van)
                {
                    m_delivery_vehicle = await World.CreateVehicle(DeliveryData.MODEL_VAN, spawn_location, DeliveryData.ParkingSpawns[rs].W);
                    SetVehicleExtra(m_delivery_vehicle.Handle, 2, false);
                }

                DecorSetInt(m_delivery_vehicle.Handle, "Delivery.Rental", DeliveryData.DecorCode);
                SetVehicleOnGroundProperly(m_delivery_vehicle.Handle);

                if (aDeliveryType == DeliveryType.Scooter)
                {
                    Vector3 spawn_location2 = new Vector3(DeliveryData.ParkingSpawns[rs].X, DeliveryData.ParkingSpawns[rs].Y, DeliveryData.ParkingSpawns[rs].Y);

                    Prop bag_ent = await World.CreateProp("prop_med_bag_01", spawn_location2, false, false);
                    AttachEntityToEntity(bag_ent.Handle, m_delivery_vehicle.Handle, GetEntityBoneIndexByName(Game.PlayerPed.Handle, "misc_a"), 0.0f, -0.55f, 0.45f, 0.0f, 0.0f, 0.0f, true, true, false, true, 0, true);

                    m_vehicle_attatchments.Add(bag_ent);

                }

            } catch (Exception e)
            {
                CitizenFX.Core.Debug.WriteLine($"{e.Message} : Exception thrown on SpawnDeliveryehicle()");
            }
        }

        private static void GetNextDeliveryPoint(bool firstTime)
        {
            if (m_active_blip != null) m_active_blip.Delete();

            for (int i = 0; i < m_delivery_point_complete.Count; i++)
            {
                if (m_delivery_point_complete[i] == false)
                {
                    if (!firstTime) m_delivery_point_complete[i] = true;
                    break;
                }
            }

            for (int i = 0; i < m_delivery_point_complete.Count; i++)
            {
                if (m_delivery_point_complete[i] == false)
                {
                    CitizenFX.Core.Debug.WriteLine("Created next delivery point.");
                    m_active_blip = CreateBlipAt(m_delivery_route[i].Item1);
                    m_active_delivery_point = m_delivery_route[i];
                    break;
                }
            }

        }

        private static void CreateRoute(DeliveryType aDeliveryType)
        {
            int total_deliveries_to_be_made = GetRandomFromRange(DeliveryData.DELIVERIES_MIN, DeliveryData.DELIVERIES_MAX);

            List<Tuple<Vector3, Vector3>> delivery_points = null;

            if (aDeliveryType == DeliveryType.Scooter) delivery_points = DeliveryData.DeliveryLocationsScooter;
            if (aDeliveryType == DeliveryType.Van) delivery_points = DeliveryData.DeliveryLocationsVan;
            if (aDeliveryType == DeliveryType.Truck) delivery_points = DeliveryData.DeliveryLocationsTruck;

            while (m_delivery_route.Count < total_deliveries_to_be_made)
            {
                Delay(1000);
                Vector3 previous_point = Vector3.Zero;

                if (m_delivery_route.Count < 1)
                {
                    previous_point = Game.PlayerPed.Position;
                }
                else
                {
                    previous_point = m_delivery_route.Last().Item1;
                }

                int rdp = GetRandomFromRange(0, delivery_points.Count - 1);
                Tuple<Vector3, Vector3> next_point = delivery_points[rdp];

                var distance_between_points = World.GetDistance(previous_point, next_point.Item1);

                /* We are not using those for now */
                //float dist_min = 50.0f;
                //float dist_max = 600.0f;

                //if (aDeliveryType == DeliveryType.Van) { dist_min = 0.0f; dist_max = 2000.0f; }
                //if (aDeliveryType == DeliveryType.Truck) { dist_min = 0.0f; dist_max = 2000.0f; }

                bool has_player_been_around_there_before = false;

                for (int i = 0; i < m_delivery_route.Count; i++)
                {
                    var _dist_between_points = World.GetDistance(next_point.Item1, m_delivery_route[i].Item1);
                    if (_dist_between_points < 50.0f) has_player_been_around_there_before = true;
                }

                if (!has_player_been_around_there_before)
                {
                    CitizenFX.Core.Debug.WriteLine("Added a new delivery point. Total: " + m_delivery_route.Count.ToString());
                    m_delivery_route.Add(next_point);
                    m_delivery_point_complete.Add(false);
                }

            }

        }

        private void ReturnToBase(DeliveryType aDeliveryType)
        {
            m_active_blip = CreateBlipAt(m_baselocation_return_point);

        }

        private void EndDelivery()
        {
            if (!Game.PlayerPed.IsSittingInVehicle() && GetVehiclePedIsIn(Game.PlayerPed.Handle, false) != m_delivery_vehicle.Model.Hash)
            {
                FinishDelivery(m_active_delivery_type, SafeDepositReturn.NO);
            } else
            {
                ReturnVehicle(m_active_delivery_type);
            }
                
        }
        private void ReturnVehicle(DeliveryType aDeliveryType)
        {
            SetModelAsNoLongerNeeded((uint)m_delivery_vehicle.Handle);
            m_delivery_vehicle.Delete();
            Screen.ShowNotification(DeliveryData._U["DELIVERY_VEHICLE_RETURNED"]);
            FinishDelivery(aDeliveryType, SafeDepositReturn.YES);
        }


        private void FinishDelivery(DeliveryType aDeliveryType, SafeDepositReturn aReturnType)
        {

            if (m_delivery_vehicle != null)
            {
                /* Remove all vehicle attatchments */
                for (int i = 0; i < m_vehicle_attatchments.Count; i++)
                {
                    m_vehicle_attatchments[i].Detach();
                    m_vehicle_attatchments[i].Delete();
                }

                m_vehicle_attatchments.Clear();

                /* Remove vehicle */
                m_delivery_vehicle.Delete();
            }

            m_delivery_state = DELIVERY_STATE.DELIVERY_INACTIVE;
            m_delivery_vehicle = null;

            m_delivery_route.Clear();

            m_delivery_point_complete.Clear();

            if (m_active_blip != null) m_active_blip.Delete();
            m_active_blip = null;
            m_active_delivery_point = null;
            m_active_delivery_type = DeliveryType.NotSelected;

            if (aReturnType == SafeDepositReturn.YES)
            {
                int safe_deposit = DeliveryData.SAFE_DEPOSIT_SCOOTER;

                if (aDeliveryType == DeliveryType.Van) safe_deposit = DeliveryData.SAFE_DEPOSIT_VAN;
                if (aDeliveryType == DeliveryType.Truck) safe_deposit = DeliveryData.SAFE_DEPOSIT_TRUCK;

                TriggerServerEvent("esx_deliveries:AddBankMoney", safe_deposit);
                Screen.ShowNotification(DeliveryData._U["SAFE_DEPOSIT_RETURNED"]);
            }

            if (aReturnType == SafeDepositReturn.NO)
            {
                Screen.ShowNotification(DeliveryData._U["SAFE_DEPOSIT_WITHHELD"]);
            }

            LoadDefaultPlayerSkin();
            m_active_blip.Delete();
            

        }



        private async Task HandleMarkers()
        {
            // Draw Mission Start Markers
            Vector3 pCoords = Game.PlayerPed.Position;

            /* Check if a delivery is active */
            if (m_delivery_state != DELIVERY_STATE.DELIVERY_INACTIVE)
            {

                /* Check if player reached the vehicle return location */
                World.DrawMarker(MarkerType.ChevronUpx1, m_baselocation_deleter, Vector3.Zero, Vector3.Zero, new Vector3(1.5f, 1.5f, 1.5f), System.Drawing.Color.FromArgb(150, 255, 50, 0), true, true);
                if (World.GetDistance(pCoords, m_baselocation_deleter) < 1.5f)
                {
                    Screen.DisplayHelpTextThisFrame(DeliveryData._U["END_DELIVERY"]);
                    if (Game.IsControlJustReleased(0, Control.Context))
                    {
                        
                        EndDelivery();
                        return;
                    }

                }


                if (m_delivery_state == DELIVERY_STATE.PLAYER_STARTED_DELIVERY)
                {

                    if (!isPlayerInsideDeliveryVehicle())
                    {
                        Vector3 vehpos = m_delivery_vehicle.Position;
                        vehpos.Z += 1.0f;

                        if (m_active_delivery_type == DeliveryType.Van) vehpos.Z += 1.0f;
                        if (m_active_delivery_type == DeliveryType.Truck) vehpos.Z += 2.0f;

                        World.DrawMarker(MarkerType.ChevronUpx1, vehpos, Vector3.Zero, Vector3.Zero, new Vector3(0.8f, 0.8f, 0.8f), System.Drawing.Color.FromArgb(150, 255, 128, 0), true, true, true);

                    }
                    else
                    {
                        if (World.GetDistance(pCoords, m_active_delivery_point.Item1) < 150.0f)
                        {
                            World.DrawMarker(MarkerType.ChevronUpx1, m_active_delivery_point.Item1, Vector3.Zero, Vector3.Zero, new Vector3(1.5f, 1.5f, 1.5f), System.Drawing.Color.FromArgb(150, 255, 128, 0), true, true);
                        }

                    }
                }

                if (m_delivery_state == DELIVERY_STATE.PLAYER_REACHED_VEHICLE_POINT)
                {

                    if (!isPlayerInsideDeliveryVehicle())
                    {
                        Vector3 trunkPos = m_delivery_vehicle.Position;
                        Vector3 trunkForwardVector = GetEntityForwardVector(m_delivery_vehicle.Handle);

                        float scaleFactor = 1.0f;
                        if (m_active_delivery_type == DeliveryType.Scooter) scaleFactor = DeliveryData.COLLECT_GOODS_SCALE_VECTOR_SCOOTER;
                        if (m_active_delivery_type == DeliveryType.Van) scaleFactor = DeliveryData.COLLECT_GOODS_SCALE_VECTOR_VAN;
                        if (m_active_delivery_type == DeliveryType.Truck) scaleFactor = DeliveryData.COLLECT_GOODS_SCALE_VECTOR_TRUCK;

                        trunkPos -= trunkForwardVector * scaleFactor;
                        trunkPos.Z += 0.7f;

                        Vector3 arrowSize = new Vector3(0.8f);
                        if (m_active_delivery_type == DeliveryType.Scooter) arrowSize = new Vector3(0.15f);

                        World.DrawMarker(MarkerType.ChevronUpx1, trunkPos, Vector3.Zero, Vector3.Zero, arrowSize, System.Drawing.Color.FromArgb(150, 255, 128, 0), true, true, true);

                        if (World.GetDistance(pCoords, trunkPos) < 1.0f)
                        {
                            Screen.DisplayHelpTextThisFrame(DeliveryData._U["REMOVE_GOODS"]);

                            if (Game.IsControlJustReleased(0, Control.Context))
                            {
                                await GetPlayerPropsForDelivery(m_active_delivery_type);
                                m_delivery_state = DELIVERY_STATE.PLAYER_REMOVED_GOODS_FROM_VEHICLE;
                            }
                        }
                    }
                }

                if (m_delivery_state == DELIVERY_STATE.PLAYER_REMOVED_GOODS_FROM_VEHICLE)
                {
                    World.DrawMarker(MarkerType.ChevronUpx1, m_active_delivery_point.Item2, Vector3.Zero, Vector3.Zero, new Vector3(1.5f, 1.5f, 1.5f), System.Drawing.Color.FromArgb(150, 255, 128, 0), true, true);
                }

                if (m_delivery_state == DELIVERY_STATE.PLAYER_RETURNING_TO_BASE)
                {
                    World.DrawMarker(MarkerType.ChevronUpx1, m_baselocation_deleter, Vector3.Zero, Vector3.Zero, new Vector3(1.5f, 1.5f, 1.5f), System.Drawing.Color.FromArgb(150, 255, 128, 0), true, true);
                }
            }

            if (m_delivery_state == DELIVERY_STATE.DELIVERY_INACTIVE)
            {
                if (World.GetDistance(pCoords, m_baselocation_coords) < 150.0f)
                {

                    World.DrawMarker(MarkerType.MotorcycleSymbol, m_baselocation_scooter, Vector3.Zero, Vector3.Zero, new Vector3(2.5f, 2.5f, 2.5f), System.Drawing.Color.FromArgb(150, 255, 128, 0), true, true);
                    World.DrawMarker(MarkerType.CarSymbol, m_baselocation_van, Vector3.Zero, Vector3.Zero, new Vector3(2.5f, 2.5f, 2.5f), System.Drawing.Color.FromArgb(150, 255, 128, 0), true, true);
                    World.DrawMarker(MarkerType.TruckSymbol, m_baselocation_truck, Vector3.Zero, Vector3.Zero, new Vector3(2.5f, 2.5f, 2.5f), System.Drawing.Color.FromArgb(150, 255, 128, 0), true, true);

                    if (World.GetDistance(pCoords, m_baselocation_scooter) < 1.5f)
                    {
                        Screen.DisplayHelpTextThisFrame(DeliveryData._U["START_DELIVERY"] + DeliveryData.SAFE_DEPOSIT_SCOOTER.ToString());
                        if (Game.IsControlJustReleased(0, Control.Context))
                        {
                            await StartDelivery(DeliveryType.Scooter);
                            PlaySound(-1, "Menu_Accept", "Phone_SoundSet_Default", false, 0, true);
                        }
                    }

                    if (World.GetDistance(pCoords, m_baselocation_van) < 1.5f)
                    {
                        Screen.DisplayHelpTextThisFrame(DeliveryData._U["START_DELIVERY"] + DeliveryData.SAFE_DEPOSIT_VAN.ToString());
                        if (Game.IsControlJustReleased(0, Control.Context))
                        {

                            m_active_delivery_type = DeliveryType.Van;
                            await StartDelivery(DeliveryType.Van);

                            PlaySound(-1, "Menu_Accept", "Phone_SoundSet_Default", false, 0, true);
                        }
                    }

                    if (World.GetDistance(pCoords, m_baselocation_truck) < 1.5f)
                    {
                        Screen.DisplayHelpTextThisFrame(DeliveryData._U["START_DELIVERY"] + DeliveryData.SAFE_DEPOSIT_TRUCK.ToString());
                        if (Game.IsControlJustReleased(0, Control.Context))
                        {
                            m_active_delivery_type = DeliveryType.Truck;
                            await StartDelivery(DeliveryType.Truck);
                            PlaySound(-1, "Menu_Accept", "Phone_SoundSet_Default", false, 0, true);
                        }
                    }

                }
            }
        }

        private static Blip CreateBlipAt(Vector3 atLocation)
        {
            var tmp_blip = World.CreateBlip(atLocation);
            tmp_blip.Sprite = (BlipSprite)1;
            tmp_blip.Color = BlipColor.Yellow;
            tmp_blip.IsShortRange = true;
            tmp_blip.Name = "Delivery Point";

            SetBlipAsMissionCreatorBlip(tmp_blip.Handle, true);
            SetBlipRoute(tmp_blip.Handle, true);

            return tmp_blip;
        }

        private static int GetRandomFromRange(int a, int b)
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            int rs = rnd.Next(a, b);
            return rs;
        }
    }

}