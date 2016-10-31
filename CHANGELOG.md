# Change Log
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/) 
and this project adheres to [Semantic Versioning](http://semver.org/).

## [Unreleased]

### Added
- A `Running` property has been added to Rumor

### Changed
- `Rumor.Run()` will restart the script instead of throwing an exception if it
  is already running
- JumpToLabel and CallToLabel are now exposed publicly, and work even if the
  Rumor has not been started yet.

### Removed
- The RumorCodeExample has been removed as it's potentially
  misleading/confusing

## [0.1.0] - 2016-10-23
Initial release.

The following actions are available in this initial release:
- **Label** - Specifies a location.
- **Say** - Replaces the dialog in the current state.
- **Add** - Appends text to the end of the dialog in the current state.
- **Choice** - Adds a choice and all following choices to the current state.
- **Pause** - Pauses execution for a short amount of time.
- **Jump** - Modifies the stack so that execution will continue at the
  specified label.
- **Call** - Pushes a new stack frame onto the stack with the children nodes
  from the specified label.
- **Return** - Exits the current stack frame.

You can also:
- Bind methods to a Rumor's scope
- Subscribe to events on each new node

For examples of how Rumor works, see the `Examples/` folder.

[Unreleased]: https://github.com/exodrifter/unity-rumor/compare/0.1.0...HEAD
[0.1.0]: https://github.com/exodrifter/unity-rumor/compare/215489c...0.1.0

