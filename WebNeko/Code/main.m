//
//  main.m
//  Neko
//
//  Created by Trevor Zhenya D'Arcy-Evans on 05/05/2010.
//  Copyright __MyCompanyName__ 2010. All rights reserved.
//

#import <UIKit/UIKit.h>

int main(int argc, char *argv[]) {
    
    NSAutoreleasePool * pool = [[NSAutoreleasePool alloc] init];
    int retVal = UIApplicationMain(argc, argv, nil, @"NekoAppDelegate");
    [pool release];
    return retVal;
}
