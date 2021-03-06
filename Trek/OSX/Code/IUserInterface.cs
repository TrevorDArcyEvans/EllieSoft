﻿
namespace SuperStarTrek
{
  public delegate void CommandSelectedEventHandler(object sender, CommandItemInfo cmd);

  public interface IUserInterface
  {
    /// <summary>
    /// prompts user to enter a string with an option to Accept/Cancel
    /// </summary>
    /// <param name="prompt">descriptive prompt to user</param>
    /// <returns>user's response trimmed of white space and in lower case</returns>
    /// <remarks>return value is string.Empty if user cancels</remarks>
    string InputString(string prompt);

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

    event CommandSelectedEventHandler CommandSelected;
  }
}
