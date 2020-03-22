// CurrentVoiceIndex
//    0 = off
//    1 = child
//    2 = female
//    3 = male
//    4 = phonics

// IsCurrentLanguageGB
//    true = GB
//    false = US

var SoundFiles_Child = new Array
(
  "88_01_A.mp3",
  "88_01_B.mp3",
  "88_01_C.mp3",
  "88_01_D.mp3",
  "88_01_E.mp3",
  "88_01_F.mp3",
  "88_01_G.mp3",
  "88_01_H.mp3",
  "88_01_I.mp3",
  "88_01_J.mp3",
  "88_01_K.mp3",
  "88_01_L.mp3",
  "88_01_M.mp3",
  "88_01_N.mp3",
  "88_01_O.mp3",
  "88_01_P.mp3",
  "88_01_Q.mp3",
  "88_01_R.mp3",
  "88_01_S.mp3",
  "88_01_T.mp3",
  "88_01_U.mp3",
  "88_01_V.mp3",
  "88_01_W.mp3",
  "88_01_X.mp3",
  "88_01_Y.mp3",
  "88_01_Z.mp3",
  "88_01_Zed.mp3"
);

var SoundFiles_Female = new Array
(
  "63_01_A.mp3",
  "63_01_B.mp3",
  "63_01_C.mp3",
  "63_01_D.mp3",
  "63_01_E.mp3",
  "63_01_F.mp3",
  "63_01_G.mp3",
  "63_01_H.mp3",
  "63_01_I.mp3",
  "63_01_J.mp3",
  "63_01_K.mp3",
  "63_01_L.mp3",
  "63_01_M.mp3",
  "63_01_N.mp3",
  "63_01_O.mp3",
  "63_01_P.mp3",
  "63_01_Q.mp3",
  "63_01_R.mp3",
  "63_01_S.mp3",
  "63_01_T.mp3",
  "63_01_U.mp3",
  "63_01_V.mp3",
  "63_01_W.mp3",
  "63_01_X.mp3",
  "63_01_Y.mp3",
  "63_01_Z.mp3",
  "63_01_Zed.mp3"
);

var SoundFiles_Male = new Array
(
  "66_01_A_DJ.mp3",
  "66_01_B_DJ.mp3",
  "66_01_C_DJ.mp3",
  "66_01_D_DJ.mp3",
  "66_01_E_DJ.mp3",
  "66_01_F_DJ.mp3",
  "66_01_G_DJ.mp3",
  "66_01_H_DJ.mp3",
  "66_01_I_DJ.mp3",
  "66_01_J_DJ.mp3",
  "66_01_K_DJ.mp3",
  "66_01_L_DJ.mp3",
  "66_01_M_DJ.mp3",
  "66_01_N_DJ.mp3",
  "66_01_O_DJ.mp3",
  "66_01_P_DJ.mp3",
  "66_01_Q_DJ.mp3",
  "66_01_R_DJ.mp3",
  "66_01_S_DJ.mp3",
  "66_01_T_DJ.mp3",
  "66_01_U_DJ.mp3",
  "66_01_V_DJ.mp3",
  "66_01_W_DJ.mp3",
  "66_01_X_DJ.mp3",
  "66_01_Y_DJ.mp3",
  "66_01_Z_DJ.mp3",
  "66_01_Zed_DJ.mp3"
);

var SoundFiles_Phonics = new Array
(
  "Letter_a.mp3",
  "Letter_b.mp3",
  "Letter_c.mp3",
  "Letter_d.mp3",
  "Letter_e.mp3",
  "Letter_f.mp3",
  "Letter_g.mp3",
  "Letter_h.mp3",
  "Letter_i.mp3",
  "Letter_j.mp3",
  "Letter_k.mp3",
  "Letter_l.mp3",
  "Letter_m.mp3",
  "Letter_n.mp3",
  "Letter_o.mp3",
  "Letter_p.mp3",
  "Letter_q.mp3",
  "Letter_r.mp3",
  "Letter_s.mp3",
  "Letter_t.mp3",
  "Letter_u.mp3",
  "Letter_v.mp3",
  "Letter_w.mp3",
  "Letter_x.mp3",
  "Letter_y.mp3",
  "Letter_z.mp3",
  "Letter_z.mp3"
);

var SoundFiles = new Array();
SoundFiles[1] = SoundFiles_Child;
SoundFiles[2] = SoundFiles_Female;
SoundFiles[3] = SoundFiles_Male;
SoundFiles[4] = SoundFiles_Phonics;

var ResourcesDirectory = "./Resources/";
if (BrowserDetect.OS == "Android")
{
  ResourcesDirectory = "/android_asset/www/Resources/";
}

var SoundsBaseDirectory = ResourcesDirectory + "Sounds/";

var SoundsDirectory = new Array();
SoundsDirectory[1] = SoundsBaseDirectory + "Child/";
SoundsDirectory[2] = SoundsBaseDirectory + "Female/";
SoundsDirectory[3] = SoundsBaseDirectory + "Male/";
SoundsDirectory[4] = SoundsBaseDirectory + "Phonics/";

function GetSoundFilePath(LetterIndex, VoiceIndex, IsGB)
{
  if (VoiceIndex == 0)
  {
    // no sounds, so no path
    return "";
  }

  // special handling for British pronounciation of 'z'
  if (LetterIndex == 25 && IsGB == true)
  {
    LetterIndex = 26;
  }

  return SoundsDirectory[VoiceIndex] + SoundFiles[VoiceIndex][LetterIndex];
}

