# Change Log
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/) 
and this project adheres to [Semantic Versioning](http://semver.org/).

## [Unreleased]

### Added
* Allow `jump` or `call` when specifying a default choice for `choose`.
* Support for Functions in expressions and substitutions.
* Add `DropStack` which allows stacks to be manually discarded during execution,
  useful for turning calls into jumps.
* Add `KeepLastStack` which drops all but the current stack.

### Changed
* Internally, `ControlNode`s no longer store node blocks; the children nodes are
  flattened out into their own blocks like it is done for `AddChoiceNode`s.
* The character `-` can now be used in identifiers.
* The ability to clear a specific choice has been added, which also changed the
  signature of `OnClear`.

### Fixed
* Fixed bug where choosing would unintentionally pop the stack frame.
* `OnFinish` is only called once after execution finishes.

## [5.0.0] - 2021-03-01

### Added
* Added support for automatically advancing the dialog.
* Added `GetVars` to `RumorScope` as a way for getting all of the variables.
* Added `Choosing`, `Waiting`, and `Pausing` flags to see what kind of yield is
  currently active.
* Added serialization support for Rumor.
* Added ability to set a timeout and default jump on choose. For example,
  `choose in 2 seconds or jump foobar`.
* Added `call` command, which moves execution to the specified label and returns
  back after execution of the label is completed. This is the same behaviour
  that it had in Rumor 3.X.

### Changed
* Choices may now be empty.
* The `Label` and `Text` properties on `AddChoiceNode` are now public.
* Changed `jump` to work to move execution to the specified label and _not_
  return back after execution of the label is completed. This is the same
  behaviour that it had in Rumor 3.X.
* `choose` and `pause` now require the `{}` block syntax when specifying an
  amount of time.
* `choice` no longer uses `>` to prefix every line; instead, it works similar
  to `:` and `+` commands.

### Fixed
* Injecting nodes clears the current yield and continues execution instead of
  getting stuck.

### Removed
* `Exodrifter.Rumor.Engine.Util` is now internal instead of public.


## [4.0.0] - 2021-02-12

Rumor has been completely rehauled in this release. It now features...

* Simplified syntax
* Simplified engine API (it no longer requires the use of a coroutine)
* Stronger type checking

...among many other changes which will soon be described in detail on the wiki.

Some features have been lost in the rehaul. Specifically:

* You can no longer bind any function you want; they can only use the types
  `bool`, `double`, and `string` (this will likely never be re-added).
* You can no longer serialize the state (this will be fixed in a future
  release).


## [3.0.1] - 2018-11-06

### Fixed
* Fix `true` and `false` always being treated as variables when they should be
  a boolean literal
* Fix scope serialization not working when .NET 4.x runtime is selected


## [3.0.0] - 2018-09-29

### Changed
* `==` now only works when comparing values of the same type, with the notable
  exception of Int and Floats which can be compared to each other.

### Fixed
* Fix compilation error when comment directly follows a statement expecting a
  block
* Fix pauses not ending if a choice has been picked
* Better parsing errors
* Allow function and variable names to start with keywords
* Fix math operator precedence
* `null + null` returns `null` instead of throwing `InvalidOperationException`
* Fix comparing any non-empty string with another, different non-empty string
  with `==` would always return `true` instead of `false`
* Fix `>=` and `<=` not working due to comparing the wrapper type instead of
  the wrapped values
* Fix using `!` on a non-null object would throw an exception
* `clear choices` and `clear dialog` compile instead of throwing a compilation
  error


## [2.0.1] - 2018-04-15

### Changed
* Compiler performance improvements

### Fixed
* Fix `elif` and `else` compilation errors


## [2.0.0] - 2018-01-15

### Added
* You can now add the enum argument `cant_skip` to the end of a pause statement
  to ignore advances until the pause ends
* You can now add the enum argument `no_wait` to the end of an add or say
  statement to auto advance the dialog
* Variable substitution with `{` and `}` in strings is now supported

### Changed
* The compiler no longer uses a tokenizer except when parsing expressions
* Compiler errors are more specific and descriptive
* The `Exodrifter.Rumor.Lang` namespace has been renamed to
  `Exodrifter.Rumor.Language`
* All unit tests and examples are wrapped in a `UNITY_EDITOR` ifdef to make
  it easier to use this repository as a submodule in non-unity project
* Bindings are no longer stored in `Rumor`; instead, it is now stored in
  `Bindings`
* `Rumor.Run` has been renamed to `Rumor.Start` in order to match the language
  used in C# `Thread.Start` and Unity's `StartCoroutine` method to improve
  consistency


## [1.1.2] - 2017-12-23

### Changed
- More descriptive error for tokens after string

### Fixed
- If conditional statements no longer require a following else or elif
  statement
- Compiler tokenizes commas correctly now


## [1.1.1] - 2017-12-23

### Added
- Full Unity scene example

### Fixed
- Fix `+=`, `-=`, `*=`, `/=` operators not working
- Fix exception when script execution finishes in some cases


## [1.1.0] - 2017-12-19

### Added
- Added `OnAddChoice` and `OnRemoveChoice` events to RumorState

### Fixed
- Fix unit test failures caused by Tokenize performance optimization code
- Fix Rumor bindings no longer attempting to convert arguments


## [1.0.1] - 2017-12-19

### Changed
- `Rumor.CallBinding` no longer calls DynamicInvoke and is much faster
- Tokenize step of compilation is much faster

### Fixed
- Fix `Rumor.Choosing` throwing an error if Rumor has not been started
- Fix scope not being initialized when passed as null when constructing Rumor
- Fix Add statements behaving like Say statements
- Fix null pointer exception in Equals expression
- Fix incorrect equality checking
- Fix improper deserialization of values caused by wrapper types (Json.NET will
  wrap object values in its own type, JValue, which causes logic to fail)
- Rumor no longer clears the scope when starting
- Null ObjectValues are now treated the same as an uninitialized (null)
  variable


## [1.0.0] - 2017-08-16

### Added
- Added the ability to specify a clear for just dialog or just choices
- Added the default binding `_choice`, which returns the contents of the
  last chosen choice
- Added the `-=`, `+=`, `/=`, and `*=` assignment operators
- Added the `<`, `<=`, `>`, and `>=` comparison operators

### Changed
- The `OnClear` event in RumorState has been changed to use a `ClearType` enum,
  which specifies if everything, just choices, or just dialog was cleared
- The Pause command will now wait for an advance if the time is less than or
  equal to 0
- Multiple methods of the same name can be binded as long as they have a
  different number of input parameters


## [0.2.1] - 2016-11-14

### Added
- Added an `OnDialogAdd`, `OnDialogSet`, and `OnClear` events to RumorState
- Added the Clear command
- Added an `OnWaitForAdvance` and `OnWaitForChoose` event to Rumor
- Added a `Cancelled` property to Rumor

### Changed
- AutoAdvance is now a time instead of a toggle, which indicates the amount of
  time before the Rumor will attempt to automatically advance
- Bindings are now stored in the Rumor instead of the Scope

### Fixed
- Fix null variables not handled properly in expressions
- Fix no boolean literals parsing in `RumorCompiler`
- Fix nodes not checking for null values
- Fix `If` conditional not checking for null values


## [0.2.0] - 2016-11-05

### Added
- Added an `AutoAdvance` property to Rumor that causes it to automatically
  advance if possible
- Added `Cancel` and `Finish` methods to Rumor
- Added `CancelCount` and `FinishCount` properties to Rumor
- Added a `Choose` node
- Added the ability to use one-line comments

### Changed
- Added `SetupDefaultBindings` convenience method to Rumor for common bindings
- `Choice` nodes no longer automatically wait for a choice at the end of a
  chain of choices. Instead, use the `Choose` node to wait for a choice


## [0.1.1] - 2016-10-31

### Added
- Added a `Running` property to Rumor
- Added `OnStart` and `OnFinish` events to Rumor
- Added `OnVarSet` event to Scope

### Changed
- `Rumor.Run()` will restart the script instead of throwing an exception if it
  is already running
- `JumpToLabel` and `CallLabel` are now exposed publicly, and work even if the
  Rumor has not been started yet
- Rumor constructors now provide a convenience method that takes a string
  directly
- You can now pass Rumor a scope to use

### Removed
- The RumorCodeExample has been removed as it's potentially
  misleading/confusing


## [0.1.0] - 2016-10-23
Initial release.

The following actions are available in this initial release:
- **Label** - Specifies a location
- **Say** - Replaces the dialog in the current state
- **Add** - Appends text to the end of the dialog in the current state
- **Choice** - Adds a choice and all following choices to the current state
- **Pause** - Pauses execution for a short amount of time
- **Jump** - Modifies the stack so that execution will continue at the
  specified label
- **Call** - Pushes a new stack frame onto the stack with the children nodes
  from the specified label
- **Return** - Exits the current stack frame

You can also:
- Bind methods to a Rumor's scope
- Subscribe to events on each new node

For examples of how Rumor works, see the `Examples/` folder.

[Unreleased]: https://github.com/exodrifter/unity-rumor/compare/5.0.0...HEAD
[5.0.0]: https://github.com/exodrifter/unity-rumor/compare/4.0.0...5.0.0
[4.0.0]: https://github.com/exodrifter/unity-rumor/compare/3.0.1...4.0.0
[3.0.1]: https://github.com/exodrifter/unity-rumor/compare/2.0.1...3.0.1
[3.0.0]: https://github.com/exodrifter/unity-rumor/compare/2.0.1...3.0.0
[2.0.1]: https://github.com/exodrifter/unity-rumor/compare/2.0.0...2.0.1
[2.0.0]: https://github.com/exodrifter/unity-rumor/compare/1.1.2...2.0.0
[1.1.2]: https://github.com/exodrifter/unity-rumor/compare/1.1.1...1.1.2
[1.1.1]: https://github.com/exodrifter/unity-rumor/compare/1.1.0...1.1.1
[1.1.0]: https://github.com/exodrifter/unity-rumor/compare/1.0.1...1.1.0
[1.0.1]: https://github.com/exodrifter/unity-rumor/compare/1.0.0...1.0.1
[1.0.0]: https://github.com/exodrifter/unity-rumor/compare/0.2.1...1.0.0
[0.2.1]: https://github.com/exodrifter/unity-rumor/compare/0.2.0...0.2.1
[0.2.0]: https://github.com/exodrifter/unity-rumor/compare/0.1.1...0.2.0
[0.1.1]: https://github.com/exodrifter/unity-rumor/compare/0.1.0...0.1.1
[0.1.0]: https://github.com/exodrifter/unity-rumor/compare/215489c...0.1.0
