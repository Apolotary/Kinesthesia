### **Kinesthesia** ###

*a simple yet configurable toolkit that works with Kinect and interprets gesture recognition data into MIDI signals*


----------

[video demos](http://www.youtube.com/user/apolotary)


----------

## Versions ##

**v 0.5** (current version)

- Gesture recognition is now handled by [KinectToolbox](http://kinecttoolbox.codeplex.com), which gives faster response and more customizable properties for gestures.
- Configuration interface for gesture behavior. (see **Configuration Files** part for more info)
- Voice command control (see **Voice Command Controls** part for more info)
- Save / load / refresh / restore / quick save / quick load methods for configuration files (see **Configuration Files** part for more info)

**v 0.4** 

- MIDI-player which supports MIDI-files parsed to CSV (see **Parsing MIDI to CSV** part for more info)
- Multiple MIDI-tracks control support
- Velocity change control for each hand (e.g. left hand changes the velocity
for the piano part played by left hand and so on)
- File browser and simple CSV-parser

**v 0.3**

- Simplified gesture recognition
- Additions in MIDI-class

**v 0.2**

- Kinect skeletal tracking basics
- Interpreting hands coordinates as notes

**v 0.1**

- Basic MIDI-interaction
- Some base classes

----------
## Configuration Files ##

**Gestures and properties**

Kinesthesia provides a simple configuration interface for better control on gesture's behavior. Currently there're four types of gestures binded for each hand, they're:

- SwipeUp
- SwipeDown
- SwipeToRight
- SwipeToLeft

Each type is handled by leftHandSwipeDetector and rightHandSwipeDetector which are binded to left and right hands correspondingly. In order to configure detector's behavior, there're 6 properties:

- Swipe Minimal Length
- Swipe Maximal Length
- Swipe Minimal Height
- Swipe Maximal Height
- Swipe Minimal Duration
- Swipe Maximal Duration

where:

- Length - minimal or maximal horizontal path length for swipe gesture
- Height - minimal or maximal vertical path length for swipe gesture
- Duration - minimal or maximal duration for swipe gesture

These properties can be configured with appropriate sliders for each hand. **Length and Height** are doubles and their range is **from 0 to 1** (because original coordinates provided by Kinect SDK are very small floating point numbers). **Duration** is measured in miliseconds and its range is **from 0 to 3000**.

**Generated configuration Files**

In order to load default configuration, Kinesthesia loads **default.csv** from the current directory where executable file is located.

Kinesthesia also creates a temporary backup file **temp.csv**, which is rewriten every time when new configuration file is loaded. 

When quicksaving, Kinesthesia creates a new .csv configuration file using current UNIX time.

When closing, Kinesthesia creates or overwrites cache.txt file, which contains paths for the latest used configuration / MIDI files.

**Custom configuration files**

Configuration file can be created in any text editor or even edited withing the Settings Log block within the software. It should have .csv extension and follow this standard:

The first two lines are configuration lines for two gesture detectors:

`HandRight, 0.4, 0.2, 0.1, 0.2, 250, 1500`              
`HandLeft,  0.4, 0.2, 0.1, 0.2, 250, 1500`

where the parameters are:

- Hand that will be tracked: **HandRight** or **HandLeft**
- Swipe Minimal Length
- Swipe Maximal Length
- Swipe Minimal Height
- Swipe Maximal Height
- Swipe Minimal Duration
- Swipe Maximal Duration

The remaining 8 lines cover events and methods which should be called. 

`HandRight, SwipeToRight, SendNote `          
`HandRight, SwipeToLeft,  SendNote  `         
`HandRight, SwipeUp,      SendNote  `        
`HandRight, SwipeDown,    SendNote  `       
`HandLeft,  SwipeToRight, ChangeVolume`        
`HandLeft,  SwipeToLeft,  ChangeVolume   `  
`HandLeft,  SwipeUp,      BendPitch  `   
`HandLeft,  SwipeDown,    BendPitch`

The parameters are:

- Hand that will be tracked: **HandRight** or **HandLeft**
- Type of Swipe Gesture: **SwipeUp**, **SwipeDown**, **SwipeToLeft**, **SwipeToRight**
- Method that will be called when this gesture will be detected: **SendNote**, **BendPitch**, **ChangeVolume**, **VelocityChange** (available only for MIDI tracks) and **DoNothing** (if you don't want to call any method)

**Quick Loading / Quick Saving / Refreshing / Restoring defaults**

- **Quick Loading** -- loads the last configuration or MIDI file. The path is stored in cache.txt
- **Quick Saving** -- saves current configuration to the current folder by creating a .csv file with current unix time as file's name
- **Refreshing** -- reloads Settings log, so it could match the changes in detector's properties
- **Restoring defaults** -- reloads configuration from default.csv file

----------
## Voice Command Controls ##

Currently, this project uses voice recognition class provided by [KinectToolbox](http://kinecttoolbox.codeplex.com). Kinesthesia can recognize the following commands:

- **Play** -- plays the current parsed MIDI track
- **Stop** -- stops playing current track
- **Swipe** -- turns on gesture recognition (unfortunately the library wasn't able to recognize the word "track")
- **Load** -- load last configuration file
- **Save** -- quickly save current configuration file
- **File** -- load last MIDI(CSV) file
- **Reload** -- reload settings log
- **Default** -- restore default configuration

----------


## Parsing MIDI to CSV ##

To parse a binary MIDI file to CSV, you'll need [this command line tool](http://www.fourmilab.ch/webtools/midicsv/)

Just copy the MIDI file to the folder with Midicsv.exe and execute command like this:

`Midicsv.exe example.mid example.csv`

where the first item is the converter itself, second is the MIDI-file and third is the desired output name

Then, choose the resulted MIDI-file via Open dialogue. Currently this file should have at least three tracks, otherwise the program woud crash. However you  can change the quantity of tracks within the MainWindow.xaml.cs class

----------


for Kinect and WPF-related stuff I use: 

- KinectSDK 
- KinectToolbox
- Examples from KinectforWindowsSDK series

for MIDI-related stuff I use this awesome library: [midi-dot-net](http://code.google.com/p/midi-dot-net/) 

for virtual port I'm currently using [loopbe1](http://nerds.de/en/loopbe1.html)

however there's also a good analogue for it: [midiyoke](http://www.midiox.com/myoke.htm)

