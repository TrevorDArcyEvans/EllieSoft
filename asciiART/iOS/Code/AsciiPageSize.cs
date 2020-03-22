using System;

namespace asciiART
{
  public class AsciiPageSize
  {
    public string Name { get; private set;}
    public int Width { get; private set;}
    public int Height { get; private set;}

    public AsciiPageSize(string name, int width, int height)
    {
      Name = name;
      Width = width;
      Height = height;
    }
  }
}

