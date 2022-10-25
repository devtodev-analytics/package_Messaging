//
// Copyright devtodev (c) 2020.
// All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UserNotifications/UserNotifications.h>

@interface UnityPushManager : NSObject

@property (nonatomic, strong) NSString * pushListener;

+(UnityPushManager *) pushManager;

- (void) pushIsAllowed:(bool) state;
- (bool) getPushState;
- (void) initPushManager;
- (const char*) getToken;
- (void) setPushNotificationOptions: (unsigned int) options;

@end
