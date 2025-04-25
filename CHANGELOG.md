## [3.9.1] - 2025-23-04

- Fixed SetCheater the method for the iOS platform
- Fixed missing amsdef files
- Improved compatibility with other plugins
- Added the SetInitializationCompleteCallback method for WebGL platform
- Changed the way COPPA is enabled for the iOS platform

## [3.9.0] - 2025-02-04

- Implemented automatic payment tracking system for Android and iOS platforms
- Fixed dependency configuration issues for the WSA platform
- Fixed WebGL build failure on the Linux platform

## [3.8.5] - 2025-02-04

- Added A/B test functionality for WebGL platform
- Removed deprecated functions for WebGL platform
- Fixed obfuscation conflicts for Android platform

## [3.8.4] - 2024-10-24

- Fixed http client for WSA(UWP) platform
- Fixed dependencies for android platform

## [3.8.3] - 2024-05-16

- Fixed an error occurring during validation in app store
- Privacy Manifest moved to Plugins folder for correct merging by unity tools
- Fixed post process behavior for newer versions of Unity

## [3.8.2] - 2024-05-08

- Added SDK’s Privacy Manifest for iOS
- Added XCFramework Signature for iOS
- Removed support for the armv7 architecture for iOS
- Fixed an issue where xcode was finding already deleted methods related to personal information

## [3.8.0] - 2024-03-18

- Added linux editor support
- Fixed the OnReceived method in the Ab-test logic
- Removed methods related to the personal information

## [3.7.1] - 2023-12-21

- Hotfix for guid conflict.

## [3.7.0] - 2023-12-19

- Added DeviceType field to the user card.
- Fixed problem with Exclude field in Analytics configuration.
- Added data compression using the Zstd library for Android, MacOS, iOS, Windows standalone platforms.
- DTDRemoteConfigReceiveResult now has an empty state, meaning that the A/B test configuration was received but the list of experiments is empty.

## [3.6.5] - 2023-09-25

- Fixed a session tracking issues for win standalone platform.
- Fixed a crash occurred in the editor on the macOS platform if the application key was empty

## [3.6.4] - 2023-07-19

- Implemented devtodevId update mechanism
- A/B-test module minor fixes.
- Fixed a bug that occurred when the RemoteConfig module was called before the Analytics module.

## [3.6.3] - 2023-06-9

- Added xCode 14.3 support.
- Fixed the logic of the CustomEvent method when it was called without parameters.

## [3.6.2] - 2023-06-2

- Fixed Ab-test logic.
- Fixed the logic of the AddImpression method.
- Updated Android dependencies.

## [3.6.1] - 2023-05-5

- Removed support for bitcode for Apple platforms(deprecated in Xcode 14).
- Fixed Ab-test logic for Android platform.

## [3.6.0] - 2023-04-23

- Fixed SQLiteConstraintException for Android platform.
- Fixed the logic of opening push notifications from different push campaigns for IOS & Android platforms.
- Added functionality that allows you to bring the application into compliance with COPPA (Children's Online Privacy Protection Act) for WSA platform.
- Added more methods for working with DTDRemoteConfig that will facilitate integration and testing for WSA & Windows standalone platforms.
- Minor improvements and fixes, stability enhancements.

## [3.5.0] - 2023-01-31

- Added functionality that allows you to bring the application into compliance with COPPA (Children's Online Privacy Protection Act) for Android platform.
- Added more methods for working with DTDRemoteConfig that will facilitate integration and testing.
- Huawei services support for Android platform.
- Minor changes in the A/B testing module for Apple and Android platforms.
- Fixed problems with SQLlite for Apple and Android platforms.
- Minor improvements and fixes, stability enhancements.

## [3.4.0] - 2022-11-23

- Added A/B test functionality (Beta) for Windows and WSA(UWP) platforms.
- Added functionality that allows you to bring the application into compliance with COPPA(Children’s Online Privacy Protection Act) for IOS and MacOS platforms.
- Added more methods for working with DTDRemoteConfig that will facilitate integration and testing.
- Fix dispatching in DTDRemoteConfigListener for android platform.
- Minor improvements and fixes, stability enhancement.

## [3.3.2] - 2022-10-21

- Added an option for assigning a callback to receive a notification when the SDK initialization is complete. (Not available for WebGL platform)
- In order to improve reliability, the amount of stored and transmitted data about game currencies/resources and user card parameters has been limited for all platforms.
- Minor improvements and fixes, stability enhancement.
- Fixed a bug that caused Duplicate class a.a found in modules on Android platform.

## [3.3.1] - 2022-09-26

- In order to improve reliability, the amount of stored and transmitted data about game currencies-/resources and user card parameters has been limited on Android platform.
- Fixed a problem with SessionService on Android platform.
- Fixed problems with SQLlite on Android platform.

## [3.3.0] - 2022-09-02

- Added A/B test functionality (Beta) for Android and IOS platforms.

## [3.2.1] - 2022-08-26

- Fixed a bug that caused android.os.DeadObjectException on Android.
- Updated dependencies
- Changed logic of working with the ‘Cheater’ mark, removed the GetCheater method
- Added logic of working with the ‘Tester’ mark, added the SetTester method
- Improved Android push notification performance, and fixed minor bugs
- Fixed a bug that caused undefined symbol: $callbackPointer when enabling logs on WebGl

## [3.2.0] - 2022-06-06

- Added Unity WebGL support.
- Fixed migration issues from previous SDK generation.
- Fixed possible freezes when requesting IDFA permissions on some iOS versions.
- Fixed issues receiving empty objects OnPushNotificationOpened and OnPushNotificationReceived.
- Fixed possible crashes while initializing push notifications after clearing app data.
- Fixed a rare bug that caused an AsyncOperationDispatcher to be saved to scene.
- Fixed anti-cheat on Android platform
- Fixed a potential error that can cause ANR when receiving an install referrer on Android platform
- Fixed an error causing crash when working with double parameters in user cards on Windows standalone and WSA (UWP) platforms
- Fixed an error causing the int type to get substituted with bool type when it equals to 0 or 1 on iOs/MacOs platforms
- Fixed obfuscation rules that may cause conflicts with other frameworks on Android platform
- The version of native dependencies .net framework was downgraded to 4.6.1 for the Windows standalone platform

## [3.1.1] - 2022-04-11

- Added new features: subscription payments monitoring and analytics
- Added a new event - currentBalance
- Push notification support on Android 12+
- Minor improvements and fixes, stability enhancement
- Improved lifecycle tracking on Android 12+
- Referral data collection from Facebook
- Please note that to collect referral data, you need to manually add the dependency '< androidPackage spec=“com.android.installreferrer:installreferrer:2.2” />' into the file AppDependencies.xml
- Warning: This package does not support WebGL. WebGL support will be added in the next releases

## [3.0.2] - 2022-01-17

- Increased SDK speed
- Fixed an operation that caused the "AndroidJNIHelper.GetSignature: using Byte parameters is obsolete, use SByte parameters instead" warning
- Fixed a bug that prevented debug logs disabling
- Added asmdef files
- Fixed conflicts with other plugins (Firebase, ARCore)
- Сhanged the version of Newtonsoft json to 12.0.3.

## [3.0.1] - 2021-12-23

- Android. Fixed data inheritance error when switching from SDK v2
- iOS: Added managing permission method to send push notifications

## [3.0.0] - 2021-12-10

- This package does not support WebGL. WebGL support will be added in the next releases.
