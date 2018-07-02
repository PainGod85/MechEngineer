#!/bin/bash

set -ex

cd ..

SEVENZIP="/c/Program Files/7-Zip/7z"

INCLUDES="-ir!MechEngineer"
INCLUDES_ALL="$INCLUDES -ir!CBTHeat -ir!CBTMovement -ir!CustomComponent -ir!DynModLib -ir!HardpointFixMod -ir!pansar -ir!SkipTutorial -ir!SpeedMod ModTek.dll"
EXCLUDES="-xr!log.txt -xr!*.log -xr!*.suo -xr!*.user -xr!bin -xr!obj -xr!.vs -xr!.git -xr!_ignored -xr!*.zip"

"$SEVENZIP" a -tzip -mx0 MechEngineer/MechEngineer.zip $EXCLUDES $INCLUDES
"$SEVENZIP" a -tzip -mx0 MechEngineer/MechEngineerWorkspace.zip $EXCLUDES $INCLUDES_ALL