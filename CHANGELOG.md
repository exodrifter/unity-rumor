# Change Log
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/) 
and this project adheres to [Semantic Versioning](http://semver.org/).

## [Unreleased]

## [1.1.2] - 2017-12-23

### Changed
- More descriptive error for tokens after string

### Fixed
- If conditional statements no longer require a following else or elif
  statement


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

[Unreleased]: https://github.com/exodrifter/unity-rumor/compare/1.1.2...HEAD
[1.1.2]: https://github.com/exodrifter/unity-rumor/compare/1.1.1...1.1.2
[1.1.1]: https://github.com/exodrifter/unity-rumor/compare/1.1.0...1.1.1
[1.1.0]: https://github.com/exodrifter/unity-rumor/compare/1.0.1...1.1.0
[1.0.1]: https://github.com/exodrifter/unity-rumor/compare/1.0.0...1.0.1
[1.0.0]: https://github.com/exodrifter/unity-rumor/compare/0.2.1...1.0.0
[0.2.1]: https://github.com/exodrifter/unity-rumor/compare/0.2.0...0.2.1
[0.2.0]: https://github.com/exodrifter/unity-rumor/compare/0.1.1...0.2.0
[0.1.1]: https://github.com/exodrifter/unity-rumor/compare/0.1.0...0.1.1
[0.1.0]: https://github.com/exodrifter/unity-rumor/compare/215489c...0.1.0

