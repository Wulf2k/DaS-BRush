+Using Eclipse Lua Development Tools with Dynamic Auto-completion:
+++Creating A New Lua Project in Eclipse Lua Development Tools:
* Install the LDT (Lua Development Tools) version of Eclipse: https://eclipse.org/ldt/#installation
  * Modifications to apply to an existing installation are provided as well as standalone pre-modded versions
  * Note that Eclipse *does not install*, running 100% portably from wherever you extract it.
* Setup your Eclipse workspace in whichever directory you'd like
* Go to `File` -> `New` -> `Lua Project` and complete the wizard-thing:
  * Note: any time you see a dropdown box which lets you select between Lua 5.1 and Lua 5.2, be sure to set it to 5.2 (since DaS.ScriptLib uses that)
  * Uncheck the "Create default template project ready to run" box under Targeted Execution Environment.
+++Adding the Necessary Lib Folder to Your Lua Project:
* Right-click your project's root node within the "Script Explorer" panel on the left and go to "Properties".
* Go to the "Build Path" Page, which is listed underneath the "Lua" dropdown on the left of the Properties window.
* Go to the "Libraries" tab.
* Click the "Add Library..." button on the right.
  * Select "Lua User Libraries" as the library type to add.
  * Click "Next".
  * Add DaS.ScriptLib to the User Libraries list:
    * Click the "Configure..." button on the right to open the *real* User Libraries window.
	  * Click the "New..." button on the right
	  * Put "DaS.ScriptLib" as the name of the new user library.
	  * Check the "Add to environment" checkbox
	  * Click OK.
	* Leave the *real* User Libraries window open for now.
    * Click DaS.ScriptLib within the *real* User Libraries to highlight it
    * Click the "Add External folder..." button on the right.
    * Browse to the `<DaS-BRush Repository Folder>\DaS.ScriptLib\Lua Headers` directory.
    * Click OK.
      * You should now see the directory you just added listed underneath DaS.ScripLib's node in the *real* User Libraries list.
	* Click the "Apply and Close" button on the *real* User Libraries window.
    * DaS.ScriptLib should now show up in the "Add Library" wizard.
  * Check the box next to DaS.ScriptLib and click "Finish" to finish the "Add Library" wizard thing.
* Click "Apply and Close" on the project properties window.
+++Creating a New Script:
* Right-click the "src" folder under your Lua project in the "Script Explorer" panel.
* Go to `New` -> `Lua File`
* Enter a name for your script.
* Click "Finish" to end the totally-necessary 1-step wizard.
* To test if the autocomplete is working:
  * Type in: "player"
  * Add "." to the end
  * An auto-complete list should come up listing player's variables. ![Image of autocomplete list](https://i.imgur.com/7apBUZq.png)
+++Adding DaS.ScriptLib as a Lua Interpreter So That the "Run" Button In Eclipse Actually Runs Your Script:
* On the menu bar, go to `Window` -> `Preferences`
* Go to the "Interpreters" page, underneath the "Lua" dropdown on the left.
* Click the "Add..." button on the right.
  * For the interpreter type, select "Lua 5.2"
  * For the interpreter executable, browse to the `Dark Souls Script Executor.exe` file in your copy of DaS-BRush (Will be in "<repository>\Bin\Debug" if you build the project solution yourself.
  * For the Interpreter name, put DaS.ScriptLib.Executor
  * Make sure the "Interpreter arguments" box is empty (delete the text).
  * Make sure Linked Execution Environment is set to "lua-5.2"
* Check the box next to DaS.ScriptLib.Executor in the Interpreters list
  * DaS.ScriptLib.Executor should be the only one checked now.
* Click "Apply and Close" on the Preferences window
+++Weird Eclipse "Gotchas":
* Pressing the Tab key when the autocomplete menu is up will not select the text and add it, but rather, will focus the autocomplete menu, requiring you to use your mouse to re-click on the text area to continue typing. So far I haven't figured out how to change that behaviour. In the meantime, you can press the `Enter` key to select the autocomplete entries!
* Neither Alt + mouse drag nor middle click + mouse drag begin a block selection. If there is a block select function in Eclipse it's extremely well hidden!
* The "Stop" button for scripts when running them is located in the `Console` panel on the bottom.
  * To switch between Consoles for multiple scripts, you must click the second button on the top-right of the console panel (which either shows a list of all currently-running scripts' consoles or cycles through the currently selected one, depending on where you click it): ![Image of the button](https://i.imgur.com/Yrlzp2n.png)