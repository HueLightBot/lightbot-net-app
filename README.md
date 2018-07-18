# HueLightBot v2 (beta)
A .NET App for managing Lightbot

## Example of the bot working:
  https://clips.twitch.tv/PlumpPerfectSlothNotATK

## Installation:
 1. Download the latest release on the releases page (https://github.com/HueLightBot/lightbot-net-app/releases)
 2. Unzip the contents to a folder.
 3. Run the HueLightBot executable (the one with our icon).
 4. Set your settings, make sure to include your twitch channel in the channel box at the top.
 5. Click the "Pair Hue Bridge" button.
 6. Press the button on your hue bridge.
 7. Return to the app and press "Ok".
 8. Click the "Apply" and then the "Start" buttons 

## Chat Commands:
`cheer200 #FF0000`   - Changes the color of the hue lights to #FF0000. This supports all cheermotes as well. You can use a resub or sub message to change the color as well. If this is over the large cheer floor value, the lights will trigger the large cheer action before switching to the color provided.

`!off cheer5000`     - Turns the lights off.

`!on cheer500`       - Turns the lights on.

`!setlights #FF0000` - Forcefully sets the lights to a color. This can only be used by the users specified in the command permissions section.

`!colorloop`         - Force the bulbs into a color loop for 30 seconds. This can only be used by the users specified in the command permissions section.
