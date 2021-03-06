// Generated by .NET Reflector from C:\Program Files (x86)\Steam\steamapps\common\Cities_Skylines\Cities_Data\Managed\Assembly-CSharp.dll
using ColossalFramework;
using ColossalFramework.Globalization;
using ColossalFramework.Packaging;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using ColossalFramework.Plugins;
using Lidgren.Network;
using SimpleJSON;
using UnityEngine;
using ColossalFramework.PlatformServices;

namespace SkylinesMultiplayer
{
    public class JoinPanel : LoadSavePanelBase<MapMetaData>
    {
        private const string DOWNLOAD_MAP_STRING = "Please subscribe to the map, to join the game.\nIt can take a while before the downloads starts";
        private const string DOWNLOADING_STRING = "Downloading... {0}%";
        private const string FINSISH_DOWNLOAD_STRING = "Map finished downloading, starting game.";

        private UILabel m_Author;
        private UILabel m_BuildableArea;
        public CityNameGroups m_CityNameGroups;
        public float m_FarmingExponent = 1f;
        private UISprite m_FertilityNoResources;
        private UIProgressBar m_FertilityResources;
        private UIListBox m_FileList;
        public float m_ForestryExponent = 1f;
        private UISprite m_ForestryNoResources;
        private UIProgressBar m_ForestryResources;
        private UISprite m_Highway;
        private UISprite m_InHighway;
        private UISprite m_InPlane;
        private UISprite m_InShip;
        public EasingType m_InterpolationEasingType = EasingType.Bounce;
        public float m_InterpolationTime = 0.7f;
        private UISprite m_InTrain;
        private UITextField m_MapName;
        private UISprite m_NoHighway;
        private UISprite m_NoPlane;
        private UISprite m_NoShip;
        private UISprite m_NoTrain;
        public float m_OilExponent = 1f;
        private UISprite m_OilNoResources;
        private UIProgressBar m_OilResources;
        public float m_OreExponent = 1f;
        private UISprite m_OreNoResources;
        private UIProgressBar m_OreResources;
        private UISprite m_OutHighway;
        private UISprite m_OutPlane;
        private UISprite m_OutShip;
        private UISprite m_OutTrain;
        private UISprite m_Plane;
        private UISprite m_Ship;
        private UITextureSprite m_SnapShot;
        public UIComponent[] m_TabFocusList;
        private UISprite m_Train;
        public float m_WaterExponent = 1f;
        private UISprite m_WaterNoResources;
        private UIProgressBar m_WaterResources;
        public bool m_Virgin;
        private UIButton m_Start;

        private UILabel m_playersLabel;
        private UILabel m_pingLabel;
        private UILabel m_errorLabel;

        private JSONArray m_serverList;

        private ulong m_mapID;
        private int m_selectedIndex;
        private NetClient m_netClient;
        private float m_pingTimeStamp;
        private IPEndPoint m_pingIp;
        private bool m_gotResponseFromServer;
        private bool m_downloadingMap;

