//
// Copyright devtodev (c) 2020.
// All rights reserved.
//

#import <UIKit/UIKit.h>
#import <UserNotifications/UserNotifications.h>
#import <UnityPushManager.h>

//! Project version number for messaging.
FOUNDATION_EXPORT double messagingVersionNumber;

//! Project version string for messaging.
FOUNDATION_EXPORT const unsigned char messagingVersionString[];

// In this header, you should import all the public headers of your framework using statements like #import <messaging/PublicHeader.h>
#ifdef __cplusplus
extern "C"{
#endif
    const char* cCopy(const char* string);
    void setNotificationOptions(unsigned int options);
    void startNotificationService();
    void pushIsAllowed(bool state);
    bool getPushState();
    const char* getToken();
    void setPushListener(stringDelegate onRegistred,
                         stringDelegate onFailed,
                         stringDelegate onInvisible,
                         stringDelegate onForeground,
                         stringDelegate onPushOpened);
#ifdef __cplusplus
}
#endif
