//
// Copyright devtodev (c) 2020.
// All rights reserved.
//

#import <Foundation/Foundation.h>

@interface DTDEventsRouter : NSObject

+ (void)getMessagingCategoriesWith:(void (^ _Nonnull)(NSArray<NSDictionary *> * _Nonnull))completionHandler;
+ (void)sendPushTokenWith:(NSString * _Nonnull)token andFlag:(BOOL)isAllowed;
+ (void)sendPushOpenedWith:(long long)timestamp withID:(long)pushId withButton:(NSString * _Nullable)button;
+ (void)sendLogMessageWith:(long)level withMessage: (NSString * _Nonnull)message;
+ (BOOL)isAnalyticsInited;

@end