        protected void Awake()
        {
            this.m_Author = base.Find<UILabel>("Author");
            this.m_FileList = base.Find<UIListBox>("MapList");
            base.ClearListing();
            m_FileList.ClearEventInvocations("eventSelectedIndexChanged");
            this.m_FileList.eventSelectedIndexChanged +=
                new ColossalFramework.UI.PropertyChangedEventHandler<int>(this.OnMapSelectionChanged);
            this.m_MapName = base.Find<UITextField>("MapName");
            this.m_MapName.ClearEventInvocations("eventTextChanged");
            //this.m_MapName.eventTextChanged += (c, t) => (this.m_Virgin = false);
            this.m_SnapShot = base.Find<UITextureSprite>("SnapShot");
            this.m_BuildableArea = base.Find<UILabel>("BuildableArea");
            this.m_OilResources = base.Find<UIProgressBar>("ResourceBarOil");
            this.m_OreResources = base.Find<UIProgressBar>("ResourceBarOre");
            this.m_ForestryResources = base.Find<UIProgressBar>("ResourceBarForestry");
            this.m_FertilityResources = base.Find<UIProgressBar>("ResourceBarFarming");
            this.m_WaterResources = base.Find<UIProgressBar>("ResourceBarWater");
            this.m_OilNoResources = base.Find("ResourceOil").Find<UISprite>("NoNoNo");
            this.m_OreNoResources = base.Find("ResourceOre").Find<UISprite>("NoNoNo");
            this.m_ForestryNoResources = base.Find("ResourceForestry").Find<UISprite>("NoNoNo");
            this.m_FertilityNoResources = base.Find("ResourceFarming").Find<UISprite>("NoNoNo");
            this.m_WaterNoResources = base.Find("ResourceWater").Find<UISprite>("NoNoNo");
            this.m_Highway = base.Find<UISprite>("Highway");
            this.m_NoHighway = this.m_Highway.Find<UISprite>("NoNoNo");
            this.m_InHighway = this.m_Highway.Find<UISprite>("Incoming");
            this.m_OutHighway = this.m_Highway.Find<UISprite>("Outgoing");
            this.m_Train = base.Find<UISprite>("Train");
            this.m_NoTrain = this.m_Train.Find<UISprite>("NoNoNo");
            this.m_InTrain = this.m_Train.Find<UISprite>("Incoming");
            this.m_OutTrain = this.m_Train.Find<UISprite>("Outgoing");
            this.m_Ship = base.Find<UISprite>("Ship");
            this.m_NoShip = this.m_Ship.Find<UISprite>("NoNoNo");
            this.m_InShip = this.m_Ship.Find<UISprite>("Incoming");
            this.m_OutShip = this.m_Ship.Find<UISprite>("Outgoing");
            this.m_Plane = base.Find<UISprite>("Plane");
            this.m_NoPlane = this.m_Plane.Find<UISprite>("NoNoNo");
            this.m_InPlane = this.m_Plane.Find<UISprite>("Incoming");
            this.m_OutPlane = this.m_Plane.Find<UISprite>("Outgoing");
            this.m_Start = this.Find<UIButton>("Start");
            m_Start.ClearEventInvocations("eventClick");
            this.m_Start.eventClick +=
                new MouseEventHandler(this.OnStartClick);
            m_Start.color = Color.red;
            this.Refresh();

            var joinPanel = transform.GetComponent<UIPanel>();

            m_playersLabel = joinPanel.AddUIComponent<UILabel>();
            SetLabel(m_playersLabel, "Players: ", 505, 100);

            m_pingLabel = joinPanel.AddUIComponent<UILabel>();
            SetLabel(m_pingLabel, "Ping: ", 505, 125);

            m_errorLabel = joinPanel.AddUIComponent<UILabel>();
            SetLabel(m_errorLabel, "", 505, 150);


            NetPeerConfiguration config = new NetPeerConfiguration("FPSMod");
            config.SetMessageTypeEnabled(NetIncomingMessageType.UnconnectedData, true);
            
            m_netClient = new NetClient(config);
            m_netClient.Start();
        }

        
        private void SetLabel(UILabel label, string p, int x, int y)
        {
            label.AlignTo(this.GetComponent<UIPanel>(), UIAlignAnchor.TopRight);
            label.relativePosition = new Vector3(x, y);
            label.text = p;
            label.textScale = 1;
            label.verticalAlignment = UIVerticalAlignment.Middle;
            label.size = new Vector3(120, 20);
        }

        private string GenerateMapName(string environment)
        {
            CityNameGroups.Environment environment2 = this.m_CityNameGroups.FindGroup(environment);
            string key = environment2.m_closeDistance[UnityEngine.Random.Range(0, environment2.m_closeDistance.Length)];
            int max = (int) Locale.Count("CONNECTIONS_PATTERN", key);
            string format = Locale.Get("CONNECTIONS_PATTERN", key, UnityEngine.Random.Range(0, max));
            int num2 = (int) Locale.Count("CONNECTIONS_NAME", key);
            string str3 = Locale.Get("CONNECTIONS_NAME", key, UnityEngine.Random.Range(0, num2));
            return string.Format(format, str3);
        }

        private void OnDestroy()
        {
            ValueAnimator.Cancel("NewGameOil");
            ValueAnimator.Cancel("NewGameOre");
            ValueAnimator.Cancel("NewGameForest");
            ValueAnimator.Cancel("NewGameFertility");
            ValueAnimator.Cancel("NewGameWater");
        }

        private void OnEnterFocus(UIComponent comp, UIFocusEventParameter p)
        {
            if (p.gotFocus == base.component)
            {
                this.m_FileList.Focus();
            }
        }

        private void OnItemDoubleClick(UIComponent comp, int sel)
        {
            if ((comp == this.m_FileList) && (sel != -1))
            {
                this.OnStartNewGame();
            }
        }

        private void OnStartClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            OnStartNewGame();
        }

