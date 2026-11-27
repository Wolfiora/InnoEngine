#import <AppKit/AppKit.h>
#include <stdio.h>
#include <stdlib.h>

static BOOL quitRequested = NO;

@interface JourneyApplication : NSApplication
@end

@implementation JourneyApplication
- (void)terminate:(id)sender
{
    quitRequested = YES;
}
@end

static void Write(NSString *path, NSString *text)
{
    NSError *failure = nil;
    if (![text writeToFile:path atomically:YES encoding:NSUTF8StringEncoding error:&failure]) {
        fprintf(stderr, "%s\n", failure.localizedDescription.UTF8String);
        exit(4);
    }
}

int main(int argc, const char *argv[])
{
    @autoreleasepool {
        if (argc < 3) return 4;
        NSString *project = [NSString stringWithUTF8String:argv[2]];
        NSString *mode = [NSString stringWithUTF8String:getenv("INNO_JOURNEY_PROTOCOL_CASE")];
        NSString *evidence = [project stringByAppendingPathComponent:@"Library/Acceptance"];
        NSString *logs = [project stringByAppendingPathComponent:@"Logs"];
        for (NSString *directory in @[evidence, logs]) {
            NSError *failure = nil;
            if (![[NSFileManager defaultManager] createDirectoryAtPath:directory withIntermediateDirectories:YES attributes:nil error:&failure]) {
                fprintf(stderr, "%s\n", failure.localizedDescription.UTF8String);
                return 4;
            }
        }
        Write([logs stringByAppendingPathComponent:@"EditorBoot.log"], @"Starting");
        NSMutableString *stages = [NSMutableString string];
        for (int index = 0; index < 8; index++) [stages appendFormat:@"Verified stage %d\n", index];
        Write([evidence stringByAppendingPathComponent:@"workflow-journey.tsv"], stages);
        [JourneyApplication sharedApplication];
        [NSApp setActivationPolicy:NSApplicationActivationPolicyAccessory];
        [NSApp finishLaunching];
        Write([evidence stringByAppendingPathComponent:@"journey-completed.txt"], @"Workflows");
        while (!quitRequested) {
            @autoreleasepool {
                NSEvent *event = [NSApp nextEventMatchingMask:NSEventMaskAny untilDate:[NSDate dateWithTimeIntervalSinceNow:0.1]
                                                     inMode:NSDefaultRunLoopMode dequeue:YES];
                if (event) [NSApp sendEvent:event];
            }
        }
        Write([logs stringByAppendingPathComponent:@"EditorBoot.log"],
              [mode isEqualToString:@"appkit-retirement-failure"] ? @"Dispose end.\nTeardown failure" : @"Dispose end.");
        return [mode isEqualToString:@"appkit-failed-process"] ? 2 : 0;
    }
}
