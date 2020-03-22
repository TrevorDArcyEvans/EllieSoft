
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using MonoTouch.Foundation;
using MonoTouch.CoreAnimation;
using MonoTouch.UIKit;

namespace Trek
{
  public class Application
  {
    static void Main(string[] args)
    {
      UIApplication.Main(args);
    }
  }

  // The name AppDelegate is referenced in the MainWindow.xib file.
  public partial class AppDelegate : UIApplicationDelegate, IUserInterface
  {
    const int NumCmds = 18;
    private Engine m_Engine;

    // [button text --> [command ID]
    private IDictionary<string, string> mCommands = new Dictionary<string, string>(NumCmds);

    // This method is invoked when the application has loaded its UI and its ready to run
    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
      app.SetStatusBarHidden(true, false);

      m_Engine = new Engine(this);
      m_Engine.Start();

      ApplyGreenScreenStyle();

      if (window.Bounds.Width > 600)
      {
        // increase font size for iPad
        TxtMain.Font = UIFont.FromName(TxtMain.Font.Name, 18.0f);
      }

      TxtMain.OnTouchesEnded += HandleTxtMainOnTouchesEnded;

      // If you have defined a view, add it here:
      window.AddSubview(navigationController.View);

      window.MakeKeyAndVisible();

      return true;
    }

    void HandleTxtMainOnTouchesEnded (NSSet touches, UIEvent evt)
    {
      if (m_Engine.IsGameFinished())
      {
        m_Engine.Start();
      }
    }

    // This method is required in iPhoneOS 3.0
    public override void OnActivated(UIApplication application)
    {
    }

    #region green screen styles

    private void ApplyGreenScreenStyle()
    {
      // Main green screen
      TxtMain.TextColor = UIColor.Green;
      TxtMain.BackgroundColor = UIColor.Black;

      // Reports
      ApplyGreenScreenStyle(CmdShortRangeScan);
      ApplyGreenScreenStyle(CmdLongRangeScan);
      ApplyGreenScreenStyle(CmdStatusReport);
      ApplyGreenScreenStyle(CmdGalacticRecord);

      // Nav
      ApplyGreenScreenStyle(CmdWarpEngineCtrl);
      ApplyGreenScreenStyle(CmdNavCalc);
      ApplyGreenScreenStyle(CmdStarBaseCalc);
      ApplyGreenScreenStyle(TxtWarpEngineDirn);
      ApplyGreenScreenStyle(TxtWarpEngineDist);
      ApplyGreenScreenStyle(TxtNavCalcX);
      ApplyGreenScreenStyle(TxtNavCalcY);

      // Weapons
      ApplyGreenScreenStyle(CmdPhotonTorpCtrl);
      ApplyGreenScreenStyle(CmdPhotonTorpCalc);
      ApplyGreenScreenStyle(CmdPhaserCtrl);
      ApplyGreenScreenStyle(CmdRepairDamage);
      ApplyGreenScreenStyle(CmdShieldsEnergyAdd);
      ApplyGreenScreenStyle(CmdShieldsEnergySubtract);
      ApplyGreenScreenStyle(TxtPhotonTorpDirn);
      ApplyGreenScreenStyle(TxtPhaserAmt);
      ApplyGreenScreenStyle(TxtShieldsEnergyAmt);

      // Admin
      ApplyGreenScreenStyle(CmdAbout);
      ApplyGreenScreenStyle(CmdHelp);
      ApplyGreenScreenStyle(CmdLoadGame);
      ApplyGreenScreenStyle(CmdResign);
      ApplyGreenScreenStyle(CmdSaveGame);
      ApplyGreenScreenStyle(TxtGameSlot);
    }

    private void ApplyGreenScreenStyle(UIButton btn)
    {
      Debug.Assert(btn.ButtonType == UIButtonType.Custom);
      btn.SetTitleColor(UIColor.Green, UIControlState.Normal);
      ApplyGreenScreenStyle(btn.Layer);
    }

    private void ApplyGreenScreenStyle(UITextView txt)
    {
      txt.TextColor = UIColor.Green;
      ApplyGreenScreenStyle(txt.Layer);
    }

    private void ApplyGreenScreenStyle(UITextField txt)
    {
      txt.TextColor = UIColor.Green;
      txt.BackgroundColor = UIColor.Black;
      ApplyGreenScreenStyle(txt.Layer);
    }

    private void ApplyGreenScreenStyle(CALayer layer)
    {
      layer.BackgroundColor = UIColor.Black.CGColor;
      layer.BorderColor = UIColor.Green.CGColor;
      layer.BorderWidth = 1.0f;
      layer.CornerRadius = 8.0f;
      layer.MasksToBounds = true;
    }

    #endregion

    #region IUserInterface

