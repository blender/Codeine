#About Using Codeine

Hi all,

Science should be replicable, thus some good engineering is needed! Many papers I have read superficially (at best) describe systems and set-ups that in my humble opinion would take a huge amount of work to replicate and still you're not guaranteed to have a perfect copy.

Thankfully our set-up is replicable pretty easily, and I am pretty sure the outcome is not going to vary.

## What is needed
*	1 Mac and 1 PC  (or 1 mac running a virtual machine with Windows)
*	1 copy of Xcode and the iOS SDK (preferably iOS 5 SDK)
*	1 iOS Device (iPhone/iPod Touch/iPad running iOS 5)
*	1 working Windows 7 installation
*	1 copy of Visual Studio 2008 ( yes, 2008 NOT 2010. This is forced upon by the surface SDK. Don't waste time trying anything else even if it is possible. Not worth the effort)
*	1 copy of the Microsoft Surface 1.0 SDK Workstation Edition ( http://www.microsoft.com/download/en/details.aspx?displaylang=en&id=15532 )


## Set up

######On the Mac
*	install the iOS SDK and Xcode
*	download https://github.com/blender/pieMenu 

######On the PC
*	install Visual Studio 2008 (2008 NOT 2010)
*	install the Microsoft Surface 1.0 SDK Workstation Edition http://www.microsoft.com/download/en/details.aspx?displaylang=en&id=15532
*	install XNA redistributable as asked from the Surface SDK
*	download https://github.com/blender/Codeine

## Running (follow these steps in order)

######On the PC
*	run the Surface Simulator bundled with the surface SDK (this is to get the toolbars for moving byte tags and other contacts around)
*	open the Codeine solution, select the Codeine project and mark it as Startup project (right click, set as startup project)
*	run the code

######On the Mac
*	edit the AppDelegate.m file and insert the IP address of the machine where the surface simulator is now running
*	run the pieMenu project

## Operating

######On the PC:
*	on the Surface Simulator where Codeine is now running place a byteTag with byteValue C0 on the surface by pressing the left and right mouse buttons at the same time. C0 is the byteValue identifier for the iPad. You have now placed an iPad on the Surface.
*	insert any other value in the byteValue and move a second byteTag around the Surface. Depending of the position and orientation of the second byteTag relative to the first tag the corresponding slice on the iPad simulator will be selected and change color depending of the orientation.


That should be all!