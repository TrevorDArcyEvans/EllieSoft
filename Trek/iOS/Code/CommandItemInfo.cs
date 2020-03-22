﻿
namespace Trek
{
  public class CommandItemInfo
  {
    /// <summary>
    /// globally unique string to identify this command
    /// </summary>
    public string CommandId { get; private set; }

    public string Description { get; private set; }

    public CommandItemInfo(string ThisCommandID, string ThisDescription)
    {
      CommandId = ThisCommandID;
      Description = ThisDescription;
    }
  }
}
