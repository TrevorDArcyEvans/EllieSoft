
namespace Trek
{
  public class CommandItemExecuteInfo : CommandItemInfo
  {
    public readonly string Param1;
    public readonly string Param2;

    public CommandItemExecuteInfo(string ThisCommandID, string param1, string param2) :
      base(ThisCommandID, string.Empty)
    {
      Param1 = param1;
      Param2 = param2;
    }
  }

  public delegate void CommandItemExecuteEventHandler(object sender, CommandItemExecuteInfo cmd);

  public interface IUserInterface
  {
    /// <summary>
    /// adds a set of commands to currently available commands
    /// </summary>
    /// <param name="Commands">selection of possible commands</param>
    /// <returns>selected command</returns>
    void AddCommands(CommandInfo Commands);

    /// <summary>
    /// writes <para>info</para> to display
    /// </summary>
    /// <param name="info">string to write to display</param>
    void Display(string info);

    /// <summary>
    /// writes <para>info</para> to display and advances to next line
    /// </summary>
    /// <param name="info">string to write to display</param>
    void DisplayLine(string info);

    /// <summary>
    /// clears display
    /// </summary>
    void Clear();

    /// <summary>
    /// quits the application
    /// </summary>
    void Quit();

    event CommandItemExecuteEventHandler ExecuteCommand;
  }
}
