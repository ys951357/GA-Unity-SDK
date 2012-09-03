Users across other games (iOS):

If you are making an iOS game you might want to add the OpenUDID Xcode script to your Xcode project (built from Unity), and enable the external methods in Unity (explained below). This will allow GA to track your users across different games, making it much easier for us to provide useful design and business relevant information specifically to you. For example, if your users purchase significantly less from the in game store in your game compared to the other games they play, we can show you the details.


How to enable OpenUDID for iOS:

1) At the top of the GA_GenericInfo.cs script in Unity3D, uncomment these two lines:

[DllImport ("__Internal")]
private static extern string FunctionGetOpenUDID ();

2) In the GetUUID method of the GA_GenericInfo.cs script in Unity3D, uncomment this line:

return FunctionGetOpenUDID();

3) Build the Unity3D iOS project.

4) Add the OpenUDID.h and OpenUDID.m files to the Classes folder of your Xcode project. (These files can be found in the _Game Analytics > _iOS folder of the GA Unity3D wrapper).

5) Replace the main.mm file in the Classes folder of your Xcode project with the main.mm file from the Unity3D wrapper. (This file can be found in the _Game Analytics > _iOS folder of the GA Unity3D wrapper).

6) Under "Build Settings" make sure that "Enable Objective-C Exceptions" is set to "Yes"