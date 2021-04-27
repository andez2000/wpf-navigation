# wpf-navigation

This is a small set of examples on how to navigate the UI in WPF.  This is still work in progress - just a repo to dump some thoughts and experiences.  

The idea is to try and not use any 3rd party libraries.  This project has been written using Jetbrains Rider.

## Samples

### 1. Codebehind Navigation

#### Codebehind Uri Navigation

Uses simple code behind in the window to navigate pages within a frame.

#### Codebehind Uri Navigation External

Uses simple code behind in the window to navigate pages outside of the application within a frame.

### 2. Services in Monolith

#### Monolith Navigation

Introduces a simple `PageNavigationService` to navigate between views within a frame.

#### Monolith Route Navigation

This is work in progress.  The purpose is to navigate to named routes in the wpf application as opposed to Uris.  The api for this has currently been developed in the `wpf-tdd` project


### 3. Test Driven WPF

This project came about from the above.  Given the turnaround time of stop starting the application to write a navigation service off the top of my head where I had never done one before 
seemed pointless.

Therefore this project is geared up for `Named Route` and `Uri` navigation.

So this project uses a combination of:

* Xunit.StaFact - allows running of WPF resources in tests
* Autofac - uses the DI to new up new pages and view models when the view and view model is resolved

The concept diagram can be found in the [wiki](https://github.com/andez2000/wpf-navigation/wiki/TDD-Concept).

