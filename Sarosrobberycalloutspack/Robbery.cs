using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using FivePD.API;
using FivePD.API.Utils;

namespace Robbery
{
    [CalloutProperties("Ladenüberfall","Sarocesch","v1.0")]
    internal class Rob : Callout
    {
        public List<Vector3> calloutLocations = new List<Vector3>()
        {
            new Vector3(27.0218f, -1346.4795f, 29.4970f),
            new Vector3(-48.4901f, -1756.2280f, 29.4210f),
            new Vector3(374.7671f, 327.0466f, 103.5664f),
            new Vector3(1136.3304f, -981.7611f, 46.4158f),
            new Vector3(1699.2644f, 4924.4438f, 42.0637f),
            new Vector3(2678.0996f, 3281.0911f, 55.2411f),
            new Vector3(1961.5781f, 3741.9390f, 32.3437f),
            new Vector3(-2968.9714f, 390.5740f, 15.0433f),
            new Vector3(1162.7184f, -323.1989f, 69.2051f),
            new Vector3(-1487.3534f, -379.6466f, 40.1634f),
            new Vector3(-1223.6482f, -907.6509f, 12.3264f),
            new Vector3(-708.1566f, -913.6121f, 19.2156f),
            new Vector3(-1821.3606f, 792.6612f, 138.1272f),
            new Vector3(1729.9298f, 6415.0991f, 35.0372f),
            new Vector3(2555.9939f, 383.0114f, 108.6229f)
        };
        public Vector3 calloutLocation;

        public Ped suspect;

        public Rob()
        {
            calloutLocation = calloutLocations.SelectRandom();

            InitInfo(calloutLocation);
            ShortName = "Ladenüberfall";
            CalloutDescription = "Leitstelle: Berichten zufolge wird grade ein Laden überfallen, seien sie vorsichtig";
            ResponseCode = 3;
            StartDistance = 200f;
        }

        public async override void OnStart(Ped player)
        {
            base.OnStart(player);
            suspect = await SpawnPed(RandomUtils.GetRandomPed(), location: Location + 1);
            suspect.AlwaysKeepTask = true;
            suspect.BlockPermanentEvents = true;
            var weapons = new[]
            {
                WeaponHash.Knife,
                WeaponHash.Unarmed,
                WeaponHash.Knife,
                WeaponHash.Pistol,
            };
            suspect.Weapons.Give(weapons[RandomUtils.Random.Next(weapons.Length)], int.MaxValue, true, true);
            suspect.Armor = 100;
            suspect.AttachBlip();
            var random = new Random();
            var tasks = new[]
            {
                new Action<Ped>(ped => ped.Task.FightAgainst(player)),
                new Action<Ped>(ped => ped.Task.FleeFrom(player)),
            };
            var randomTask = tasks[random.Next(tasks.Length)];
            randomTask.Invoke(suspect);
        }

        public async override Task OnAccept()
        {
            InitBlip();
            UpdateData();
        }
    }
}