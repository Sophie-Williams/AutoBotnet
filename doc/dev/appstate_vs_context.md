
# AppState vs Context

It may not always be obvious when to use `SAppState` or `SContext`, or even `SContext.database`, because their usages might seem similar at first glance. However, there are (mostly) specific reasons to use each one.

## SContext

This is generally for transient services, or "instantiated" services, which are created from stored data, and can manipulate stored data, but these services are not actually stored. For example, `executors` and `notificationPipeline` require user data to function, and can read and write user data, but are not themselves stored, rather, they are created and used to manage data indirectly.

## SAppState

This is for mostly "live" or "hot" data, which is frequently manipulated. It is persisted either manually upon important operations (such as new user account creation), or upon graceful application exit, where it safely unloads services and persists data. We use the app state storage for manipulating data (that will eventually be stored to a `state` database) in memory, where it is much less expensive than accessing the disk. For example, the app state stores `UserMetrics`, the `WorldMap`, and the `EntityBag` storage.

## Main Database

The main database stores less-frequently modified data, such as user account records and scripts. Since these are relatively less important to be easily manipulated in memory, and since it is more important that their changes be persisted immediately, the database is used for user accounts.
