using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace SuperStarTrek
{
  public partial class MainWindowController : MonoMac.AppKit.NSWindowController, IUserInterface
  {
    private Engine m_Engine;

    // map of 'command description' --> CommandItemInfo
    private readonly Dictionary<string, CommandItemInfo> m_Cmds = new Dictionary<string, CommandItemInfo>();

    #region Constructors

    // Called when created from unmanaged code
    public MainWindowController (IntPtr handle) : base(handle)
    {
      Initialize ();
    }

    // Called when created directly from a XIB file
    [Export("initWithCoder:")]
    public MainWindowController (NSCoder coder) : base(coder)
    {
      Initialize ();
    }

    // Call to load from the XIB/NIB file
    public MainWindowController () : base("MainWindow")
    {
      Initialize ();
    }

    // Shared initialization code
    void Initialize ()
    {
    }

    #endregion

    public override void WindowDidLoad()
    {
      base.WindowDidLoad();

      m_Engine = new Engine(this);
      m_Engine.Start();

      // green screen
      TxtView.Font = NSFont.UserFixedPitchFontOfSize(12.0f);
      TxtView.BackgroundColor = NSColor.Black;
      TxtView.TextColor = NSColor.Green;

      Window.BackgroundColor = NSColor.Black;
    }

    // strongly typed window accessor
    public new MainWindow Window
    {
      get { return (MainWindow)base.Window; }
    }

    partial void BtnClicked (MonoMac.AppKit.NSButton sender)
    {
      if (null != CommandSelected)
      {
        CommandSelected (this, m_Cmds[sender.Title]);
      }
    }

    #region IUserInterface

    public string InputString(string prompt)
    {
      var InputCtrllr = new InputBoxController(prompt);

      // adjust width of input box according to prompt + padding
#if false

      http://efreedom.com/Question/1-1992950/NSString-SizeWithAttributes-Content-Rect

      float heightForStringDrawing(NSString *myString, NSFont *myFont, float myWidth)
      {
       NSTextStorage *textStorage = [[[NSTextStorage alloc] initWithString:myString] autorelease];
       NSTextContainer *textContainer = [[[NSTextContainer alloc] initWithContainerSize:NSMakeSize(myWidth, FLT_MAX)] autorelease];
       NSLayoutManager *layoutManager = [[[NSLayoutManager alloc] init] autorelease];
      
       [layoutManager addTextContainer:textContainer];
       [textStorage addLayoutManager:layoutManager];
       [textStorage addAttribute:NSFontAttributeName value:myFont range:NSMakeRange(0, [textStorage length])];
       [textContainer setLineFragmentPadding:0.0];
      
       (void) [layoutManager glyphRangeForTextContainer:textContainer];
      
       return [layoutManager usedRectForTextContainer:textContainer].size.height;
      }

#endif
      var promptWidth = 10 * prompt.Length;

      // centre input box on screen
      var screenFrame = NSScreen.MainScreen.Frame;
      var oldInputBoxFrame = InputCtrllr.Window.Frame;
      var newInputBoxFrame = new System.Drawing.RectangleF((screenFrame.Width - promptWidth) / 2, (screenFrame.Height - oldInputBoxFrame.Height) / 2, promptWidth, oldInputBoxFrame.Height);
      InputCtrllr.Window.SetFrame(newInputBoxFrame, true);

      var Retval = NSApplication.SharedApplication.RunModalForWindow (InputCtrllr.Window);

      return (Retval == 1) ? InputCtrllr.RealValue.StringValue : string.Empty;
    }

    public void AddCommands (CommandInfo Commands)
    {
      if (Commands.Commands.Count != 17)
      {
        throw new ArgumentOutOfRangeException("Commands.Commands", "Must be eaxactly 17 commands to match number of buttons in UI");
      }

      Btn01.Title = Commands.Commands[00].Description;
      Btn02.Title = Commands.Commands[01].Description;
      Btn03.Title = Commands.Commands[02].Description;
      Btn04.Title = Commands.Commands[03].Description;
      Btn05.Title = Commands.Commands[04].Description;
      Btn06.Title = Commands.Commands[05].Description;
      Btn07.Title = Commands.Commands[06].Description;
      Btn08.Title = Commands.Commands[07].Description;
      Btn09.Title = Commands.Commands[08].Description;
      Btn10.Title = Commands.Commands[09].Description;
      Btn11.Title = Commands.Commands[10].Description;
      Btn12.Title = Commands.Commands[11].Description;
      Btn13.Title = Commands.Commands[12].Description;
      Btn14.Title = Commands.Commands[13].Description;
      Btn15.Title = Commands.Commands[14].Description;
      Btn16.Title = Commands.Commands[15].Description;
      Btn17.Title = Commands.Commands[16].Description;

      foreach (CommandItemInfo cii in Commands.Commands)
      {
        m_Cmds.Add (cii.Description, cii);
      }

    }

    public void Display (string info)
    {
      TxtView.Value += info;
    }

    public void DisplayLine (string info)
    {
      TxtView.Value += info + Environment.NewLine;
    }

    public void Clear ()
    {
      TxtView.Value = string.Empty;
    }

    public void Quit()
    {
      NSApplication.SharedApplication.Terminate(this);
    }

    public event CommandSelectedEventHandler CommandSelected;

    #endregion

  }
}

