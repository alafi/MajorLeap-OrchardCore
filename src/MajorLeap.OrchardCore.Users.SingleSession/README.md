# MajorLeap - OrchardCore Users Single Session

An OrchardCore module that prevents users from having more than one cookie-based sign-in sessions at the same time (i.e. prevents multiple browser sessions).

Any attempt to login by the user will force a sign-out from previous sessions.

## How to Use This Feature

Search for the NuGet package `MajorLeap - OrchardCore Users Single Session` and include it in your web project. Login to OrchardCore administration site then navigate to `Features` and enable this module.

**Note:**

Every time this module is enabled, all users will be forced to logout (including the current user). Just re-login again and verify that the feature is working properly
