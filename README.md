# Building Dark Souls Scripting Library
* Launch Visual Studio 2017 **as an administrator** (right click -> "Run as administrator"
  * The explicit administrative privelages are required for strong-key-signing the compiled executable for NeoLua, the Lua library that DaS.ScriptLib uses.
    * This may change in the future if, upon concluding without a doubt that no NeoLua source code edits will be required, we decide to simply bundle a pre-built version of NeoLua instead.
  * If done correctly, Visual Studio's title bar will confirm the administrative permissions: ![Image of Visual Studio title bar when ran as administrator](https://i.imgur.com/mSnKC2a.png)
* Open the `DaS.BossRush.sln` solution file.
* Click `Build` -> `Build Solution` in the menu bar, press F6, etc. to build the project.
  * If you get several `Metadata file 'x' could not be found` / `could not find library 'x'` / etc errors along with one `Error signing assembly -- Access is denied.` error, then the instance of Visual Studio from which you are attempting to build the solution does not *actually* have the required administrative permissions (if you're clicking "run as administrator" and still having issues, you'll have to seek Windows-specific help for that).

  ## Using Eclipse Lua Development Tools with Dynamic Auto-completion:
### Creating a new Lua Project in Eclipse Lua Development Tools:
* Install the LDT (Lua Development Tools) version of Eclipse: https://eclipse.org/ldt/#installation
  - Modifications to apply to an existing installation are provided as well as standalone pre-modded versions
  - Note that Eclipse *does not install*, running 100% portably from wherever you extract it.
* Setup your Eclipse workspace in whichever directory you'd like
* Go to `File` -> `New` -> `Lua Project` and complete the wizard-thing:
  * Type in a name for the project in the box next to "Project name:".
  * Under the "Targeted Execution Environment" group box:
    * make sure the "Select one:" option is ticked and select "lua-5.2" from the dropdown box (since DaS.ScriptLib uses 5.2)
    * Uncheck the "Create default template project ready to run" box (unless you want an almost-empty file in your project named `main.lua`).
  * Under the "Target Grammar" group box:
    * Make sure the "Default one:" option is ticked. Since you previously selected "lua-5.2" as your Targeted Execution Environment, it should show "lua-5.2" next to the "Default one:" option.

### Adding the DaS.ScriptLib Lua Includes folder to your project as a User Library:
* Add DaS.ScriptLib to the User Libraries list:
  * From the menu bar up-top, go to `Window` -> `Preferences`
  * Go to the "User Libraries Page" under the "Lua" dropdown in the list on the left of the window.
  * Click the "New..." button on the right
  * Put "DaS.ScriptLib" as the name of the new user library.
  * Check the "Add to environment" checkbox
  * Click OK.
  * Click `DaS.ScriptLib` within the User Libraries list to select/highlight it
  * Click the "Add External folder..." button on the right.
  * Browse to the `Lua Includes` folder in your DaS-BRush folder.
    - If you downloaded a pre-built .zip of it, it will be in `<Extracted Zip File Directory>\Lua Includes`
    - If you cloned the GitHub repository and built the Visual Studio Solution, then folder will be located at `<Solution Directory>/Bin/Debug/Lua Includes`
  * Click OK.
  * You should now see the directory you just added listed underneath DaS.ScripLib's dropdown-node-thing in the User Libraries list.
  * Click the "Apply and Close" button on the "Preferences" window.
* Add the DaS.ScriptLib User Library to your project:
  * Right-click your project's root node within the "Script Explorer" panel on the left and go to "Properties".
  * Go to the "Build Path" Page, which is listed underneath the "Lua" dropdown on the left of the Properties window.
  * Go to the "Libraries" tab.
  * Click the "Add Library..." button on the right.
    * Select "Lua User Libraries" as the library type to add.
    * Click "Next".
    * Click the checkbox next to the **DaS.ScriptLib** entry you added to the User Libraries list in the previous section in order to select it.
    * Click "Finish" to close the "Add Library" wizard thing.
  * Click "Apply and Close" on the project properties window.

### Creating a new Lua script:
* Right-click the "src" folder under your Lua project in the "Script Explorer" panel.
* Go to `New` -> `Lua File`
* Enter a name for your script.
* Click "Finish" to end the totally-necessary 1-step wizard.
* To test if the autocomplete is working:
  * Type in: "player"
  * Add "." to the end
  * An auto-complete list should come up listing player's variables (note that it may take a couple of seconds to load the first time). ![Image of autocomplete list](https://i.imgur.com/7apBUZq.png)

### Adding DaS.ScriptLib.Executor as a Lua interpreter so that the "Run" button in Eclipse actually runs the selected script:
* On the menu bar, go to `Window` -> `Preferences`
* Go to the "Interpreters" page, underneath the "Lua" dropdown on the left.
* Click the "Add..." button on the right to open the "Add interpreter" window:
  * For the interpreter type, select "Lua 5.2"
  * For the interpreter executable, browse to your DaS-BRush folder and choose the `Dark Souls Script Executor.exe` file.
    - If you downloaded a pre-built .zip of it, it will be in `<Extracted Zip File Directory>\Dark Souls Script Executor.exe`
    - If you cloned the GitHub repository and built the Visual Studio Solution, then the executable will be located at `<Solution Directory>/Bin/Debug/Dark Souls Script Executor.exe`
  * For the Interpreter name, put `DaS.ScriptLib.Executor`
  * Delete any text inside of the "Interpreter arguments" textbox.
  * Make sure Linked Execution Environment is set to "lua-5.2"
* Check the box next to DaS.ScriptLib.Executor in the Interpreters list
  * DaS.ScriptLib.Executor should be the only one checked now.
* Click "Apply and Close" on the Preferences window.

### Weird Eclipse "gotchas" (for people used to Visual Studio, Notepad++, etc):
* Pressing the Tab key when the autocomplete menu is up will not select the text and add it, but rather, will focus the autocomplete menu, requiring you to use your mouse to re-click on the text area to continue typing. So far I haven't figured out how to change that behaviour. In the meantime, you can press the `Enter` key to select the autocomplete entries!
* Neither Alt + mouse drag nor middle click + mouse drag begin a block selection. If there is a block select function in Eclipse it's extremely well hidden!
* The "Stop" button (the red button in the image shown below!) for scripts when running them is located in the `Console` panel on the bottom.
  * To switch between Consoles for multiple scripts, you must click the second button on the top-right of the console panel (which either shows a list of all currently-running scripts' consoles or cycles through the currently selected one, depending on where you click it): ![Image of the button](https://i.imgur.com/Yrlzp2n.png)
  
### Eclipse "Pro"-Tips:
* Holding down Ctrl will underline any lua variable or function will you hover the mouse over. If you then click it like a URL, it will actually navigate to its definition in the code (it will even navigate to the Lua header dummy definitions so you can see all the empty functions, values of 0, etc)
* Hovering the mouse cursor over a lua variable or function without holding Ctrl will display documentation for that element. Example:
```lua
--[[
    Comment containing the text to appear in the documentation tooltip. 
    Can be either a multi-line comment or an end-of-line comment.
    Keep in mind that the tooltip GUI doesn't display line-breaks (although it does have word-wrapping *cough*unlike Visual Studio's*cough*)
]]
function someFunction(arg1, arg2)
```
![Image of provided documentation example in action](https://i.imgur.com/9WY7jbO.png)
