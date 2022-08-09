using ErrorOr;

namespace BugTrackerAPI.ServiceErrors
{
    public static class Errors
    {
        public static class Bug
        {
            public static Error NotFound => Error.NotFound(
                code: "Bug.NotFound",
                description: "Bug not found"
                );
        }
    }
}
