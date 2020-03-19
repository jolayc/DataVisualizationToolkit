# Data Visualization Toolkit in Mixed Reality

## Authors
Joseph Laycano (Dynamic plotting)  
Jason Wang (Static point Plotting and Geographic Plotting)

## Dependencies 
Mixed Reality Tool Kit (MRTK)

## Setup  
1. Import MRTK into the project
2. Import Data Visualization Toolkit into the project
3. Add Mix reality tool kit config to the scene
4. Enter your access token for mapbox into the mapbox setup menu
5. run the scene to ensure everything was added correcly  

[Download DVT Unity Package](https://drive.google.com/file/d/1oF-D8vvg3qXeNqasi91t1Oeps-qcEKB2/view)
#### How To Video
[![Data Visualization Toolkit Demonstration Video Series - Introduction & Set up](https://i.imgur.com/f0y7q1q.png)](http://www.youtube.com/watch?v=gPphdNLEayo "Data Visualization Toolkit Demonstration Video Series - Introduction & Set up")

## Static Plotting
This module allows users to plot static points in 3D or 2D  
<img src="https://i.imgur.com/hwFQV0H.jpg" width="324" height="324"> <img src="https://i.imgur.com/HSD8MDH.jpg" width="424" height="324">

#### How To Video
[![](https://i.imgur.com/nqutXme.png)](http://www.youtube.com/watch?v=DI6Th80Ve7Y "Data Visualization Toolkit Demonstration Video Series - Static Plotting Functionality")

## Geographic Plotting
This module allows users to plot data related to real world locations   
<img src="https://i.imgur.com/lfMh6C1.jpg" width="424" height="324"> <img src="https://i.imgur.com/lmoW3Aa.png" width="324" height="324">

#### How To Video
[![](https://i.imgur.com/FGxWd8s.png)](http://www.youtube.com/watch?v=SSaBtswGp24 "Data Visualization Toolkit Demonstration Video Series - Geographical Plotting Functionality Unlisted")
## Dynamic Plotting
This module allows user to plot animated points  

#### How To Video
[![](https://i.imgur.com/shKYGJB.png)](http://www.youtube.com/watch?v=abUTxeNvdwg "Data Visualization Toolkit Demonstration Video Series - Dynamic Plotting Functionality")

## Building For Hololens
1. use UWP for building
2. use .Net scripting backend
3. Build type D3D
4. In publish settings ensure SpatialPerception and InternetClient is enabled
#### setting up Visual studio to build 
1. tools-> Extensions and updates look for sqlite
2. install SQLite for Universal Windows Platform
3. Right click on references of the default project
4. Add references
5. Expand Universal Windows on the left click Extensions
6. check SQLite for universal Windows Platform  
<img src="https://i.imgur.com/6prfnvr.jpg" width="400" height="400"> <img src="https://i.imgur.com/UPKUPZw.jpg" width="400" height="400">
