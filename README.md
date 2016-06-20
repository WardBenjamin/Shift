# Shift

Shift is an open-source C# game development framework, along the lines of Monogame/XNA. Shift is built on top of SDL2 and OpenGL 3+, and uses the SDL2-CS bindings by [@flibitijibibo](https://github.com/flibitijibibo).

#### Origins

Shift is a spiritual successor to [Flare.Framework](https://github.com/WardBenjamin/Flare.Framework), and essentially just does everything better. It supports OpenGL 3.1 Core, equivalent to OpenGL 3.3 but with support for older hardware (down to Intel HD Graphics 3000).

#### Development status

Shift is a work in progress. New changes are targeted towards the `develop` branch, which is the default on Github, so be sure to check out `master` if you want to build a stable release version. Development builds should be usable as-is, but keep in mind that there may be API changes and other tweaks between builds.

#### Usage

To work with Shift, just add it to a C# project from Nuget (Note: not yet uploaded). The easiest way to get started is to write a base class and entry point similar to this:

            Game game = new MyGame();
            game.Run();
			
All that MyGame needs to do is to override Init, Update, and Draw, then it automagically works! You can also explicitly call a Game constructor to set the title and window size before Init, i.e.:

			public class MyGame : Game
			{
				public MyGame() : Game("My amazing game!", 800, 600) {}
				// Init, Update, Draw, ...
			}

We are still in need of samples and more advanced documentation, though most of the API is documented via XML comments. In the future, online documentation and sample applications will be available.

#### Features

Shift has a fairly XNA-like API, though not supporting some XNA features, for simplicty and ease of use. APIs do differ in many places, and Shift is still in development so more features are being added, so keep that in mind.

#### Development Policy

If you would like to contribute to Shift, just open an issue and/or a pull request and we can discuss your changes. Contributions are more than welcome!

#### License

Shift is released under the Microsoft Public License (MS-PL). See LICENSE.md for details.