        public void OnKeyDown(UIComponent comp, UIKeyEventParameter p)
        {
            /*
            if (!p.used)
            {
                if (p.keycode == KeyCode.Escape)
                {
                    this.OnClosed();
                    p.Use();
                }
                else if (p.keycode == KeyCode.Tab)
                {
                    UIComponent activeComponent = UIView.activeComponent;
                    int index = Array.IndexOf<UIComponent>(this.m_TabFocusList, activeComponent);
                    if ((index != -1) && (this.m_TabFocusList.Length > 0))
                    {
                        int num2 = 0;
                        do
                        {
                            if (p.shift)
                            {
                                index = (index - 1)%this.m_TabFocusList.Length;
                                if (index < 0)
                                {
                                    index += this.m_TabFocusList.Length;
                                }
                            }
                            else
                            {
                                index = (index + 1)%this.m_TabFocusList.Length;
                            }
                        } while (!this.m_TabFocusList[index].isEnabled && (num2 < this.m_TabFocusList.Length));
                        this.m_TabFocusList[index].Focus();
                    }
                    p.Use();
                }
                else if (p.keycode == KeyCode.Return)
                {
                    this.OnStartNewGame();
                    p.Use();
                }
            }*/
        }

        
        private void Update()
        {
            if (m_mapID != 0)
            {
                float subscribedItemProgress = PlatformService.workshop.GetSubscribedItemProgress(new PublishedFileId(m_mapID));

                if (subscribedItemProgress > 0)
                {
                    m_downloadingMap = true;
                    this.m_errorLabel.text = string.Format(DOWNLOADING_STRING, subscribedItemProgress * 100);
                }
                else if (m_downloadingMap)
                {
                    Package.AssetType[] assetTypes = new Package.AssetType[] { UserAssetType.SaveGameMetaData };
                    var package = PackageManager.FilterAssets(assetTypes).FirstOrDefault(x => x.package.GetPublishedFileID().AsUInt64 == m_mapID);

                    if (package != null)
                    {
                        m_downloadingMap = false;
                        StartGame(package);
                    }

                    this.m_errorLabel.text = FINSISH_DOWNLOAD_STRING;
                }
            }

            HandleIncomingNetworkMessages();
        }

        private void HandleIncomingNetworkMessages()
        {

            NetIncomingMessage msg;
            while (m_netClient != null && (msg = m_netClient.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                        Debug.Log(msg.ReadString());
                        break;
                    case NetIncomingMessageType.WarningMessage:
                        Debug.LogWarning(msg.ReadString());
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        Debug.LogError(msg.ReadString());
                        break;
                    case NetIncomingMessageType.UnconnectedData:
                        int msgType = msg.ReadInt32();
                        if (msgType == (int)UnConnectedMessageFunction.PingServerForPlayerCount)
                        {
                            int playerCount = msg.ReadInt32();
                            if (m_pingIp.Equals(msg.SenderEndPoint))
                            {
                                int ping = (int)Math.Ceiling(Time.realtimeSinceStartup / 1000 - m_pingTimeStamp / 1000);
                                Debug.Log("Pong " + ping + " P: " + playerCount);
                                m_playersLabel.text = "Players: " + playerCount;
                                m_pingLabel.text = "Ping: " + ping;
                                m_gotResponseFromServer = true;
                                m_Start.color = Color.green;
                            }
                        }
                        break;
                    default:
                        Debug.LogWarning("Unhandled type: " + msg.MessageType);
                        break;
                }
                m_netClient.Recycle(msg);
            }
        }


        private void OnMapSelectionChanged(UIComponent comp, int sel)
        {
            if (sel > -1)
            {
                try
                {
                    m_mapID = ulong.Parse(m_serverList[sel]["mapId"]);
                    Debug.Log("Switching to: " + m_mapID);
                    m_selectedIndex = sel;

                    this.m_MapName.text = m_serverList[sel]["name"];
                    m_Start.color = Color.red;

                    NetOutgoingMessage msg = m_netClient.CreateMessage();
                    msg.Write((int)UnConnectedMessageFunction.PingServerForPlayerCount);

                    int ip = BitConverter.ToInt32(IPAddress.Parse(m_serverList[sel]["ip"]).GetAddressBytes(), 0);
                    IPAddress ipAddress = new IPAddress(ip);
                    IPEndPoint receiver = new IPEndPoint(ipAddress, int.Parse(m_serverList[sel]["port"]));

                    m_netClient.SendUnconnectedMessage(msg, receiver);

                    m_pingTimeStamp = Time.realtimeSinceStartup;
                    m_pingIp = receiver;

                    m_playersLabel.text = "Players: -";
                    m_pingLabel.text = "Ping: Trying to ping server";
                    m_errorLabel.text = "";
                    
                    m_gotResponseFromServer = false;
                }
                catch (Exception)
                {
                    m_pingLabel.text = "Could not connect to the server";
                    throw;
                }
            }
        }

