# QuickBrowserSwitcher
A utility that handles your default browser on the fly.

I'll make this actually usable, one day.

## How does it work?

There's two parts to how this application works.

First, there's a tiny application that is set as the default browser. When called, this application will check what browser to start and redirect the URI or file handler to the respective browser.

The other part is a little system tray application. When clicked, it will switch what browser is being used as your browser. This tray application is not required to be running to launch a browser, it just updates a registry key.
