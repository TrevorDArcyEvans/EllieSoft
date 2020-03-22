// CurrentVoiceIndex
//    0 = off
//    1 = female
//    2 = male

var ResourcesDirectory = "./Resources/";

if (typeof device != "undefined" && device.platform == "Android")
{
  ResourcesDirectory = "/android_asset/www/Resources/";
}

var SoundsBaseDirectory = ResourcesDirectory + "Sounds/";

var SoundsDirectory = new Array();
SoundsDirectory[1] = SoundsBaseDirectory + "Heather/";
SoundsDirectory[2] = SoundsBaseDirectory + "Ray/";

function GetSoundFilePath(thisDate, VoiceIndex)
{
  if (VoiceIndex == 0)
  {
    // no sounds, so no path
    return "";
  }

  var filePath = ZeroPad(thisDate.getHours(), 2) + ZeroPad(thisDate.getMinutes(), 2) + ".mp3";

  return SoundsDirectory[VoiceIndex] + filePath;
}

