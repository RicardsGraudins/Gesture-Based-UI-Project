## Gesture-Based-UI-Project
This repository contains code and information for my fourth-year (hons) undergraduate project for the module Gesture Based UI Development. The module is taught to undergraduate students at [GMIT](http://www.gmit.ie/) in the Department of Computer Science and Applied Physics for the course [B.S.c. (Hons) in Software Developement.](https://www.gmit.ie/software-development/bachelor-science-honours-software-development). The lecturer is Damien Costello.

## Project Guidelines:
*Develop an application with a Natural User Interface.  You have a choice of technologies available to you and an opportunity to combine a lot of technology that you have worked with over the past four years. At the very least, this should be a local implementation of the application using gestures to interact with it. You can expand out to include real-world hardware and use this as an opportunity to prove a concept. The Internet of Things is a common phrase, so you could implement a solution taking advantage of hardware like the Raspberry Pi, using the cloud for data transfer and creating a realworld scenario through this medium. The programming language is your choice and there are several options including JavaScript, C#, C++ and Lua.*

## Project Purpose: Media Player Application
The purpose of this application is to provide a multi-purpose media player that can play music, videos and read text files out loud. The application can be controlled using voice commands through the use of [Cortana](https://en.wikipedia.org/wiki/Cortana), and like any other media player application - through the use of a GUI.  

## Project Research and Planning:
Initially I had considered using a [Myo armband](https://en.wikipedia.org/wiki/Myo_armband) to create a natural user interface for my main project this year - [MageCraft](https://github.com/RicardsGraudins/MageCraft). The idea was to use the armband to control the character movement and allow the player to cast various spells, however with the limited amount of hardware available as well as Myo being highly favoured as the hardware for this module by most students - I decided to base this project using a different technology - Cortana. Unfortunately Cortana could not be used with MageCraft mainly because the game is fast pace and voice commands are more suited to slower pace games where each player takes a turn. After conducting more research I decided to create a media player, and although there are voice controlled media players out there already, I wanted to see how difficult it would be to create my own variation.

## Project Design & Architecture:
The application was built in C# using Visual Studio as a [Universal Windows Platform](https://en.wikipedia.org/wiki/Universal_Windows_Platform) in order for it to work on desktop, mobile etc. The project design is an **adapted** MVVM design pattern (View, ViewModel, Model), to be more precise - the application has 3 pages (Music, Video, Reader), each of these pages have a ViewModel which in MVVM terms has both View and ViewModel responsibilities and each ViewModel uses a Model that interacts with the system and data services. When the user navigates between these pages the page the user is on loads the appropriate Cortana voice commands which allows the user to control the application vocally. Is it important to note that each page has its own set of commands and as the user switches between pages the commands get overwritten, this means that a user cannot execute commands for the video/reader pages while viewing the music page. The application was designed this way in order to keep the commands very minimal and similar such that the user does not have to learn different commands for all 3 pages but rather only commands for one that can be used across all 3 pages.

## What is Cortana:
Cortana is a virtual assistant created by Microsoft for Windows 10, Windows 10 Mobile, Windows Phone 8.1, Invoke smart speaker, Microsoft Band, Xbox One, iOS, Android, Windows Mixed Reality, and soon Amazon Alexa. Cortana can set reminders, recognize natural voice without the requirement for keyboard input, and answer questions using information from the Bing search engine. Cortana is currently available in English, Portuguese, French, German, Italian, Spanish, Chinese, and Japanese language editions, depending on the software platform and region in which it is used. Cortana mainly competes against assistants such as Apple Siri, Google Assistant, and Amazon Alexa.

## List of Commands:
Words marked with [ ] are optional and are not necessary in order to execute the command.  
Words marked with { } are labels loaded through phraselists that tell Cortana which element you specifically mean i.e. in this application we have phraselists for 'song' and 'book', these phraselists are dynamically updated when a page loads by taking the files located in Assets/Music & Assets/Books.  

To use a command say: `Hey Cortana, Media Player <Command>`  
or alternatively click Cortana search and say: `Media Player <Command>`  

Replace `<Command>` with any of the following commands from the lists, also make sure the application is running before saying the command - does not matter if its in the suspended/unsuspended state.

#### Music Page:
* `Pause [song]`
* `Pause [music]`
* `Resume [music]`
* `Play [music]`
* `[Play] next [song]`
* `[Play] previous [song]`
* `Go back`
* `Play {song}`
* `Play [a] random [song]`
* `Skip [song]`
* `Exit`
* `Close`
* `Quit`
* `[Start] background [task]`
* ` Execute order 66`

#### Video Page:
* `Resume [the] [video]`
* `Play [the] [video]`
* `Pause [the] [video]`
* `Stop [playing] [the] [video]`
* `[Go] fullscreen [the] [video]`
* `Increase [the] [video] volume`
* `Decrease [the] [video] volume`
* `Increase [the] [video] volume [by] half`
* `Decrease [the] [video] volume [by] half`
* `Exit [video]`

#### Reader page;
* `Resume [reading]`
* `Continue [reading]`
* `Pause [reading]`
* `Stop [reading] [the] [book]`
* `Increase [the] volume`
* `Decrease [the] volume`
* `Increase [the] volume [by] half`
* `Decrease [the] volume [by] half`
* `Exit [reader]`
* `Stop reading`
* `read {book}`

As you can see all of the above commands are quite similar on each page and as a result remebering the commands is quite simple.  

These commands aim to be:  
* Efficient - using as few words as possible.
* Relevant - information pertinent only to the task.
* Clear - avoids ambiguity & uses everyday language.
* Trustworthy - as accurate as possible.

By following the above 4 guidelines all of the commands are pretty much self explanatory to anyone who has ever used a media player and the fact the commands are clear and short lowers the odds of Cortana misinterpreting the command being given. Most of the commands have optional keywords that aren't necessary but it does allow the user to be more clarified if desired.

## Conclusion:
As a result of completing this project I have learned that it is relatively simple to create your own media player using the features already available in Visual Studio and that is it isn't all that difficult to integrate Cortana. The only difficulties with using Cortana for the first time is ensuring it is set up correctly on your machine and then make certain that all the proper namespaces and paths are set up in order for your application to load the voice commands which are simple to create as long as the guidelines are followed. The same can be said when setting up background features for Cortana, simply ensure that all the paths and namespaces are correct. Overall I would say that this was a great learning experience on how to use Cortana and if I were to integrate Cortana into a different application in the future it wouldn't take much time at all to set up since once you have it set up once the process is the same for all applications.

## How to run:
The project can be run by opening the solution using Visual Studio 2015 or alternatively using .apk files to download the application.  

**Note**: The application was built for Windows 10 build 10240 which was older than the current version - Windows 10 Anniversary Edition (10.0; Build 14393) since Cortana does not function as supposed to on the current Anniversary edition. In the event the project does not run as intended, switch the build settings by right clicking "MediaPlayerApplication(Universal Windows)" in Solution Explorer -> Properties -> Application -> try switching target version and/or min version and rebuilding the solution.

In order to have Cortana work with this application you need to have a Windows version that supports Cortana and have her set up in such a way that the "Cortana Language"
setting is set to English UK as well as your region set to UK. If these settings are not configured correctly Cortana will not respond correctly to the set commands
associated with this application.

## References:
* [Cortana](https://en.wikipedia.org/wiki/Cortana)
* [Cortana Interactions](https://docs.microsoft.com/en-us/windows/uwp/design/input/cortana-interactions)
* [Universal Windows Platform](https://docs.microsoft.com/en-us/windows/uwp/get-started/universal-application-platform-guide)
* [FFmpegInterop](https://github.com/Microsoft/FFmpegInterop)
