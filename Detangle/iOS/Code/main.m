//
//  main.m
//  Detangle
//
//  Created by Trevor D'Arcy-Evans on 02/06/2011.
//  Copyright __MyCompanyName__ 2011. All rights reserved.
//

#import <UIKit/UIKit.h>

int main(int argc, char *argv[]) {
    
    NSAutoreleasePool * pool = [[NSAutoreleasePool alloc] init];
    int retVal = UIApplicationMain(argc, argv, nil, @"DetangleAppDelegate");
    [pool release];
    return retVal;
}
