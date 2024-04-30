//
// Copyright devtodev (c) 2020.
// All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UserNotifications/UserNotifications.h>

@interface UnityPushManager : NSObject

typedef void (*stringDelegate)(const char *message);

+(UnityPushManager *) pushManager;

- (void) pushIsAllowed:(bool) state;
- (bool) getPushState;
- (void) initPushManager;
- (const char*) getToken;
- (void) setPushNotificationOptions: (unsigned int) options;
- (void) setOnRegister: (stringDelegate) onRegistred
              onFailed: (stringDelegate) onFailed
           onInvisible: (stringDelegate) onInvisible
          onForeground: (stringDelegate) onForeground
          onPushOpened: (stringDelegate) onPushOpened;
@end
