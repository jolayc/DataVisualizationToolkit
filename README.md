# Data Visualization Toolkit in Mixed Reality

## Authors
Joseph Laycano (Dynamic Plotting)  
Jason Wang (Static Plotting and Geographic Plotting)

## Dependencies 
Mixed Reality Toolkit-Unity (MRTK)
[Available here] (https://github.com/microsoft/MixedRealityToolkit-Unity)

## Setup  
1. Import MRTK into the Unity project
2. Import Data Visualization Toolkit into the project
3. Add Mix Reality Toolkit configuration into the scene
4. Open Mapbox setup menu and enter API access token
5. Run the scene (Play button) to ensure everything was configured correctly  

[Download DVT Unity Package](https://drive.google.com/file/d/1oF-D8vvg3qXeNqasi91t1Oeps-qcEKB2/view)
#### How To Video
[![Data Visualization Toolkit Demonstration Video Series - Introduction & Set up](https://i.imgur.com/f0y7q1q.png)](http://www.youtube.com/watch?v=gPphdNLEayo "Data Visualization Toolkit Demonstration Video Series - Introduction & Set up")

## Static Plotting
This module allows users to plot static points in 3D or 2D  
<img src="https://i.imgur.com/hwFQV0H.jpg" width="324" height="324"> <img src="https://i.imgur.com/HSD8MDH.jpg" width="424" height="324">

#### How To Video
[![](https://i.imgur.com/nqutXme.png)](http://www.youtube.com/watch?v=DI6Th80Ve7Y "Data Visualization Toolkit Demonstration Video Series - Static Plotting Functionality")

## Geographic Plotting
This module allows users to plot geographical data related to real world locations   
<img src="https://i.imgur.com/lfMh6C1.jpg" width="424" height="324"> <img src="https://i.imgur.com/lmoW3Aa.png" width="324" height="324">

#### How To Video
[![](https://i.imgur.com/FGxWd8s.png)](http://www.youtube.com/watch?v=SSaBtswGp24 "Data Visualization Toolkit Demonstration Video Series - Geographical Plotting Functionality Unlisted")
## Dynamic Plotting
This module allows user to plot animated dynamic points 

#### How To Video
[![](https://i.imgur.com/shKYGJB.png)](http://www.youtube.com/watch?v=abUTxeNvdwg "Data Visualization Toolkit Demonstration Video Series - Dynamic Plotting Functionality")

## Building For Hololens
1. Use UWP for building
2. Use .Net scripting backend
3. Build type D3D
4. In Publish Settings, ensure SpatialPerception and InternetClient is enabled
#### Setting up Visual Studio to Build Application 
1. Tools -> Extensions and Updates, look for _sqlite_
2. Install SQLite for Universal Windows Platform
3. Right click on References of the default project
4. Add References
5. Expand Universal Windows on the left click Extensions
6. Check SQLite for Universal Windows Platform  

<img src="https://i.imgur.com/6prfnvr.jpg" width="400" height="400"> <img src="https://i.imgur.com/UPKUPZw.jpg" width="400" height="400">