    public void AddCommands(CommandInfo Commands)
    {
      if (Commands.Commands.Count != NumCmds)
      {
        throw new ArgumentOutOfRangeException("Commands.Commands", "Must be exactly " + NumCmds.ToString() + "  commands to match number of buttons in UI");
      }

      // form command map
      foreach (CommandItemInfo cii in Commands.Commands)
      {
        switch (cii.CommandId)
        {
          case CommandId.WarpEngineControl:
            mCommands.Add(CmdWarpEngineCtrl.Title(UIControlState.Normal), cii.CommandId);
            break;

          case CommandId.ShortRangeScan:
            mCommands.Add(CmdShortRangeScan.Title(UIControlState.Normal), cii.CommandId);
            break;

          case CommandId.LongRangeScan:
            mCommands.Add(CmdLongRangeScan.Title(UIControlState.Normal), cii.CommandId);
            break;

          case CommandId.PhaserControl:
            mCommands.Add(CmdPhaserCtrl.Title(UIControlState.Normal), cii.CommandId);
            break;

          case CommandId.PhotonTorpedoControl:
            mCommands.Add(CmdPhotonTorpCtrl.Title(UIControlState.Normal), cii.CommandId);
            break;

          case CommandId.AddEnergyToShields:
            mCommands.Add(CmdShieldsEnergyAdd.Title(UIControlState.Normal), cii.CommandId);
            break;

          case CommandId.SubtractEnergyFromShields:
            mCommands.Add(CmdShieldsEnergySubtract.Title(UIControlState.Normal), cii.CommandId);
            break;

          case CommandId.DamageControl:
            mCommands.Add(CmdRepairDamage.Title(UIControlState.Normal), cii.CommandId);
            break;

          case CommandId.CumulativeGalaticRecord:
            mCommands.Add(CmdGalacticRecord.Title(UIControlState.Normal), cii.CommandId);
            break;

          case CommandId.StatusReport:
            mCommands.Add(CmdStatusReport.Title(UIControlState.Normal), cii.CommandId);
            break;

          case CommandId.PhotonTorpedoCalculator:
            mCommands.Add(CmdPhotonTorpCalc.Title(UIControlState.Normal), cii.CommandId);
            break;

          case CommandId.StarbaseCalculator:
            mCommands.Add(CmdStarBaseCalc.Title(UIControlState.Normal), cii.CommandId);
            break;

          case CommandId.NavigationCalculator:
            mCommands.Add(CmdNavCalc.Title(UIControlState.Normal), cii.CommandId);
            break;

          case CommandId.SaveGame:
            mCommands.Add(CmdSaveGame.Title(UIControlState.Normal), cii.CommandId);
            break;

          case CommandId.LoadGame:
            mCommands.Add(CmdLoadGame.Title(UIControlState.Normal), cii.CommandId);
            break;

          case CommandId.Help:
            mCommands.Add(CmdHelp.Title(UIControlState.Normal), cii.CommandId);
            break;

          case CommandId.About:
            mCommands.Add(CmdAbout.Title(UIControlState.Normal), cii.CommandId);
            break;

          case CommandId.ResignCommission:
            mCommands.Add(CmdResign.Title(UIControlState.Normal), cii.CommandId);
            break;

          default:
            throw new ArgumentOutOfRangeException("Failed to handle: " + cii.CommandId);
        }

        Debug.Assert(mCommands.Count == NumCmds);
      }
    }

    public void Display(string info)
    {
      TxtMain.Text += info;
    }

    public void DisplayLine(string info)
    {
      TxtMain.Text += info + Environment.NewLine;
    }

    public void Clear()
    {
      TxtMain.Text = string.Empty;
    }

    public void Quit()
    {
      DisplayLine("");
      DisplayLine("                            Game Over");
      DisplayLine("                    Touch to start a new game");

      // restart game
      ///m_Engine.Start();
    }

    public event CommandItemExecuteEventHandler ExecuteCommand;

    #endregion

    private void GetParameters(string cmd, out string param1, out string param2)
    {
      param1 = string.Empty;
      param2 = string.Empty;

      switch (cmd)
      {
        // no parameters
        case CommandId.LongRangeScan:
        case CommandId.StatusReport:
        case CommandId.CumulativeGalaticRecord:
        case CommandId.StarbaseCalculator:
        case CommandId.PhotonTorpedoCalculator:
        case CommandId.ResignCommission:
        case CommandId.Help:
        case CommandId.About:
          break;

        // 1 parameter
        case CommandId.ShortRangeScan:
          param1 = true.ToString();
          break;

        case CommandId.PhaserControl:
          param1 = TxtPhaserAmt.Text;
          break;

        case CommandId.PhotonTorpedoControl:
          param1 = TxtPhotonTorpDirn.Text;
          break;

        case CommandId.AddEnergyToShields:
          param1 = TxtShieldsEnergyAmt.Text;
          break;

        case CommandId.SubtractEnergyFromShields:
          param1 = TxtShieldsEnergyAmt.Text;
          break;

        case CommandId.DamageControl:
          param1 = (ChkAutoRepairDamage.On) ? "y" : "n";
          break;

        case CommandId.SaveGame:
          param1 = TxtGameSlot.Text;
          break;

        case CommandId.LoadGame:
          param1 = TxtGameSlot.Text;
          break;

        // two parameters
        case CommandId.NavigationCalculator:
          param1 = TxtNavCalcX.Text;
          param2 = TxtNavCalcY.Text;
          break;

        case CommandId.WarpEngineControl:
          param1 = TxtWarpEngineDirn.Text;
          param2 = TxtWarpEngineDist.Text;
          break;

        default:
          throw new ArgumentOutOfRangeException("Failed to handle: " + cmd);
      }
    }

    partial void OnExecuteCommand(MonoTouch.UIKit.UIButton sender)
    {
      if (ExecuteCommand != null)
      {
        // go back to main screen to show results
        navigationController.SelectedIndex = 0;

        // identify button
        string cmd = mCommands[sender.Title(UIControlState.Normal)];

        // get parameters for button
        string param1 = string.Empty;
        string param2 = string.Empty;
        GetParameters(cmd, out param1, out param2);

        ExecuteCommand(this, new CommandItemExecuteInfo(cmd, param1, param2));
      }
    }

    partial void DidEndOnExit(MonoTouch.UIKit.UITextField sender)
    {
      // hide keyboard
      sender.ResignFirstResponder();
    }
  }
}

