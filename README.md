# About
This is a repro project containing a bug when allowing microphone permissions using android's APIs.

## How to reproduce bug
1. Run the app on a phone with Android 6.0 Marshmallow or higher.
    - It only happens for runtime permissions. So it doesn't happen on Lollipop and lower.
2. Click Start Record button.
3. Allow permission
4. The `AudioRecord#StartRecording` method will throw an exception saying something like the `AudioRecord` is uninitialized. This is exactly what happens when you don't allow the permission.

## Working around the bug
1. Click Start Record button.
2. Allow permission and let it crash.
3. Open the app again.
4. Microphone now works.