[Download and Setup](http://support.gameanalytics.com/entries/22584741-Download-and-setup)

NOTE: To use this GIT repository in your Unity game please add the contents directly to your Assets folder.

### Recent Changes:

##### v.0.4.5:

- Fixed a bug with creating new GA prefabs after changing folder structure.
- Fixed a reference exception with the GA logo texture.
- Fixed a JSON formatting error that would occur with exceptions containing a " symbol.

##### v.0.4.4:

- New menu options to upload / delete mesh AssetBundles for use with the new browser heatmap tool.
- New menu option to update folder structure for compatibility with UnityScript (JavaScript) and Boo (menu: GameAnalytics > Folder Structure > Switch to JS).
- New menu option to revert to original folder structure (menu: GameAnalytics > Folder Structure > Revert to original).
- Added build option to heatmaps.
- Added "Select for all builds" option to heatmaps.
- Fixed some minor issues with the heatmap histogram display for heatmaps with negative values.
- Fixed a minor encoding issue with message authorization.
- Changed Average FPS and Critical FPS events to Design events (instead of Quality events). This will allow you to view the average values in our online tool.
- Changed the class name of our MiniJson implementation to GA_MiniJson, to avoid conflicts with other plugins.
- Due to a bug with the Solid render model currently not supporting negative values, it has been removed as an option from heatmaps for now.

##### v.0.4.2:

- Average FPS (frames per second) events are now sent on every data submit interval (set under the Advanced tap of GA_Settings), instead of when the scene changes.
- Added an option for starting a new session on mobile games (iOS and Android), when the game is paused (minimized) and resumed.
- Minor improvements to the way exceptions are submitted.
- Minor changes to the file and folder structure of the GA Unity SDK package.
- Clarified some pop-up help text.
- Added support for iOS advertiser ID (see GameAnalytics > Plugins > iOS > iOS_Readme.txt).
- A few additional bug fixes and minor tweaks.

##### v.0.4.1:

- Fixed a bug with user ID generation in v.0.4.0

##### v.0.4.0:

- Added Flash Player support. As a result many code optimisations have been made, which should also improve performance even more on other platforms.
- Added GameAnalytics gizmos and logos to GA objects, for easier identification in the hierarchy and scene view.
- Fixed an issue with building to iOS with the Stripping Level "Use micro mscorlib". Please see the iOS considerations here: https://gameanalytics.zendesk.com/entries/23064016-Tips#player
- GA_HeatMap: Added a grid size for heatmaps under the advanced tab of GA_Settings. Data must be visualised with the same grid size as is used during collection.
- GA_HeatMap: Fixed an issue where the download button would not be re-enabled if the download failed.
- GA_HeatMap: Fixed an issue where heatmap histograms with too low sample count would look weird.
- GA_Tracker: Added custom interval to BreadCrumb event

##### v.0.3.7:

- Minor bug fixes.

##### v.0.3.5:

- New heatmap features, such as: add/subtract heatmaps, and color presets.
- Heatmap download indicator.
- Custom area.
- A ton of small fixes and improvements.

##### v.0.3.2:

- Fixed an issue with building projects on certain platforms.

##### v.0.3.1:

Complete revamp of the GA Unity Package, including:
- Huge performance boost.
- Completely new structure (no longer requires a GA prefab).
- 3D Heatmaps.
- Setup Wizard, Example game + Exampleg game tutorial.
- Status Window.
- System Tracker + Object Tracker.
- And lots more!

##### v.0.2.2:

- Fixed a performance issue with checking for network connectivity.

##### v.0.2.1:

- Restructuring of the inspector view.
- Added Drag'n'Drop event tracking for basic events.
- Improved QA functions, including error handling and tracking.
- Lots of minor tweaks, improvements and bug fixes.

# Game Analytics Unity3D Wrapper

The Game Analytics Unity3D wrapper is a plugin for Unity which makes it easy to send game event data to the Game Analytics service for visualization. By collecting data from players playing your game you will be able to identify bugs and balance issues, track purchases, and determine how your players really play your game!

##### Some key features:

- Helps you track user, game design, quality assurance, and business events in your game.
- Options for automatically tracking level transitions, errors and exceptions, and system specifications.
- Option for archiving data when the user is playing offline. Archived data will be sent next time the user plays with an internet connection.

The code for the Game Analytics Unity3D wrapper is open source - feel free to create your own fork, or use the code to develop your own wrapper.

### Game Analytics Full Documentation

You can find the [full documentation](http://beta.gameanalytics.com/docs) on the Game Analytics website for wrappers, and RESTful API, as well as information about how to collect useful data for your game.

### Game Analytics Website

The Game Analytics website can be found [here](http://beta.gameanalytics.com/). To use the Unity3D wrapper you will have to create an account on the website, and add your new game.
