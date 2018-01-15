<p align="center">
    <img src="https://raw.githubusercontent.com/wiki/exodrifter/unity-rumor/Images/Logo.png">
</p>

Rumor
=====
Rumor is a free, open source narrative content framework meant to be integrated
into any Unity3D game that needs a dialog system. This library is a good fit
for teams that want a scripting language for writers to use and need an
easy-to-use API for running the dialog. This library does not and will not
contain any rendering capabilities.

You can download official releases at the [releases page](https://github.com/exodrifter/unity-rumor/releases).

To learn more about Rumor, check the official [wiki](https://github.com/exodrifter/unity-rumor/wiki).

Change Log
----------
Please see the file named [`CHANGELOG.md`](CHANGELOG.md).

Licensing
---------
Please see the file named [`LICENSE.md`](LICENSE.md).

Usage
-----
**Example.txt**
```
label start:
    say "Hi!"
    say "Is this working?"

    choice "Yes!":
        say "Great!"
    choice "No.":
        say "Darn..."
        pause 0.5
        add "Maybe next time."
        jump end
    choose

$ apples = get_apples()
$ pears = get_pears()

if apples == pears:
    $ pears += 1

say "I have " + apples + " apples."
say "You have { pears } pears."
say "Who has more fruits?"

choice "I do.":
    if apples < pears:
        jump correct
    jump incorrect
choice "You do.":
    if apples > pears:
        jump correct
    jump incorrect
choose

label correct:
    say "That's right!"
    jump end

label incorrect:
    say "That's wrong!"
    jump end

label end:
    say "Well, thanks for stopping by!"
    say "See you next time!"
```

**Example.cs**
```
using Exodrifter.Rumor.Engine;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class RumorScriptExample : MonoBehaviour
{
    private Rumor rumor;
    public Text text;

    void Awake()
    {
        rumor = new Rumor(File.ReadAllText("Example.txt"));
        rumor.Scope.DefaultSpeaker = "Narrator";

        rumor.Bindings.Bind("get_apples", () => { return Random.Range(2, 6); });
        rumor.Bindings.Bind("get_pears", () => { return Random.Range(2, 6); });

        StartCoroutine(rumor.Start());
    }

    void Update()
    {
        if (rumor == null) {
            text.text = "";
            return;
        }

        if (rumor.Choosing) {
            int num = 1;
            text.text = "";

            foreach (var choice in rumor.State.Choices) {
                text.text += num + ") " + choice + "\n";
                num++;
            }
        }
        else {
            text.text = rumor.State.Dialog["Narrator"];
        }

        rumor.Update(Time.deltaTime);

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) {
            rumor.Advance();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            rumor.Choose(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            rumor.Choose(1);
        }
    }
}
```

See `Examples/RumorScriptExample.cs` to run this example in Unity.
