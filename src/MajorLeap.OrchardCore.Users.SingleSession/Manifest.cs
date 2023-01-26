using MajorLeap.OrchardCore.Users.SingleSession;
using OrchardCore.Modules.Manifest;

[assembly: Module(
    Id = Startup._MODULE_ID,
    Name = "MajorLeap - OrchardCore Users Single Session",
    Author = "ALafi",
    Website = "www.majorleap.com",
    Version = "1.0.1",
    Description = "An OrchardCore module that prevents users from having more than one cookie-based sign-in sessions at the same time. Enabling this feature will end all users sessions and force a sign-out.",
    Category = "Security",
    Dependencies = new[] { "OrchardCore.Users" }/*,
    Tags = new[] { "Bootstrap", "Default" }*/
)]