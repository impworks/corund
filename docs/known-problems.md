=== APK signing ===

*Problem:*

When deploying an APK from VS, the following error appears:

    Keystore was tampered or password is incorrect

*Solution*:

Use uber-apk-signer.
Steps:

    1. Create a keystore inside Visual Studio UI. Specify a simple password without special chars!
    2. Download Uber APK signer: https://github.com/patrickfav/uber-apk-signer
    3. Copy the keystore from its origin to a folder containing the unsigned APK
    4. Invoke

        java -jar uas.jar --apks .\MyApk.apk --ks foobar.keystore --ksAlias foobar

=== Contents on iOS ===

*Problem:*

When running an app on iOS, calling `Content.Load<T>` throws an error.

*Cause:*

Bug in MonoGame:
https://github.com/MonoGame/MonoGame/issues/7897

*Solution:*

    1. Mark all contents as EmbeddedResource
    2. Use EmbeddedContentProvider to load it instead of the usual one
