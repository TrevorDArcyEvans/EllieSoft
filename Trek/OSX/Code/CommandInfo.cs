using System.Collections.Generic;

namespace SuperStarTrek
{
  public class CommandInfo
  {
    public string Prompt { get; private set; }
    public string Title { get; private set; }
    public IList<CommandItemInfo> Commands { get; private set; }

    public CommandInfo(string ThisPrompt, string ThisTitle, IList<CommandItemInfo> ThisCommands)
    {
      Prompt = ThisPrompt;
      Title = ThisTitle;
      Commands = ThisCommands;
    }
  }
}
