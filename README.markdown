### **Kinesthesia** ###

*a simple yet configurable toolkit that works with Kinect and interprets gesture recognition data into MIDI signals*


----------

[video demos](http://www.youtube.com/user/apolotary)


----------

## Versions ##

**v 0.4** (current version)

- MIDI-player which supports MIDI-files parsed to CSV (see **Parsing MIDI to CSV part for more info**)
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


## Parsing MIDI to CSV ##

To parse a binary MIDI file to CSV, you'll need [this command line tool](http://www.fourmilab.ch/webtools/midicsv/)

Just copy the MIDI file to the folder with Midicsv.exe and execute command like this:

`MIdicsv.exe rm_theme.mid rm_theme.csv`

where the first item is the converter itself, second is the MIDI-file and third is the desired output name

Then, choose the resulted MIDI-file via Open dialogue. Currently this file should have at least three tracks, otherwise the program woud crash. However you  can change the quantity of tracks within the MainWindow.xaml.cs class

----------


for Kinect and WPF-related stuff I use: 

- KinectSDK 
- Code4Fun Kinect Toolkit  
- Examples from KinectforWindowsSDK series

for MIDI-related stuff I use this awesome library: [midi-dot-net](http://code.google.com/p/midi-dot-net/) 

for virtual port I'm currently using [loopbe1](http://nerds.de/en/loopbe1.html)

however there's also a good analogue for it: [midiyoke](http://www.midiox.com/myoke.htm)