        private void OnResolutionChanged(UIComponent comp, Vector2 previousResolution, Vector2 currentResolution)
        {
            base.component.CenterToParent();
        }

        public void OnStartNewGame()
        {
            if (m_selectedIndex < 0 || m_serverList.Count == 0)
                return;

            var mapId = ulong.Parse(m_serverList[m_selectedIndex]["mapId"]);

            if (mapId == 0)
            {
                Debug.Log("Mapid = 0");
                return;
            }
            if (!m_gotResponseFromServer)
            {
                Debug.Log("No response from server " + m_gotResponseFromServer.ToString() + " " + m_pingTimeStamp);
                return;
            }

            Package.AssetType[] assetTypes = new Package.AssetType[] { UserAssetType.SaveGameMetaData };
            var package = PackageManager.FilterAssets(assetTypes).FirstOrDefault(x => x.package.GetPublishedFileID().AsUInt64 == mapId);

            if (package != null)
            {
                StartGame(package);
            }
            else
            {
                //TODO Also show a popup here as backup if the steamoverlay dosen't work
                m_errorLabel.text = DOWNLOAD_MAP_STRING;
                PlatformService.ActivateGameOverlayToWebPage("http://steamcommunity.com/sharedfiles/filedetails/?id=" + mapId + "/");
                Debug.Log("Please Download mapid " + mapId);
            }

            UIView.library.Hide(base.GetType().Name, 1);
        }

        private void StartGame(Package.Asset package)
        {
            ConnectSettings.Ip = m_serverList[m_selectedIndex]["ip"];
            ConnectSettings.Port = int.Parse(m_serverList[m_selectedIndex]["port"]);

            SaveGameMetaData mmd = package.Instantiate<SaveGameMetaData>();
            SavePanel.lastLoadedName = package.name;

            SimulationMetaData ngs = new SimulationMetaData
            {
                m_CityName = mmd.cityName,
                m_updateMode = SimulationManager.UpdateMode.LoadGame,
                m_environment = "",
                m_disableAchievements = SimulationMetaData.MetaBool.True
            };

            Singleton<LoadingManager>.instance.LoadLevel(mmd.assetRef, "Game", "InGame", ngs);
        }

        private void OnVisibilityChanged(UIComponent comp, bool visible)
        {
            if (visible)
            {
                this.m_Virgin = true;
                this.Refresh();
                base.component.CenterToParent();
                base.component.Focus();
            }
            else
            {
                this.m_FileList.selectedIndex = -1;
                MainMenu.SetFocus();
            }
        }

       
        protected override void Refresh()
        {
            base.ClearListing();
            m_FileList.items = null;

            using (WebClient client = new WebClient())
            {
                // Download data.
                client.Encoding = Encoding.UTF8;
                string value = client.DownloadString(Settings.ServerUrl + "/?modVersion=" + Settings.ModVersion);

                m_serverList = JSON.Parse(value).AsArray;
            }

            string[] servers = new string[m_serverList.Count];
            int i = 0;
            foreach (JSONNode obj in m_serverList)
            {
                servers[i] = obj["name"];
                i++;
            }
            
            this.m_FileList.items = servers;

            /*base.ClearListing();
            Package.AssetType[] assetTypes = new Package.AssetType[] {UserAssetType.MapMetaData};
            IEnumerator<Package.Asset> enumerator = PackageManager.FilterAssets(assetTypes).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Package.Asset current = enumerator.Current;
                    if ((current != null) && current.isEnabled)
                    {
                        MapMetaData mmd = current.Instantiate<MapMetaData>();
                        if (mmd.isPublished)
                        {
                            string mapName = mmd.mapName;
                            if (Locale.Exists("MAPNAME", mapName))
                            {
                                mapName = Locale.Get("MAPNAME", mapName);
                            }
                            base.AddToListing(mapName, current, mmd);
                        }
                    }
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
            this.m_FileList.items = base.GetListingItems();
            if (this.m_FileList.items.Length > 0)
            {
                this.m_FileList.selectedIndex = 0;
            }*/
        }

        private string ResolveName(string str)
        {
            ulong num;
            string[] separator = new string[] {":"};
            string[] strArray = str.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            if (((strArray.Length == 2) && (strArray[0] == "steamid")) && ulong.TryParse(strArray[1], out num))
            {
                return new Friend(new UserID(num)).personaName;
            }
            return "Unknown";
        }
    }
}