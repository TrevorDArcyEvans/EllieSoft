// CurrentVoiceIndex
//    0 = off
//    1 = female

var SoundFiles_Female = new Array
(
  "a.mp3",
  "chi.mp3",
  "e.mp3",
  "fu.mp3",
  "ha.mp3",
  "he.mp3",
  "hi.mp3",
  "ho.mp3",
  "i.mp3",
  "ka.mp3",
  "ke.mp3",
  "ki.mp3",
  "ko.mp3",
  "ku.mp3",
  "ma.mp3",
  "me.mp3",
  "mi.mp3",
  "mo.mp3",
  "mu.mp3",
  "n.mp3",
  "na.mp3",
  "ne.mp3",
  "ni.mp3",
  "no.mp3",
  "nu.mp3",
  "o.mp3",
  "ra.mp3",
  "re.mp3",
  "ri.mp3",
  "ro.mp3",
  "ru.mp3",
  "sa.mp3",
  "se.mp3",
  "shi.mp3",
  "so.mp3",
  "su.mp3",
  "ta.mp3",
  "te.mp3",
  "to.mp3",
  "tsu.mp3",
  "u.mp3",
  "wa.mp3",
  "wo.mp3",
  "ya.mp3",
  "yo.mp3",
  "yu.mp3"
);

var SoundFiles = new Array();
SoundFiles[1] = SoundFiles_Female;

var ResourcesDirectory = "./Resources/";
if (BrowserDetect.OS == "Android")
{
  ResourcesDirectory = "/android_asset/www/Resources/";
}

var SoundsBaseDirectory = ResourcesDirectory + "Sounds/";

var SoundsDirectory = new Array();
SoundsDirectory[1] = SoundsBaseDirectory + "Female/";

function GetSoundFilePath(LetterIndex, VoiceIndex)
{
  if (VoiceIndex == 0)
  {
    // no sounds, so no path
    return "";
  }

  return SoundsDirectory[VoiceIndex] + SoundFiles[VoiceIndex][LetterIndex];
}

