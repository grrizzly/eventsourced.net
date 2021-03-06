# EventSourced.Net
Getting Started with ASP.NET MVC6, Event Sourcing, CQRS, Eventual Consistency & Domain-Driven Design, *not* Entity Framework.

- [Up and running](#up-and-running)
  - [On MacOS](#on-macos)
  - [On Windows](#on-windows)
    - [Without Visual Studio](#without-visual-studio)
    - [With Visual Studio](#with-visual-studio)
  - [Troubleshooting (all platforms)](#troubleshooting)

## Up and running
When you're ready to clone the repository:

    cd /path/to/local/working/copy/parent # where ever that may be
    git clone https://github.com/danludwig/eventsourced.net.git
    cd eventsourced.net

### On MacOS
##### .NET Version Manager
The first thing you will need to run this app on a mac is the .NET Version Manager (`dnvm`). To find out if you have it installed already, run the following in a terminal window:

    dnvm

If the reponse tells you that the dnvm command was not found, [download & install the ASP.NET 5 pkg from get.asp.net](https://get.asp.net). After the installation has finished, close the terminal window, open a new one, and run the above command again to confirm that it is installed and available on your environment path.

##### Mono
You will also need Mono, since this app currently does not target the .NET core50 framework. To find out if you have mono installed, run the following in a terminal window:

    mono -V

If the response tells you that the mono command was not found, or reports back a version less than 4.2, [download & install a Mono pkg for version 4.2 or higher](http://www.mono-project.com/download/). After the installation has finished, close the terminal window, open a new one, and run the above command again to confirm that it is installed and available on your environment path.

##### Runtime 1.0.0-rc1-final
The .NET Version Manager installer will install the latest runtime and make it the default. This app currently targets runtime `1.0.0-rc1-final`, which you will need to have installed and available on your environment path. To find out which runtime is the default, run the following in a terminal window:

    dnvm list

If you do not see an entry with Version `1.0.0-rc1-final` & Runtime `mono` in the listing, run the following to install it:

    dnvm install 1.0.0-rc1-final

After the installation has finished, run `dnvm list` again in a *new* teriminal window. If you see that Version `1.0.0-rc1-final` is no longer the Active version, run the following:

    dnvm use 1.0.0-rc1-final -persistent

Unless otherwise specified, any other documentation about the `dnu` or `dnx` commands in this readme will assume that version `1.0.0-rc1-final` is the currently Active version in the environment path, according to `dnvm list`.

##### ArangoDB
Before you can run the app, you will need to start an ArangoDB instance at [http://localhost:8529](http://localhost:8529). *For a better development experience, it is recommended that you at least [clone this repository](#up-and-running) to a local working copy before installing the ArangoDB app (read Advanced Options below)*. Follow these instructions to install the free ArangoDB app when you're ready:

- Start the App Store app, type `ArangoDB` into the search box and hit <kbd>return</kbd>.
- The search should return 1 result for `ArangoDB Developer Tools`. Click `GET`, then click `INSTAL APP`.
- Sign in with your Apple ID and wait for the installation to complete.
- When the app is finished installing, click `OPEN`.
- Read the prompt, and then click the `New Instance` button.
- If you aren't sure what to name the instance, name it `EventSourced.Net`. 
- **Make sure you change the port from `8000` to `8529`**. If you do not, you will have to change a [small bit of code](https://github.com/danludwig/eventsourced.net/blob/7a6fc13990d45b84fb70758281a0cae722302195/src/EventSourced.Net.Services/Services/Storage/ArangoDb/Settings.cs#L8) so that the app knows where to find the database.
- Advanced Options: Although you can leave these blank, you will have a better development experience if you specify the `Database Directory` and `Log File`. To keep these consistent with the EventStore database, create 2 new folders at `devdbs/ArangoDB/db` and `devdbs/ArangoDB/log` relative to the root of your local working copy of the app repository. You can run the following commands in a terminal window to create the folders before selecting them in the Advanced Options section of the ArangoDB new instance dialog:

```
cd /path/to/local/working/copy/of/eventsourced.net # where ever that may be
mkdir devdbs
cd devdbs
mkdir ArangoDB
cd ArangoDB
mkdir db
mkdir log
```

- You can also change the `Log Level` and uncheck the `Run on Startup` box if you prefer.
- Finally, click the `Create` button to create the default instance.

Whenever the ArangoDB app is running on your mac, you can access it using the avacado icon in your menu bar. If you don't see the icon in your menu bar, start the app by launching it from your `Applications` folder or by using `Launchpad`.

After setting up a default ArangoDB instance, you will still have to start it:

- Click the avacado icon in your mac's menu bar.
- Select the name of the instance you created (for example, `EventSourced.Net`).
- Click `Start`.

When the ArangoDB instance is up and running, you should be able to access its Admin Interface at [http://localhost:8529](http://localhost:8529).

##### Restore nuget package dependencies
The first time you clone the repository, and each time a nuget package dependency is added, removed, or changed, you will need to run the following at the repository root folder in a terminal window:

    cd /path/to/local/working/copy/of/eventsourced.net # where ever that may be
    dnu restore

Note that restoring packages may take a couple of minutes when run for the first time.

##### Work around a known `FileSystemWatcher` issue
There is just one last thing you need to do before you can run the app. There is [a pretty well known issue about the MVC file watcher](https://github.com/aspnet/Mvc/issues/2348) that will probably snag you. To avoid it, run the following:

    export MONO_MANAGED_WATCHER=false

##### Launch the app
Run the following in a terminal window from the `src/EventSourced.Net.Web` project directory to start the app in a web server:

    cd src/EventSourced.Net.Web
    dnx web

- Be patient. There is a lot that happens during the first app run. It will start up much faster next time.
- Finally, navigate to [http://localhost:5000](http://localhost:5000) in your favorite web browser.

### On Windows
This app uses the  [EventStore database](https://geteventstore.com/), which when not run as an administrator (or started up from another program running as administrator) will likely encounter an error while trying to start its HTTP server at  [http://localhost:2113](http://localhost:2113). After running the following command once as administrator, starting EventStore normally should not cause this error:

    # Feel free to change the user if you want to and you know what you're doing.
    netsh http add urlacl url=http://localhost:2113/ user=everyone

If you ever want to undo the above command, run this (also once as administrator):

    netsh http delete urlacl url=http://localhost:2113/

#### Without Visual Studio
##### .NET Version Manager
Run the following in either command prompt or powershell to check if the .NET Version Manager is installed:

    dnvm

If dnvm is not recognized, you should [install it by running this command **in powershell**](https://github.com/aspnet/Home/blob/dev/README.md#powershell).

If you encounter an error while running the install command, check `Get-ExecutionPolicy`. If it is `Restricted`, start a new powershell window *as administrator* and change it using `Set-ExecutionPolicy RemoteSigned`. Repeating the dnvm powershell install command should then succeed. If you'd like to, use `Set-ExecutionPolicy Restricted` to restore the previous execution policy after installation is complete.

After installing you should close the installer window, open a new command prompt or powershell window, and run the `dnvm` command again to confirm that it has been installed.

##### Runtime 1.0.0-rc1-final
The .NET Version Manager installer will install the latest runtime and make it the default. This app currently targets runtime `1.0.0-rc1-final`, which you will need to have installed and available on your environment path. To find out which runtime is the default, run the following in command prompt or powershell:

    dnvm list

If you do not see an entry with Version `1.0.0-rc1-final` in the listing, run the following to install it:

    dnvm install 1.0.0-rc1-final

After the installation has finished, run `dnvm list` again in a new command prompt or powershell window. If you see that Version `1.0.0-rc1-final` is no longer the Active version, run the following:

    dnvm use 1.0.0-rc1-final -persistent

Unless otherwise specified, any other documentation about the `dnu` or `dnx` commands in this readme will assume that version `1.0.0-rc1-final` is the currently Active version in the environment path, according to `dnvm list`.

##### Restore nuget package dependencies
The first time you clone the repository, and each time a nuget package dependency is added, removed, or changed, you will need to run the following at the repository root folder in either command prompt or powershell:

    cd /path/to/local/working/copy/of/eventsourced.net # where ever that may be

Note that restoring packages may take a couple of minutes when run for the first time.

##### Launch the app
Run the following in commmand prompt or powershell from the `src/EventSourced.Net.Web` project directory to start the app in a web server:

    cd src/EventSourced.Net.Web
    dnx web

- Be patient. There is a lot that happens during the first app run. It will start up much faster next time.
- When prompted about allowing `Network Command Shell` to make changes to your PC, click `Yes`.
- When prompted by the Windows Firewall about allowing dnx.exe to communicate on private networks, click `Allow access`.
- Finally, navigate to [http://localhost:5000](http://localhost:5000) in your favorite web browser.

#### With Visual Studio
To run in Visual Studio, you will need at least version 2015 with Update 1 installed. Don't try to open the solution or any of the project files unless you're sure you also have ASP.NET 5 RC installed, as described below.

##### Download & install ASP.NET 5 RC if necessary
- Start Visual Studio.
- Type <kbd>Ctrl</kbd>+<kbd>Shift</kbd>+<kbd>N</kbd> to create a new project.
- Select `Templates/Visual C#/Web -> ASP.NET Web Application` and click `OK`.
- If you see 3 items under the `ASP.NET 5 Templates` section, cancel out of all dialogs and proceed to the next step.
- If instead you see a single `Get ASP.NET 5 RC` item under the `ASP.NET 5 Templates` section, select it and click OK. This will automatically download an additional exe file that you will need to run in order to install some things. Note you will have to close all instances of Visual Studio for the installer to complete. When finished, repeat the above steps to confirm you can create a new project using one of the 3 `ASP.NET 5 Templates`.

##### Open the solution
The first time you open [this app's solution file](https://github.com/danludwig/eventsourced.net/blob/master/EventSourced.Net.sln), you may be prompted to install a DNX SDK verion ending in `1.0.0-rc1-final`. Be sure to click `Yes` at this prompt. If for some reason the install fails, close the solution, open a command prompt or powershell window, run `dnvm install 1.0.0-rc1-final` and re-open the solution.

##### Build the solution
Keep an eye on the Solution Explorer and wait for it to finish `Restoring packages` if necessary. Once idle, type <kbd>Ctrl</kbd>+<kbd>Shift</kbd>+<kbd>B</kbd> to build the solution. If for some reason the build fails, open a command prompt or powershell window, navigate to the root of your working copy clone, run `dnu restore` and rebuild the solution.

##### Start the EventSourced.Net.Web project for the first time
- In Solution Explorer right-click `Solution 'EventSourced.Net' (6 projects)` and select `Properties`.
- Make sure the `Single startup project` radio button is selected, the `EventSourced.Net.Web` item is selected in the drop down, and then click `OK`.
- Type <kbd>F5</kbd> to start the app.
- Be patient. There is a lot that happens during the first app run. It will start up much faster next time.
- When prompted about allowing `Network Command Shell` to make changes to your PC, click `Yes`.
- When prompted by the Windows Firewall about allowing dnx.exe to communicate on private networks, click `Allow access`.

### Troubleshooting
The very first time you start up the app (using either `dnx web`, OmniSharp, or Visual Studio), it will automatically try to download a (couple of) compressed file(s) containing the database server(s). On windows both databases will be downloaded, but for MacOS, only one is currently automated. Next, the app will install each database by extracting its compressed file to the `devdbs` folder in your working copy of the repository, then start up each server. How long this takes will depend on your platform, network bandwidth and machine performance, but shouldn't take longer than a minute or two once the zip files are downloaded.

If you would like to monitor the progress of this, navigate to the `devdbs` folder which will be created in the root of your working copy of the repository. On both platforms it should create a subfolder under `devdbs` for `EventStore`, whereas on windows it will also create a subfolder for `ArangoDB`. If you delete these folders, the app will recreate them the next time its web server is started.

If you encounter any errors, try running the app at least one more time before [posting an issue here in GitHub](https://github.com/danludwig/eventsourced.net/issues). There can be race conditions during the very first run while the databases are set up. Once they are set up, starting the app should be as simple as running `dnx web` from the `src/EventSourced.Net.Web` repository directory, or F5 from Visual Studio.
