#!/bin/sh

#sign app bundle
codesign -v -f -s "3rd Party Mac Developer Application: Trevor D'Arcy-Evans" "$1.app"

#sign installer
productbuild --component "$1.app" /Applications --sign "3rd Party Mac Developer Installer: Trevor D'Arcy-Evans" "$1.pkg"

