using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace SuperStarTrek
{
  class Engine
  {
    private const string DataFileRootName = "StarTrekData";
    private const string DataFileExtension = ".xml";

    private StarTrekData Data;
    private readonly IUserInterface UI_Main;

    public Engine(IUserInterface ThisUI_Main)
    {
      UI_Main = ThisUI_Main;

      UI_Main.AddCommands(MainCommands);

      UI_Main.CommandSelected += UI_Main_CommandSelected;

      InitializeGame();
    }

    void UI_Main_CommandSelected(object sender, CommandItemInfo cmd)
    {
      PrintGameStatus();
      if (IsGameFinished())
      {
        if (NewGame())
        {
          InitializeGame();
          Start();
        }
        else
        {
          UI_Main.Quit();
        }

        return;
      }

      // filter out computer commands first
      if (cmd.CommandId == "rec" ||
          cmd.CommandId == "sta" ||
          cmd.CommandId == "toc" ||
          cmd.CommandId == "bas" ||
          cmd.CommandId == "nvc")
      {
        if (Data.ComputerDamage > 0)
        {
          UI_Main.DisplayLine("The main computer is damaged. Repairs are underway.");
          UI_Main.DisplayLine("");
          return;
        }
      }


      switch (cmd.CommandId)
      {
        #region Main commands

        case "nav":
          Navigation();
          break;

        case "srs":
          ShortRangeScan();
          break;

        case "lrs":
          LongRangeScan();
          break;

        case "pha":
          PhaserControls();
          break;

        case "tor":
          TorpedoControl();
          break;

        case "dam":
          DamageControl();
          break;

        case "sav":
          SaveUserGame();
          break;

        case "ldg":
          LoadUserGame();
          break;

        case "hlp":
          ShowHelp();
          break;

        case "xxx":
          ResignCommission();
          if (NewGame())
          {
            InitializeGame();
            Start();
          }
          else
          {
            UI_Main.Quit();
          }
          break;

        #endregion

        #region Shield commands

        case "add":
          ShieldControls(true, Data.Energy);
          break;

        case "sub":
          ShieldControls(false, Data.ShieldLevel);
          break;

        #endregion

        #region Computer commands

        case "rec":
          DisplayGalaticRecord();
          break;

        case "sta":
          DisplayStatus();
          break;

        case "toc":
          PhotonTorpedoCalculator();
          break;

        case "bas":
          StarBaseCalculator();
          break;

        case "nvc":
          NavigationCalculator();
          break;

        #endregion

        default:
          Debug.Fail("Failed to handle: " + cmd.CommandId);
          break;
      }
    }

    #region String Constants

    #region Title

    public static readonly string[] TitleStrings =
    {
      @"                         ______ __   __ ______ ______ ______ ",
      @"                        / __  // /  / // __  // ____// __  /",
      @"                       / / /_// /  / // /_/ // /__  / /_/ /",
      @"                       _\ \  / /  / // ___ // __ / /   __/",
      @"                     / /_/ // /__/ // /    / /___ / /\ \",
      @"                    /_____/ \____ //_/    /_____//_/  \_\",
      @"",
      @"                        _______ ______  ______ __  __ ",
      @"                       /__  __// __  / / ____// / / /",
      @"                         / /  / /_/ / / /__  / // /",
      @"                        / /  /   __/ / __ / /   / ",
      @"                       / /  / /\ \  / /___ / /\ \",
      @"                      /_/  /_/  \_\/_____//_/  \_\",
      @"",
      @"                     ________________        _",
      @"                     \__(=======/_=_/____.--'-`--.___",
      @"                                \ \   `,--,-.___.----'",
      @"                              .--`\\--'../",
      @"                             '---._____.|]",
    };

    #endregion

    #region QuadrantNames

    public static readonly string[] QuadrantNames =
    {
      "Aaamazzara",
      "Altair IV",
      "Aurelia",
      "Bajor",
      "Benthos",
      "Borg Prime",
      "Cait",
      "Cardassia Prime",
      "Cygnia Minor",
      "Daran V",
      "Duronom",
      "Dytallix B",
      "Efros",
      "El-Adrel IV",
      "Epsilon Caneris III",
      "Ferenginar",
      "Finnea Prime",
      "Freehaven",
      "Gagarin IV",
      "Gamma Trianguli VI",
      "Genesis",
      "H'atoria",
      "Holberg 917-G",
      "Hurkos III",
      "Iconia",
      "Ivor Prime",
      "Iyaar",
      "Janus VI",
      "Jouret IV",
      "Juhraya",
      "Kabrel I",
      "Kelva",
      "Ktaris",
      "Ligillium",
      "Loval",
      "Lyshan",
      "Magus III",
      "Matalas",
      "Mudd",
      "Nausicaa",
      "New Bajor",
      "Nova Kron",
      "Ogat",
      "Orion III",
      "Oshionion Prime",
      "Pegos Minor",
      "P'Jem",
      "Praxillus",
      "Qo'noS",
      "Quadra Sigma III",
      "Quazulu VIII",
      "Rakosa V",
      "Rigel VII",
      "Risa",
      "Romulus",
      "Rura Penthe",
      "Sauria",
      "Sigma Draconis",
      "Spica",
      "Talos IV",
      "Tau Alpha C",
      "Ti'Acor",
      "Udala Prime",
      "Ultima Thule",
      "Uxal",
      "Vacca VI",
      "Volan II",
      "Vulcan",
      "Wadi",
      "Wolf 359",
      "Wysanti",
      "Xanthras III",
      "Xendi Sabu",
      "Xindus",
      "Yadalla Prime",
      "Yadera II",
      "Yridian",
      "Zalkon",
      "Zeta Alpha II",
      "Zytchin III",
    };

    #endregion

    #region Commands

    private static readonly string MainCommandPrompt = "Enter command: ";
    private static readonly string MainCommandTitle = "--- Commands -----------------";
    private static readonly List<CommandItemInfo> MainCommandItems = new List<CommandItemInfo>
    {
      new CommandItemInfo("nav", "Warp Engine Control"),
      new CommandItemInfo("srs", "Short Range Scan"),
      new CommandItemInfo("lrs", "Long Range Scan"),

      new CommandItemInfo("pha", "Phaser Control"),
      new CommandItemInfo("tor", "Photon Torpedo Control"),
      new CommandItemInfo("add", "Add Energy To Shields"),
      new CommandItemInfo("sub", "Subtract Energy From Shields"),
      new CommandItemInfo("dam", "Damage Control"),

      new CommandItemInfo("rec", "Cumulative Galatic Record"),
      new CommandItemInfo("sta", "Status Report"),

      new CommandItemInfo("toc", "Photon Torpedo Calculator"),
      new CommandItemInfo("bas", "Starbase Calculator"),
      new CommandItemInfo("nvc", "Navigation Calculator"),

      new CommandItemInfo("sav", "Save Game"),
      new CommandItemInfo("ldg", "Load Saved Game"),
      new CommandItemInfo("hlp", "Help"),
      new CommandItemInfo("xxx", "Resign Commission"),
    };

    private static readonly CommandInfo MainCommands = new CommandInfo(MainCommandPrompt, MainCommandTitle, MainCommandItems);

    #endregion

    #endregion

    public void Start()
    {
      UI_Main.Clear();
      PrintStrings(TitleStrings);
      PrintMission();
      GenerateSector();
      PrintGameStatus();
    }

    private bool NewGame()
    {
      UI_Main.DisplayLine("The Federation is in need of a new starship commander");
      UI_Main.DisplayLine(" for a similar mission.");
      UI_Main.DisplayLine("");

      string command = UI_Main.InputString("If there is a volunteer, let him step forward and enter 'aye': ");
      UI_Main.DisplayLine("");

      return command == "aye";
    }

    private bool IsGameFinished()
    {
      return Data.Destroyed || (Data.Energy == 0) || (Data.Klingons == 0) || (Data.TimeRemaining == 0) || Data.Resigned;
    }

    private void PrintGameStatus()
    {
      if (Data.Destroyed)
      {
        UI_Main.DisplayLine("MISSION FAILED: ENTERPRISE DESTROYED!!!");
        UI_Main.DisplayLine("");
        UI_Main.DisplayLine("");
        UI_Main.DisplayLine("");
      }
      else if (Data.Energy == 0)
      {
        UI_Main.DisplayLine("MISSION FAILED: ENTERPRISE RAN OUT OF ENERGY.");
        UI_Main.DisplayLine("");
        UI_Main.DisplayLine("");
        UI_Main.DisplayLine("");
      }
      else if (Data.Klingons == 0)
      {
        UI_Main.DisplayLine("MISSION ACCOMPLISHED: ALL KLINGON SHIPS DESTROYED. WELL DONE!!!");
        UI_Main.DisplayLine("");
        UI_Main.DisplayLine("");
        UI_Main.DisplayLine("");
      }
      else if (Data.TimeRemaining == 0)
      {
        UI_Main.DisplayLine("MISSION FAILED: ENTERPRISE RAN OUT OF TIME.");
        UI_Main.DisplayLine("");
        UI_Main.DisplayLine("");
        UI_Main.DisplayLine("");
      }
      else if (Data.Resigned)
      {
        UI_Main.DisplayLine("MISSION FAILED: COMMANDER RESIGNED.");
        UI_Main.DisplayLine("");
        UI_Main.DisplayLine("");
        UI_Main.DisplayLine("");
      }
    }

    private void ShowHelp ()
    {
      // load from resource
      Assembly assy = Assembly.GetExecutingAssembly ();
      StreamReader sr = new StreamReader(assy.GetManifestResourceStream("SuperStarTrek.StarTrek.txt"));

      UI_Main.Clear();
      UI_Main.DisplayLine(sr.ReadToEnd());
    }

    private void DamageControl()
    {
      int TotalDamage = Data.NavigationDamage +
                        Data.ShortRangeScanDamage +
                        Data.LongRangeScanDamage +
                        Data.ShieldControlDamage +
                        Data.ComputerDamage +
                        Data.PhotonDamage +
                        Data.PhaserDamage;
      int TimeToRepair = 1 + (int)TotalDamage / 7;    // repairs always take a minimum of 1 day

      UI_Main.DisplayLine("Technicians standing by to effect repairs to your ship");
      UI_Main.DisplayLine(string.Format("Estimated time to repair: {0} stardates.", TimeToRepair));
      string Choice = UI_Main.InputString("Will you authorize the repair order (Y/N)? ");

      if (Choice == "y")
      {
        Data.ResetDamage();
        Data.TimeRemaining -= TimeToRepair;
        Data.StarDate += TimeToRepair;
      }
    }

    #region Load/Save Games

    private string GetDataFilePath(string SlotName)
    {
      string DataFileName = DataFileRootName + SlotName + DataFileExtension;
      string DataFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DataFileName);

      return DataFilePath;
    }

    /// <summary>
    /// used for system default save/load file
    /// </summary>
    /// <returns></returns>
    private string GetDataFilePath()
    {
      return GetDataFilePath("");
    }

    private string GetDataFilePath(int Slot)
    {
      Debug.Assert(Slot >= 0 && Slot <= 9, "Only slots 0-9 are supported");

      return GetDataFilePath(Slot.ToString());
    }

    /// <summary>
    /// load game from specified file
    /// </summary>
    /// <param name="DataFilePath">full path to file to load game from</param>
    private void LoadGame(string DataFilePath)
    {
      if (!File.Exists(DataFilePath))
      {
        // never saved a game, so save this one
        SaveGame();
      }
      Debug.Assert(File.Exists(DataFilePath));

      XmlSerializer DataSerializer = new XmlSerializer(typeof(StarTrekData));
      using (FileStream fs = new FileStream(DataFilePath, FileMode.Open))
      {
        Data = (StarTrekData)DataSerializer.Deserialize(fs);
      }

      PrintMission();
    }

    /// <summary>
    /// used by system for default load game
    /// </summary>
    public void LoadGame()
    {
      string DataFilePath = GetDataFilePath();

      LoadGame(DataFilePath);
    }

    private void LoadUserGame()
    {
      const string Separator = ",";
      string prompt = "Enter save slot (";

      // work out which slots have a corresponding file
      for (int i = 0; i <= 9; i++)
      {
        string ThisDataFilePath = GetDataFilePath(i);
        if (File.Exists(ThisDataFilePath))
        {
          prompt += i.ToString();
          prompt += Separator;
        }
      }

      // remove trailing separator
      if (prompt.EndsWith(Separator))
      {
        prompt = prompt.Remove(prompt.Length - Separator.Length, Separator.Length);
      }

      prompt += ") ";

      string DataFilePath = string.Empty;
      if (GetUserGameDataFilePath(prompt, ref DataFilePath))
      {
        if (File.Exists(DataFilePath))
        {
          LoadGame(DataFilePath);
        }
        else
        {
          UI_Main.DisplayLine("Selected slot does not exist ");
        }
      }
    }

    /// <summary>
    /// save current game to specified file
    /// </summary>
    /// <param name="DataFilePath">full path to file to save game into</param>
    private void SaveGame(string DataFilePath)
    {
      XmlSerializer DataSerializer = new XmlSerializer(typeof(StarTrekData));
      using (StreamWriter sw = new StreamWriter(DataFilePath))
      {
        DataSerializer.Serialize(sw, Data);
      }
    }

    /// <summary>
    /// used by system for default save game
    /// </summary>
    public void SaveGame()
    {
      string DataFilePath = GetDataFilePath();

      SaveGame(DataFilePath);
    }

    private void SaveUserGame()
    {
      string DataFilePath = string.Empty;

      if (GetUserGameDataFilePath("Enter save slot (0-9) ", ref DataFilePath))
      {
        SaveGame(DataFilePath);
      }
    }

    private bool GetUserGameDataFilePath(string prompt, ref string DataFilePath)
    {
      int Slot = 0;
      if (InputInt(UI_Main, prompt, out Slot))
      {
        if (Slot < 0 || Slot > 9)
        {
          UI_Main.DisplayLine("Invalid save slot ");
          return false;
        }

        DataFilePath = GetDataFilePath(Slot);
        return true;
      }

      return false;
    }

    #endregion

    private void ResignCommission()
    {
      Data.Resigned = true;

      UI_Main.DisplayLine(string.Format("There were {0} Klingon Battlecruisers left at the", Data.Klingons));
      UI_Main.DisplayLine(" end of your mission.");
      UI_Main.DisplayLine("");
      UI_Main.DisplayLine("");
    }

    private static double ComputeDirection(int x1, int y1, int x2, int y2)
    {
      double direction = 0;
      if (x1 == x2)
      {
        if (y1 < y2)
        {
          direction = 7;
        }
        else
        {
          direction = 3;
        }
      }
      else if (y1 == y2)
      {
        if (x1 < x2)
        {
          direction = 1;
        }
        else
        {
          direction = 5;
        }
      }
      else
      {
        double dy = Math.Abs(y2 - y1);
        double dx = Math.Abs(x2 - x1);
        double angle = Math.Atan2(dy, dx);
        if (x1 < x2)
        {
          if (y1 < y2)
          {
            direction = 9.0 - 4.0 * angle / Math.PI;
          }
          else
          {
            direction = 1.0 + 4.0 * angle / Math.PI;
          }
        }
        else
        {
          if (y1 < y2)
          {
            direction = 5.0 + 4.0 * angle / Math.PI;
          }
          else
          {
            direction = 5.0 - 4.0 * angle / Math.PI;
          }
        }
      }

      return direction;
    }

    private void NavigationCalculator()
    {
      UI_Main.Clear();

      UI_Main.DisplayLine("");
      UI_Main.DisplayLine(string.Format("Enterprise located in quadrant [{0},{1}].", (Data.QuadrantX + 1), (Data.QuadrantY + 1)));
      UI_Main.DisplayLine("");

      double quadX;
      double quadY;

      if (!InputDouble(UI_Main, "Enter destination quadrant X (1--8): ", out quadX) || quadX < 1 || quadX > 8)
      {
        UI_Main.DisplayLine("Invalid X coordinate.");
        UI_Main.DisplayLine("");
        return;
      }

      if (!InputDouble(UI_Main, "Enter destination quadrant Y (1--8): ", out quadY) || quadY < 1 || quadY > 8)
      {
        UI_Main.DisplayLine("Invalid Y coordinate.");
        UI_Main.DisplayLine("");
        return;
      }

      UI_Main.DisplayLine("");
      int qx = ((int)(quadX)) - 1;
      int qy = ((int)(quadY)) - 1;
      if (qx == Data.QuadrantX && qy == Data.QuadrantY)
      {
        UI_Main.DisplayLine("That is the current location of the Enterprise.");
        UI_Main.DisplayLine("");
        return;
      }

      UI_Main.DisplayLine(string.Format("Direction: {0:#.##}", ComputeDirection(Data.QuadrantX, Data.QuadrantY, qx, qy)));
      UI_Main.DisplayLine(string.Format("Distance:  {0:##.##}", Distance(Data.QuadrantX, Data.QuadrantY, qx, qy)));
      UI_Main.DisplayLine("");
    }

    private void StarBaseCalculator()
    {
      UI_Main.Clear();

      UI_Main.DisplayLine("");
      if (Data.Quadrants[Data.QuadrantY][Data.QuadrantX].StarBase)
      {
        UI_Main.DisplayLine(string.Format("Starbase in sector [{0},{1}].", (Data.StarBaseX + 1), (Data.StarBaseY + 1)));
        UI_Main.DisplayLine(string.Format("Direction: {0:#.##}", ComputeDirection(Data.SectorX, Data.SectorY, Data.StarBaseX, Data.StarBaseY)));
        UI_Main.DisplayLine(string.Format("Distance:  {0:##.##}", Distance(Data.SectorX, Data.SectorY, Data.StarBaseX, Data.StarBaseY) / 8));
      }
      else
      {
        UI_Main.DisplayLine("There are no starbases in this quadrant.");
      }
      UI_Main.DisplayLine("");
    }

    private void PhotonTorpedoCalculator()
    {
      UI_Main.Clear();

      UI_Main.DisplayLine("");
      if (Data.KlingonShips.Count == 0)
      {
        UI_Main.DisplayLine("There are no Klingon ships in this quadrant.");
        UI_Main.DisplayLine("");
        return;
      }

      foreach (KlingonShip ship in Data.KlingonShips)
      {
        UI_Main.DisplayLine(string.Format("Direction {2:#.##}: Klingon ship in sector [{0},{1}].",
            (ship.SectorX + 1), (ship.SectorY + 1),
            ComputeDirection(Data.SectorX, Data.SectorY, ship.SectorX, ship.SectorY)));
      }
      UI_Main.DisplayLine("");
    }

    private void DisplayStatus()
    {
      UI_Main.Clear();

      UI_Main.DisplayLine("");
      UI_Main.DisplayLine(string.Format("               Time Remaining: {0}", Data.TimeRemaining));
      UI_Main.DisplayLine(string.Format("      Klingon Ships Remaining: {0}", Data.Klingons));
      UI_Main.DisplayLine(string.Format("                    Starbases: {0}", Data.StarBases));
      UI_Main.DisplayLine(string.Format("           Warp Engine Damage: {0}", Data.NavigationDamage));
      UI_Main.DisplayLine(string.Format("   Short Range Scanner Damage: {0}", Data.ShortRangeScanDamage));
      UI_Main.DisplayLine(string.Format("    Long Range Scanner Damage: {0}", Data.LongRangeScanDamage));
      UI_Main.DisplayLine(string.Format("       Shield Controls Damage: {0}", Data.ShieldControlDamage));
      UI_Main.DisplayLine(string.Format("         Main Computer Damage: {0}", Data.ComputerDamage));
      UI_Main.DisplayLine(string.Format("Photon Torpedo Control Damage: {0}", Data.PhotonDamage));
      UI_Main.DisplayLine(string.Format("                Phaser Damage: {0}", Data.PhaserDamage));
      UI_Main.DisplayLine("");
    }

    private void DisplayGalaticRecord()
    {
      UI_Main.Clear();

      UI_Main.DisplayLine("");
      UI_Main.DisplayLine("-------------------------------------------------");
      for (int i = 0; i < 8; i++)
      {
        StringBuilder sb = new StringBuilder();
        for (int j = 0; j < 8; j++)
        {
          sb.Append("| ");
          int klingonCount = 0;
          int starbaseCount = 0;
          int starCount = 0;
          Quadrant quadrant = Data.Quadrants[i][j];
          if (quadrant.Scanned)
          {
            klingonCount = quadrant.Klingons;
            starbaseCount = quadrant.StarBase ? 1 : 0;
            starCount = quadrant.Stars;
          }
          sb.Append(string.Format("{0}{1}{2} ", klingonCount, starbaseCount, starCount));
        }

        sb.Append("|");
        UI_Main.DisplayLine(sb.ToString());
        UI_Main.DisplayLine("-------------------------------------------------");
      }
      UI_Main.DisplayLine("");
    }

    private void PhaserControls()
    {
      if (Data.PhaserDamage > 0)
      {
        UI_Main.DisplayLine("Phasers are damaged. Repairs are underway.");
        UI_Main.DisplayLine("");
        return;
      }

      if (Data.KlingonShips.Count == 0)
      {
        UI_Main.DisplayLine("There are no Klingon ships in this quadrant.");
        UI_Main.DisplayLine("");
        return;
      }

      double phaserEnergy;
      UI_Main.DisplayLine("Phasers locked on target.");
      if (!InputDouble(UI_Main, string.Format("Enter phaser energy (1--{0}): ", Data.Energy), out phaserEnergy) || phaserEnergy < 1 || phaserEnergy > Data.Energy)
      {
        UI_Main.DisplayLine("Invalid energy level.");
        UI_Main.DisplayLine("");
        return;
      }
      UI_Main.DisplayLine("");

      UI_Main.DisplayLine("Firing phasers...");
      List<KlingonShip> destroyedShips = new List<KlingonShip>();
      foreach (KlingonShip ship in Data.KlingonShips)
      {
        Data.Energy -= (int)phaserEnergy;
        if (Data.Energy < 0)
        {
          Data.Energy = 0;
          break;
        }
        double distance = Distance(Data.SectorX, Data.SectorY, ship.SectorX, ship.SectorY);
        double deliveredEnergy = phaserEnergy * (1.0 - distance / 11.3);
        ship.ShieldLevel -= (int)deliveredEnergy;
        if (ship.ShieldLevel <= 0)
        {
          UI_Main.DisplayLine(string.Format("Klingon ship destroyed at sector [{0},{1}].",
              (ship.SectorX + 1), (ship.SectorY + 1)));
          destroyedShips.Add(ship);
        }
        else
        {
          UI_Main.DisplayLine(string.Format("Hit ship at sector [{0},{1}]. Klingon shield strength dropped to {2}.",
              (ship.SectorX + 1), (ship.SectorY + 1), ship.ShieldLevel));
        }
      }

      foreach (KlingonShip ship in destroyedShips)
      {
        Data.Quadrants[Data.QuadrantY][Data.QuadrantX].Klingons--;
        Data.Klingons--;
        Data.Sector[ship.SectorY][ship.SectorX] = SectorType.Empty;
        Data.KlingonShips.Remove(ship);
      }

      if (Data.KlingonShips.Count > 0)
      {
        UI_Main.DisplayLine("");
        KlingonsAttack();
      }
      UI_Main.DisplayLine("");
    }

    private void ShieldControls(bool adding, int maxTransfer)
    {
      double transfer;
      if (!InputDouble(UI_Main, string.Format("Enter amount of energy (1--{0}): ", maxTransfer), out transfer)
          || transfer < 1 || transfer > maxTransfer)
      {
        UI_Main.DisplayLine("Invalid amount of energy.");
        UI_Main.DisplayLine("");
        return;
      }
      UI_Main.DisplayLine("");

      if (adding)
      {
        Data.Energy -= (int)transfer;
        Data.ShieldLevel += (int)transfer;
      }
      else
      {
        Data.Energy += (int)transfer;
        Data.ShieldLevel -= (int)transfer;
      }

      UI_Main.DisplayLine(string.Format("Shield strength is now {0}. Energy level is now {1}.", Data.ShieldLevel, Data.Energy));
      UI_Main.DisplayLine("");
    }

    private bool KlingonsAttack()
    {
      if (Data.KlingonShips.Count > 0)
      {
        foreach (KlingonShip ship in Data.KlingonShips)
        {
          if (Data.Docked)
          {
            UI_Main.DisplayLine(string.Format("Enterprise hit by ship at sector [{0},{1}]. No damage due to starbase shields.",
                (ship.SectorX + 1), (ship.SectorY + 1)));
          }
          else
          {
            double distance = Distance(Data.SectorX, Data.SectorY, ship.SectorX, ship.SectorY);
            double deliveredEnergy = 300 * Data.random.NextDouble() * (1.0 - distance / 11.3);
            Data.ShieldLevel -= (int)deliveredEnergy;
            if (Data.ShieldLevel < 0)
            {
              Data.ShieldLevel = 0;
              Data.Destroyed = true;
            }
            UI_Main.DisplayLine(string.Format("Enterprise hit by ship at sector [{0},{1}]. Shields dropped to {2}.",
                (ship.SectorX + 1), (ship.SectorY + 1), Data.ShieldLevel));
            if (Data.ShieldLevel == 0)
            {
              return true;
            }
          }
        }

        return true;
      }

      return false;
    }

    private static double Distance(double x1, double y1, double x2, double y2)
    {
      double x = x2 - x1;
      double y = y2 - y1;

      return Math.Sqrt(x * x + y * y);
    }

    private void InduceDamage(int item)
    {
      if (Data.random.Next(7) > 0)
      {
        return;
      }

      int damage = 1 + Data.random.Next(5);
      if (item < 0)
      {
        item = Data.random.Next(7);
      }

      switch (item)
      {
        case 0:
          Data.NavigationDamage = damage;
          UI_Main.DisplayLine("Warp engines are malfunctioning.");
          break;

        case 1:
          Data.ShortRangeScanDamage = damage;
          UI_Main.DisplayLine("Short range scanner is malfunctioning.");
          break;

        case 2:
          Data.LongRangeScanDamage = damage;
          UI_Main.DisplayLine("Long range scanner is malfunctioning.");
          break;

        case 3:
          Data.ShieldControlDamage = damage;
          UI_Main.DisplayLine("Shield controls are malfunctioning.");
          break;

        case 4:
          Data.ComputerDamage = damage;
          UI_Main.DisplayLine("The main computer is malfunctioning.");
          break;

        case 5:
          Data.PhotonDamage = damage;
          UI_Main.DisplayLine("Photon torpedo controls are malfunctioning.");
          break;

        case 6:
          Data.PhaserDamage = damage;
          UI_Main.DisplayLine("Phasers are malfunctioning.");
          break;

        default:
          Debug.Fail("Failed to handle item: " + item.ToString());
          break;
      }

      UI_Main.DisplayLine("");
    }

    private bool RepairDamage()
    {
      if (Data.NavigationDamage > 0)
      {
        Data.NavigationDamage--;
        if (Data.NavigationDamage == 0)
        {
          UI_Main.DisplayLine("Warp engines have been repaired.");
        }
        UI_Main.DisplayLine("");
        return true;
      }

      if (Data.ShortRangeScanDamage > 0)
      {
        Data.ShortRangeScanDamage--;
        if (Data.ShortRangeScanDamage == 0)
        {
          UI_Main.DisplayLine("Short range scanner has been repaired.");
        }
        UI_Main.DisplayLine("");
        return true;
      }

      if (Data.LongRangeScanDamage > 0)
      {
        Data.LongRangeScanDamage--;
        if (Data.LongRangeScanDamage == 0)
        {
          UI_Main.DisplayLine("Long range scanner has been repaired.");
        }
        UI_Main.DisplayLine("");
        return true;
      }

      if (Data.ShieldControlDamage > 0)
      {
        Data.ShieldControlDamage--;
        if (Data.ShieldControlDamage == 0)
        {
          UI_Main.DisplayLine("Shield controls have been repaired.");
        }
        UI_Main.DisplayLine("");
        return true;
      }

      if (Data.ComputerDamage > 0)
      {
        Data.ComputerDamage--;
        if (Data.ComputerDamage == 0)
        {
          UI_Main.DisplayLine("The main computer has been repaired.");
        }
        UI_Main.DisplayLine("");
        return true;
      }

      if (Data.PhotonDamage > 0)
      {
        Data.PhotonDamage--;
        if (Data.PhotonDamage == 0)
        {
          UI_Main.DisplayLine("Photon torpedo controls have been repaired.");
        }
        UI_Main.DisplayLine("");
        return true;
      }

      if (Data.PhaserDamage > 0)
      {
        Data.PhaserDamage--;
        if (Data.PhaserDamage == 0)
        {
          UI_Main.DisplayLine("Phasers have been repaired.");
        }
        UI_Main.DisplayLine("");
        return true;
      }

      return false;
    }

    private void LongRangeScan()
    {
      UI_Main.Clear();

      if (Data.LongRangeScanDamage > 0)
      {
        UI_Main.DisplayLine("Long range scanner is damaged. Repairs are underway.");
        UI_Main.DisplayLine("");
        return;
      }

      UI_Main.DisplayLine("-------------------");
      for (int i = Data.QuadrantY - 1; i <= Data.QuadrantY + 1; i++)
      {
        StringBuilder sb = new StringBuilder();
        for (int j = Data.QuadrantX - 1; j <= Data.QuadrantX + 1; j++)
        {
          sb.Append("| ");
          int klingonCount = 0;
          int starbaseCount = 0;
          int starCount = 0;
          if (i >= 0 && j >= 0 && i < 8 && j < 8)
          {
            Quadrant quadrant = Data.Quadrants[i][j];
            quadrant.Scanned = true;
            klingonCount = quadrant.Klingons;
            starbaseCount = quadrant.StarBase ? 1 : 0;
            starCount = quadrant.Stars;
          }
          sb.Append(string.Format("{0}{1}{2} ", klingonCount, starbaseCount, starCount));
        }

        sb.Append("|");
        UI_Main.DisplayLine(sb.ToString());
        UI_Main.DisplayLine("-------------------");
      }

      UI_Main.DisplayLine("");
    }

    private void TorpedoControl()
    {
      if (Data.PhotonDamage > 0)
      {
        UI_Main.DisplayLine("Photon torpedo control is damaged. Repairs are underway.");
        UI_Main.DisplayLine("");
        return;
      }

      if (Data.PhotonTorpedoes == 0)
      {
        UI_Main.DisplayLine("Photon torpedoes exhausted.");
        UI_Main.DisplayLine("");
        return;
      }

      if (Data.KlingonShips.Count == 0)
      {
        UI_Main.DisplayLine("There are no Klingon ships in this quadrant.");
        UI_Main.DisplayLine("");
        return;
      }

      double direction;
      if (!InputDouble(UI_Main, "Enter firing direction (1.0--9.0): ", out direction) || direction < 1.0 || direction > 9.0)
      {
        UI_Main.DisplayLine("Invalid direction.");
        UI_Main.DisplayLine("");
        return;
      }

      UI_Main.DisplayLine("");
      UI_Main.DisplayLine("Photon torpedo fired...");
      Data.PhotonTorpedoes--;

      double angle = -(Math.PI * (direction - 1.0) / 4.0);
      if (Data.random.Next(3) == 0)
      {
        angle += ((1.0 - 2.0 * Data.random.NextDouble()) * Math.PI * 2.0) * 0.03;
      }
      double x = Data.SectorX;
      double y = Data.SectorY;
      double vx = Math.Cos(angle) / 20;
      double vy = Math.Sin(angle) / 20;
      int lastX = -1, lastY = -1;
      int newX = Data.SectorX;
      int newY = Data.SectorY;

      while (x >= 0 && y >= 0 && Math.Round(x) < 8 && Math.Round(y) < 8)
      {
        newX = (int)Math.Round(x);
        newY = (int)Math.Round(y);
        if (lastX != newX || lastY != newY)
        {
          UI_Main.DisplayLine(string.Format("  [{0},{1}]", newX + 1, newY + 1));
          lastX = newX;
          lastY = newY;
        }

        foreach (KlingonShip ship in Data.KlingonShips)
        {
          if (ship.SectorX == newX && ship.SectorY == newY)
          {
            UI_Main.DisplayLine(string.Format("Klingon ship destroyed at sector [{0},{1}].",
                (ship.SectorX + 1), (ship.SectorY + 1)));
            Data.Sector[ship.SectorY][ship.SectorX] = SectorType.Empty;
            Data.Klingons--;
            Data.KlingonShips.Remove(ship);
            Data.Quadrants[Data.QuadrantY][Data.QuadrantX].Klingons--;
            goto label;
          }
        }

        switch (Data.Sector[newY][newX])
        {
          case SectorType.Starbase:
            Data.StarBases--;
            Data.Quadrants[Data.QuadrantY][Data.QuadrantX].StarBase = false;
            Data.Sector[newY][newX] = SectorType.Empty;
            UI_Main.DisplayLine(string.Format("The Enterprise destroyed a Federation starbase at sector [{0},{1}]!",
                newX + 1, newY + 1));
            goto label;

          case SectorType.Star:
            UI_Main.DisplayLine(string.Format("The torpedo was captured by a star's gravitational field at sector [{0},{1}].",
                newX + 1, newY + 1));
            goto label;

          case SectorType.Empty:
          case SectorType.Enterprise:
          case SectorType.Klingon:
            break;

          default:
            Debug.Fail("Failed to handle: " + Data.Sector[newY][newX].ToString());
            break;
        }

        x += vx;
        y += vy;
      }

      UI_Main.DisplayLine("Photon torpedo failed to hit anything.");

    label:

      if (Data.KlingonShips.Count > 0)
      {
        UI_Main.DisplayLine("");
        KlingonsAttack();
      }
      UI_Main.DisplayLine("");
    }

    private void Navigation()
    {
      double maxWarpFactor = 8.0;
      if (Data.NavigationDamage > 0)
      {
        maxWarpFactor = 0.2 + Data.random.Next(9) / 10.0;
        UI_Main.DisplayLine(string.Format("Warp engines damaged. Maximum warp factor: {0}", maxWarpFactor));
        UI_Main.DisplayLine("");
      }

      double direction, distance;
      if (!InputDouble(UI_Main, "Enter course (1.0--9.0): ", out direction)
          || direction < 1.0 || direction > 9.0)
      {
        UI_Main.DisplayLine("Invalid course.");
        UI_Main.DisplayLine("");
        return;
      }

      if (!InputDouble(UI_Main, string.Format("Enter warp factor (0.1--{0}): ", maxWarpFactor), out distance)
          || distance < 0.1 || distance > maxWarpFactor)
      {
        UI_Main.DisplayLine("Invalid warp factor.");
        UI_Main.DisplayLine("");
        return;
      }

      UI_Main.DisplayLine("");

      distance *= 8;
      int energyRequired = (int)distance;
      if (energyRequired >= Data.Energy)
      {
        UI_Main.DisplayLine("Unable to comply. Insufficient energy to travel that speed.");
        UI_Main.DisplayLine("");
        return;
      }
      else
      {
        UI_Main.DisplayLine("Warp engines engaged.");
        UI_Main.DisplayLine("");
        Data.Energy -= energyRequired;
      }

      int lastQuadX = Data.QuadrantX, lastQuadY = Data.QuadrantY;
      double angle = -(Math.PI * (direction - 1.0) / 4.0);
      double x = Data.QuadrantX * 8 + Data.SectorX;
      double y = Data.QuadrantY * 8 + Data.SectorY;
      double dx = distance * Math.Cos(angle);
      double dy = distance * Math.Sin(angle);
      double vx = dx / 1000;
      double vy = dy / 1000;
      int quadX, quadY, sectX, sectY, lastSectX = Data.SectorX, lastSectY = Data.SectorY;
      Data.Sector[Data.SectorY][Data.SectorX] = SectorType.Empty;
      for (int i = 0; i < 1000; i++)
      {
        x += vx;
        y += vy;
        quadX = ((int)Math.Round(x)) / 8;
        quadY = ((int)Math.Round(y)) / 8;
        if (quadX == Data.QuadrantX && quadY == Data.QuadrantY)
        {
          sectX = ((int)Math.Round(x)) % 8;
          sectY = ((int)Math.Round(y)) % 8;
          if (sectX < 0 || sectY < 0)
          {
            Data.SectorX = lastSectX;
            Data.SectorY = lastSectY;
            Data.Sector[Data.SectorY][Data.SectorX] = SectorType.Enterprise;
            UI_Main.DisplayLine("Course would go outside the universe.");
            UI_Main.DisplayLine("");
            goto label;
          }

          if (Data.Sector[sectY][sectX] != SectorType.Empty)
          {
            Data.SectorX = lastSectX;
            Data.SectorY = lastSectY;
            Data.Sector[Data.SectorY][Data.SectorX] = SectorType.Enterprise;
            UI_Main.DisplayLine("Encountered obstacle within quadrant.");
            UI_Main.DisplayLine("");
            goto label;
          }

          lastSectX = sectX;
          lastSectY = sectY;
        }
      }

      if (x < 0)
      {
        x = 0;
      }
      else if (x > 63)
      {
        x = 63;
      }
      
      if (y < 0)
      {
        y = 0;
      }
      else if (y > 63)
      {
        y = 63;
      }

      quadX = ((int)Math.Round(x)) / 8;
      quadY = ((int)Math.Round(y)) / 8;
      Data.SectorX = ((int)Math.Round(x)) % 8;
      Data.SectorY = ((int)Math.Round(y)) % 8;
      if (quadX != Data.QuadrantX || quadY != Data.QuadrantY)
      {
        Data.QuadrantX = quadX;
        Data.QuadrantY = quadY;
        GenerateSector();
      }
      else
      {
        Data.QuadrantX = quadX;
        Data.QuadrantY = quadY;
        Data.Sector[Data.SectorY][Data.SectorX] = SectorType.Enterprise;
      }

    label:

      if (IsDockingLocation(Data.SectorY, Data.SectorX))
      {
        Data.ResetSupplies();
        Data.ResetDamage();

        Data.ShieldLevel = 0;

        Data.Docked = true;
      }
      else
      {
        Data.Docked = false;
      }

      if (lastQuadX != Data.QuadrantX || lastQuadY != Data.QuadrantY)
      {
        Data.TimeRemaining--;
        Data.StarDate++;
      }

      ShortRangeScan();

      if (Data.Docked)
      {
        UI_Main.DisplayLine("Lowering shields as part of docking sequence...");
        UI_Main.DisplayLine("Enterprise successfully docked with starbase.");
        UI_Main.DisplayLine("");
      }
      else
      {
        if (Data.Quadrants[Data.QuadrantY][Data.QuadrantX].Klingons > 0
            && lastQuadX == Data.QuadrantX && lastQuadY == Data.QuadrantY)
        {
          KlingonsAttack();
          UI_Main.DisplayLine("");
        }
        else if (!RepairDamage())
        {
          InduceDamage(-1);
        }
      }
    }

    private bool InputDouble(IUserInterface UI, string prompt, out double value)
    {
      try
      {
        string NumStr = UI.InputString(prompt);
        value = Double.Parse(NumStr);
        return true;
      }
      catch
      {
        value = 0;
      }
      return false;
    }

    private bool InputInt(IUserInterface UI, string prompt, out int value)
    {
      try
      {
        string NumStr = UI.InputString(prompt);
        value = Int32.Parse(NumStr);
        return true;
      }
      catch
      {
        value = 0;
      }
      return false;
    }

    private void GenerateSector()
    {
      Quadrant quadrant = Data.Quadrants[Data.QuadrantY][Data.QuadrantX];
      bool starbase = quadrant.StarBase;
      int stars = quadrant.Stars;
      int klingons = quadrant.Klingons;
      Data.KlingonShips.Clear();
      for (int i = 0; i < 8; i++)
      {
        for (int j = 0; j < 8; j++)
        {
          Data.Sector[i][j] = SectorType.Empty;
        }
      }

      Data.Sector[Data.SectorY][Data.SectorX] = SectorType.Enterprise;
      while (starbase || stars > 0 || klingons > 0)
      {
        int i = Data.random.Next(8);
        int j = Data.random.Next(8);
        if (IsSectorRegionEmpty(i, j))
        {
          if (starbase)
          {
            starbase = false;
            Data.Sector[i][j] = SectorType.Starbase;
            Data.StarBaseY = i;
            Data.StarBaseX = j;
          }
          else if (stars > 0)
          {
            Data.Sector[i][j] = SectorType.Star;
            stars--;
          }
          else if (klingons > 0)
          {
            Data.Sector[i][j] = SectorType.Klingon;
            KlingonShip klingonShip = new KlingonShip();
            klingonShip.ShieldLevel = 300 + Data.random.Next(200);
            klingonShip.SectorY = i;
            klingonShip.SectorX = j;
            Data.KlingonShips.Add(klingonShip);
            klingons--;
          }
        }
      }
    }

    private bool IsDockingLocation(int i, int j)
    {
      for (int y = i - 1; y <= i + 1; y++)
      {
        for (int x = j - 1; x <= j + 1; x++)
        {
          if (ReadSector(y, x) == SectorType.Starbase)
          {
            return true;
          }
        }
      }

      return false;
    }

    private bool IsSectorRegionEmpty(int i, int j)
    {
      for (int y = i - 1; y <= i + 1; y++)
      {
        if (ReadSector(y, j - 1) != SectorType.Empty
          && ReadSector(y, j + 1) != SectorType.Empty)
        {
          return false;
        }
      }

      return ReadSector(i, j) == SectorType.Empty;
    }

    private SectorType ReadSector(int i, int j)
    {
      if (i < 0 || j < 0 || i > 7 || j > 7)
      {
        return SectorType.Empty;
      }

      return Data.Sector[i][j];
    }

    private void ShortRangeScan()
    {
      UI_Main.Clear();

      if (Data.ShortRangeScanDamage > 0)
      {
        UI_Main.DisplayLine("Short range scanner is damaged. Repairs are underway.");
        UI_Main.DisplayLine("");
      }
      else
      {
        Quadrant quadrant = Data.Quadrants[Data.QuadrantY][Data.QuadrantX];
        quadrant.Scanned = true;
        PrintSector(quadrant);
      }

      UI_Main.DisplayLine("");
    }

    private void PrintSector(Quadrant quadrant)
    {
      string condition = "GREEN";
      if (quadrant.Klingons > 0)
      {
        condition = "RED";
      }
      else if (Data.Energy < 300)
      {
        condition = "YELLOW";
      }

      UI_Main.DisplayLine(string.Format("-=--=--=--=--=--=--=--=-             Region: {0}", quadrant.Name));
      PrintSectorRow(0, string.Format("           Quadrant: [{0},{1}]", Data.QuadrantX + 1, Data.QuadrantY + 1));
      PrintSectorRow(1, string.Format("             Sector: [{0},{1}]", Data.SectorX + 1, Data.SectorY + 1));
      PrintSectorRow(2, string.Format("           Stardate: {0}", Data.StarDate));
      PrintSectorRow(3, string.Format("     Time remaining: {0}", Data.TimeRemaining));
      PrintSectorRow(4, string.Format("          Condition: {0}", condition));
      PrintSectorRow(5, string.Format("             Energy: {0}", Data.Energy));
      PrintSectorRow(6, string.Format("            Shields: {0}", Data.ShieldLevel));
      PrintSectorRow(7, string.Format("   Photon Torpedoes: {0}", Data.PhotonTorpedoes));
      UI_Main.DisplayLine(string.Format("-=--=--=--=--=--=--=--=-             Docked: {0}", Data.Docked));

      if (quadrant.Klingons > 0)
      {
        UI_Main.DisplayLine("");
        UI_Main.DisplayLine(string.Format("Condition RED: Klingon ship{0} detected.", (quadrant.Klingons == 1 ? "" : "s")));
        if (Data.ShieldLevel == 0 && !Data.Docked)
        {
          UI_Main.DisplayLine("Warning: Shields are down.");
        }
      }
      else if (Data.Energy < 300)
      {
        UI_Main.DisplayLine("");
        UI_Main.DisplayLine("Condition YELLOW: Low energy level.");
        condition = "YELLOW";
      }
    }

    private void PrintSectorRow(int row, string suffix)
    {
      StringBuilder sb = new StringBuilder();
      for (int column = 0; column < 8; column++)
      {
        switch (Data.Sector[row][column])
        {
          case SectorType.Empty:
            sb.Append("   ");
            break;

          case SectorType.Enterprise:
            sb.Append("<*>");
            break;

          case SectorType.Klingon:
            sb.Append("+K+");
            break;

          case SectorType.Star:
            sb.Append(" * ");
            break;

          case SectorType.Starbase:
            sb.Append(">!<");
            break;

          default:
            Debug.Fail("Failed to handle: " +Data.Sector[row][column].ToString());
            break;
        }
      }

      if (suffix != null)
      {
        sb.Append(suffix);
      }
      UI_Main.DisplayLine(sb.ToString());
    }

    private void PrintMission()
    {
      UI_Main.DisplayLine(string.Format("Mission: Destroy {0} Klingon ships in {1} stardates with {2} starbases.",
          Data.Klingons, Data.TimeRemaining, Data.StarBases));
      UI_Main.DisplayLine("");
    }

    private void InitializeGame()
    {
      Data = new StarTrekData();

      List<string> names = new List<string>();
      foreach (string name in QuadrantNames)
      {
        names.Add(name);
      }

      for (int i = 0; i < 8; i++)
      {
        for (int j = 0; j < 8; j++)
        {
          int index = Data.random.Next(names.Count);
          Quadrant quadrant = new Quadrant();
          Data.Quadrants[i][j] = quadrant;
          quadrant.Name = names[index];
          quadrant.Stars = 1 + Data.random.Next(8);
          names.RemoveAt(index);
        }
      }

      int klingonCount = Data.Klingons;
      int starbaseCount = Data.StarBases;
      while (klingonCount > 0 || starbaseCount > 0)
      {
        int i = Data.random.Next(8);
        int j = Data.random.Next(8);
        Quadrant quadrant = Data.Quadrants[i][j];
        if (!quadrant.StarBase)
        {
          quadrant.StarBase = true;
          starbaseCount--;
        }

        if (quadrant.Klingons < 3)
        {
          quadrant.Klingons++;
          klingonCount--;
        }
      }
    }

    private void PrintStrings(string[] strings)
    {
      foreach (string str in strings)
      {
        UI_Main.DisplayLine(str);
      }
      UI_Main.DisplayLine("");
    }
  }
}
